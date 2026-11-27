using System.ComponentModel.DataAnnotations;

namespace HomeWork9.Application.DTOs.Books;

public class BookDto
{
    public int Id { get; set; }

    [Required]
    [StringLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string ISBN { get; set; } = string.Empty;

    [Range(typeof(decimal), "0", "9999999999999999.99")]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue)]
    public int Stock { get; set; }

    [Range(1, int.MaxValue)]
    public int PublisherId { get; set; }

    [StringLength(2000)]
    public string? Description { get; set; }

    [StringLength(500)]
    public string? CoverImageUrl { get; set; }

    [Required]
    public List<int> AuthorIds { get; set; } = new();

    [Required]
    public List<int> CategoryIds { get; set; } = new();
}