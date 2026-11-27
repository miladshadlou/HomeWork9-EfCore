using HomeWork9.Application.DTOs.Books;
using HomeWork9.Application.Enums;
using HomeWork9.Application.Interfaces;
using HomeWork9.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HomeWork9.Application.Services;

public class BookService : IBookService
{
    private readonly IBookStoreDbContext _context;

    public BookService(IBookStoreDbContext context)
    {
        _context = context;
    }

    // متد اضافه کردن کتاب
    public async Task<BookOperationResult> AddBookAsync(
    BookDto bookDto,
    CancellationToken cancellationToken = default)
    {
        // 1. بررسی وجود کتاب با عنوان مشابه
        var existingBook = await _context.Books
            .FirstOrDefaultAsync(
                b => b.Title == bookDto.Title.Trim(),
                cancellationToken);

        if (existingBook is not null)
        {
            existingBook.IncreaseStock();
        }
        else
        {
            // 2. بررسی وجود ناشر
            var publisherExists = await _context.Publishers
                .AnyAsync(
                    p => p.Id == bookDto.PublisherId,
                    cancellationToken);

            if (!publisherExists)
                return BookOperationResult.PublisherNotFound;


            // 3. بررسی تکراری نبودن ISBN
            var normalizedIsbn = bookDto.ISBN.Trim();

            var isbnExists = await _context.Books
                .AnyAsync(
                    b => b.ISBN == normalizedIsbn,
                    cancellationToken);

            if (isbnExists)
                return BookOperationResult.IsbnAlreadyExists;


            // 4. اعتبارسنجی ارتباطات: نویسنده‌ها و دسته بندی ها
            var validationResult = await ValidateBookRelationsAsync(
                    bookDto.AuthorIds,
                    bookDto.CategoryIds,
                    cancellationToken);

            if (validationResult != BookOperationResult.Success)
                return validationResult;

            // 5. ساخت موجودیت کتاب
            var book = new Book(
                bookDto.Title,
                bookDto.ISBN,
                bookDto.Price,
                bookDto.Stock,
                bookDto.PublisherId,
                bookDto.Description,
                bookDto.CoverImageUrl);


            // 6. تنظیم ارتباط‌های کتاب
            book.SetAuthors(bookDto.AuthorIds);
            book.SetCategories(bookDto.CategoryIds);

            _context.Books.Add(book);
        }

        // 7. ذخیره تغییرات
        await _context.SaveChangesAsync(cancellationToken);

        return BookOperationResult.Success;
    }

    // متد ویرایش کتاب
    public async Task<BookOperationResult> UpdateBookAsync(
    int bookId,
    BookDto bookDto,
    CancellationToken cancellationToken = default)
    {
        // 1. پیدا کردن کتاب و بارگذاری ارتباط‌های آن
        var book = await _context.Books
            .Include(b => b.BookAuthors)
            .Include(b => b.BookCategories)
            .FirstOrDefaultAsync(
                b => b.Id == bookId,
                cancellationToken);

        if (book is null)
            return BookOperationResult.BookNotFound;


        // 2. بررسی وجود ناشر
        var publisherExists = await _context.Publishers
            .AnyAsync(
                p => p.Id == bookDto.PublisherId,
                cancellationToken);

        if (!publisherExists)
            return BookOperationResult.PublisherNotFound;


        // 3. بررسی تکراری نبودن ISBN
        var normalizedIsbn = bookDto.ISBN.Trim();

        var isbnAlreadyExists = await _context.Books
            .AnyAsync(
                b => b.ISBN == normalizedIsbn && b.Id != bookId,
                cancellationToken);

        if (isbnAlreadyExists)
            return BookOperationResult.IsbnAlreadyExists;


        // 4. اعتبارسنجی ارتباطات: نویسنده‌ها و دسته بندی ها
        var validationResult = await ValidateBookRelationsAsync(
                bookDto.AuthorIds,
                bookDto.CategoryIds,
                cancellationToken);

        if (validationResult != BookOperationResult.Success)
            return validationResult;

        // 5. اعمال تغییرات روی کتاب
        book.UpdateDetails(
            bookDto.Title,
            bookDto.ISBN,
            bookDto.Price,
            bookDto.Stock,
            bookDto.PublisherId,
            bookDto.Description,
            bookDto.CoverImageUrl);

        // 6. به‌روزرسانی نویسنده‌ها و دسته‌بندی‌ها
        book.SetAuthors(bookDto.AuthorIds);
        book.SetCategories(bookDto.CategoryIds);


        // 7. ذخیره تغییرات
        await _context.SaveChangesAsync(cancellationToken);

        return BookOperationResult.Success;
    }

