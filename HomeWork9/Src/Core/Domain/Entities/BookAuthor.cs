namespace HomeWork9.Domain.Entities;

public class BookAuthor
{
    // مخصوص EF Core
    private BookAuthor()
    {
    }

    // ایجاد ارتباط توسط Book
    internal BookAuthor(Book book, int authorId)
    {
        ArgumentNullException.ThrowIfNull(book);

        if (authorId <= 0)
            throw new ArgumentOutOfRangeException(nameof(authorId));

        Book = book;
        AuthorId = authorId;
    }

    public int BookId { get; private set; }

    public Book Book { get; private set; } = null!;

    public int AuthorId { get; private set; }

    public Author Author { get; private set; } = null!;
}