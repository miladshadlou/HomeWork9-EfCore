namespace HomeWork9.Domain.Entities;

public class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    private readonly List<BookCategory> _bookCategories = new();

    public IReadOnlyCollection<BookCategory> BookCategories
        => _bookCategories.AsReadOnly();
}