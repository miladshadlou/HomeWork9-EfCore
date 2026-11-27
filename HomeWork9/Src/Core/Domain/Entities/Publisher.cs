namespace HomeWork9.Domain.Entities;

public class Publisher
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Website { get; set; } = null!;

    public ICollection<Book> Books { get; set; }
        = new List<Book>();
}

