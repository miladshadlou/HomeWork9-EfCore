namespace HomeWork9.Domain.Entities;

public class BookCategory
{
    // مخصوص EF Core
    private BookCategory()
    {
    }

    // ایجاد ارتباط توسط Book
    internal BookCategory(Book book, int categoryId)
    {
        ArgumentNullException.ThrowIfNull(book);

        if (categoryId <= 0)
            throw new ArgumentOutOfRangeException(nameof(categoryId));

        Book = book;
        CategoryId = categoryId;
    }

    public int BookId { get; private set; }

    public Book Book { get; private set; } = null!;

    public int CategoryId { get; private set; }

    public Category Category { get; private set; } = null!;
}