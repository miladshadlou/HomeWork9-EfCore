namespace HomeWork9.Domain.Entities;

public class Book
{
    public int Id { get; private set; }

    public string Title { get; private set; } = null!;

    public string ISBN { get; private set; } = null!;

    public string? Description { get; private set; }

    public decimal Price { get; private set; }

    public int Stock { get; private set; }

    public int PublisherId { get; private set; }

    public Publisher Publisher { get; private set; } = null!;

    public bool IsActive { get; private set; } = true;

    public string? CoverImageUrl { get; private set; }

    // Private backing fields
    private readonly List<BookAuthor> _bookAuthors = new();

    private readonly List<BookCategory> _bookCategories = new();

    // Read-only navigation collections
    public IReadOnlyCollection<BookAuthor> BookAuthors
        => _bookAuthors.AsReadOnly();

    public IReadOnlyCollection<BookCategory> BookCategories
        => _bookCategories.AsReadOnly();

    private Book()
    {
    }
    public Book(
    string title,
    string isbn,
    decimal price,
    int stock,
    int publisherId,
    string? description = null,
    string? coverImageUrl = null)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException(
                "Book title is required.", nameof(title));

        if (string.IsNullOrWhiteSpace(isbn))
            throw new ArgumentException(
                "Book ISBN is required.", nameof(isbn));

        if (stock < 0)
            throw new ArgumentOutOfRangeException(
                nameof(stock), "Stock cannot be negative.");

        if (publisherId <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(publisherId));

        Title = title.Trim();
        ISBN = isbn.Trim();
        Description = description;

        ChangePrice(price);

        Stock = stock;
        PublisherId = publisherId;
        CoverImageUrl = coverImageUrl;
    }
    public void IncreaseStock()
    {
        Stock = checked(Stock + 1);
    }

    public void ChangePrice(decimal newPrice)
    {
        if (newPrice < 0)
            throw new ArgumentOutOfRangeException(
                nameof(newPrice),
                "Book price cannot be negative.");

        Price = newPrice;
    }
    public void UpdateDetails(
    string title,
    string isbn,
    decimal price,
    int stock,
    int publisherId,
    string? description,
    string? coverImageUrl)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException(
                "Book title is required.", nameof(title));

        if (string.IsNullOrWhiteSpace(isbn))
            throw new ArgumentException(
                "Book ISBN is required.", nameof(isbn));

        if (stock < 0)
            throw new ArgumentOutOfRangeException(
                nameof(stock), "Stock cannot be negative.");

        if (publisherId <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(publisherId));

        // اعتبارسنجی قیمت، قبل از تغییر سایر اطلاعات
        ChangePrice(price);

        Title = title.Trim();
        ISBN = isbn.Trim();
        Stock = stock;
        PublisherId = publisherId;
        Description = description;
        CoverImageUrl = coverImageUrl;
    }

    public void SetAuthors(IEnumerable<int> authorIds)
    {
        ArgumentNullException.ThrowIfNull(authorIds);

        var requestedIds = authorIds.ToHashSet();

        if (requestedIds.Any(id => id <= 0))
            throw new ArgumentException(
                "Author IDs must be positive.",
                nameof(authorIds));

        // Find existing author IDs
        var existingIds = _bookAuthors
            .Select(ba => ba.AuthorId)
            .ToHashSet();

        // Remove authors that are no longer selected
        var authorsToRemove = _bookAuthors
            .Where(ba => !requestedIds.Contains(ba.AuthorId))
            .ToList();

        foreach (var author in authorsToRemove)
        {
            _bookAuthors.Remove(author);
        }

        // Add newly selected authors
        foreach (var authorId in requestedIds.Except(existingIds))
        {
            _bookAuthors.Add(new BookAuthor(this, authorId));
        }
    }

    public void SetCategories(IEnumerable<int> categoryIds)
    {
        ArgumentNullException.ThrowIfNull(categoryIds);

        var requestedIds = categoryIds.ToHashSet();

        if (requestedIds.Any(id => id <= 0))
            throw new ArgumentException(
                "Category IDs must be positive.",
                nameof(categoryIds));

        var existingIds = _bookCategories
            .Select(bc => bc.CategoryId)
            .ToHashSet();

        var categoriesToRemove = _bookCategories
            .Where(bc => !requestedIds.Contains(bc.CategoryId))
            .ToList();

        foreach (var category in categoriesToRemove)
        {
            _bookCategories.Remove(category);
        }

        foreach (var categoryId in requestedIds.Except(existingIds))
        {
            _bookCategories.Add(new BookCategory(this, categoryId));
        }
    }
    public void Deactivate()
    {
        IsActive = false;
    }
    public void Activate()
    {
        IsActive = true;
    }

}