    // متد حذف کتاب(soft delete)
    public async Task<BookOperationResult> DeleteBookAsync(
    int bookId,
    BookDeletionMode mode,
    CancellationToken cancellationToken = default)
    {
        var book = await _context.Books
            .FirstOrDefaultAsync(
                b => b.Id == bookId,
                cancellationToken);

        if (book is null)
            return BookOperationResult.BookNotFound;

        switch (mode)
        {
            case BookDeletionMode.Soft:
                book.Deactivate();
                break;

            case BookDeletionMode.Hard:

                var hasOrderHistory = await _context.OrderItems
                    .AnyAsync(
                        oi => oi.BookId == bookId,
                        cancellationToken);

                if (hasOrderHistory)
                    return BookOperationResult.BookHasOrderHistory;

                _context.Books.Remove(book);
                break;

            default:
                throw new ArgumentOutOfRangeException(nameof(mode));
        }

        await _context.SaveChangesAsync(cancellationToken);

        return BookOperationResult.Success;
    }

    // متد نمایش اطلاعات کتاب
    public async Task<BookDto?> GetBookByIdAsync(
    int bookId,
    CancellationToken cancellationToken = default)
    {
        return await _context.Books
            .Where(b => b.Id == bookId && b.IsActive)
            .Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                ISBN = b.ISBN,
                Price = b.Price,
                Stock = b.Stock,
                PublisherId = b.PublisherId,
                Description = b.Description,
                CoverImageUrl = b.CoverImageUrl,

                AuthorIds = b.BookAuthors
                    .Select(ba => ba.AuthorId)
                    .ToList(),

                CategoryIds = b.BookCategories
                    .Select(bc => bc.CategoryId)
                    .ToList()
            })
            .FirstOrDefaultAsync(cancellationToken);
    }

    // نمایش لیست عنوان کتاب
    public async Task<List<string>> GetBookTitlesAsync(
        CancellationToken cancellationToken = default)
    {
        return await _context.Books
            .AsNoTracking()
            .Where(b => b.IsActive)
            .OrderBy(b => b.Title)
            .Select(b => b.Title)
            .ToListAsync(cancellationToken);
    }

    // نمایش لیست اطلاعات کتاب ها صفحه بندی شده
    public async Task<(List<BookDto> Books, int TotalCount)>
    GetBooksAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        if (pageNumber < 1)
            throw new ArgumentOutOfRangeException(
                nameof(pageNumber),
                "Page number must be greater than zero.");

        if (pageSize < 1 || pageSize > 100)
            throw new ArgumentOutOfRangeException(
                nameof(pageSize),
                "Page size must be between 1 and 100.");

        var query = _context.Books
            .Where(b => b.IsActive);

        // تعداد کل کتاب‌های فعال
        var totalCount = await query.CountAsync(
            cancellationToken);

        // محاسبه تعداد رکوردهایی که باید رد شوند
        var skip = (long)(pageNumber - 1) * pageSize;

        // دریافت کتاب‌های صفحه موردنظر
        var books = await query
            .OrderBy(b => b.Id)
            .Skip((int)skip)
            .Take(pageSize)
            .Select(b => new BookDto
            {
                Id = b.Id,
                Title = b.Title,
                ISBN = b.ISBN,
                Price = b.Price,
                Stock = b.Stock,
                PublisherId = b.PublisherId,
                Description = b.Description,
                CoverImageUrl = b.CoverImageUrl,

                AuthorIds = b.BookAuthors
                    .Select(ba => ba.AuthorId)
                    .ToList(),

                CategoryIds = b.BookCategories
                    .Select(bc => bc.CategoryId)
                    .ToList()
            })
            .ToListAsync(cancellationToken);

        return (books, totalCount);
    }

    // برگرداندن رکورد soft delete
    public async Task<BookOperationResult> RestoreBookAsync(
    int bookId,
    CancellationToken cancellationToken = default)
    {
        var book = await _context.Books
            .FirstOrDefaultAsync(
                b => b.Id == bookId,
                cancellationToken);

        if (book is null)
            return BookOperationResult.BookNotFound;

        book.Activate();

        await _context.SaveChangesAsync(cancellationToken);

        return BookOperationResult.Success;
    }

    // اعتبارسنجی ارتباطات: نویسنده و دسته بندی
    private async Task<BookOperationResult> ValidateBookRelationsAsync(
    IEnumerable<int> authorIds,
    IEnumerable<int> categoryIds,
    CancellationToken cancellationToken)
    {
        var uniqueAuthorIds = authorIds
            .Distinct()
            .ToArray();

        var authorsCount = await _context.Authors
            .CountAsync(
                a => uniqueAuthorIds.Contains(a.Id),
                cancellationToken);

        if (authorsCount != uniqueAuthorIds.Length)
            return BookOperationResult.InvalidAuthorIds;


        var uniqueCategoryIds = categoryIds
            .Distinct()
            .ToArray();

        var categoriesCount = await _context.Categories
            .CountAsync(
                c => uniqueCategoryIds.Contains(c.Id),
                cancellationToken);

        if (categoriesCount != uniqueCategoryIds.Length)
            return BookOperationResult.InvalidCategoryIds;

        return BookOperationResult.Success;
    }
}