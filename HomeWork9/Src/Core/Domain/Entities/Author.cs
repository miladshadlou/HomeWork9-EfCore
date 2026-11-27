namespace HomeWork9.Domain.Entities
{
    public class Author
    {
        public int Id { get; set; }

        public string FullName { get; set; } = null!;

        public string Biography { get; set; } = null!;

        private readonly List<BookAuthor> _bookAuthors = new();

        public IReadOnlyCollection<BookAuthor> BookAuthors
            => _bookAuthors.AsReadOnly();
    }
}
