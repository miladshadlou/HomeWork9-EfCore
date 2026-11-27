using HomeWork9.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using HomeWork9.Application.DTOs.Books;
using HomeWork9.Application.Enums;

namespace HomeWork9.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpPost]
    public async Task<IActionResult> AddBook(
     BookDto bookDto,
     CancellationToken cancellationToken)
    {
        var result = await _bookService.AddBookAsync(
            bookDto,
            cancellationToken);

        return HandleBookOperationResult(
                    result,
                    Ok(new
                    {
                        message = "Book added or stock increased successfully."
                    }));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateBook(
    int id,
    BookDto bookDto,
    CancellationToken cancellationToken)
    {
        var result = await _bookService.UpdateBookAsync(
            id,
            bookDto,
            cancellationToken);

        return HandleBookOperationResult(
                result,
                NoContent());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteBook(
        int id,
        [FromQuery] BookDeletionMode mode = BookDeletionMode.Soft,
        CancellationToken cancellationToken = default)
    {
        if (!Enum.IsDefined(typeof(BookDeletionMode), mode))
        {
            return BadRequest(
                "Invalid deletion mode. Use 0 for Soft or 1 for Hard.");
        }

        var result = await _bookService.DeleteBookAsync(
            id,
            mode,
            cancellationToken);

        return HandleBookOperationResult(
                result,
                NoContent());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetBookById(
    int id,
    CancellationToken cancellationToken)
    {
        var book = await _bookService.GetBookByIdAsync(
            id,
            cancellationToken);

        if (book is null)
            return NotFound("Book not found.");

        return Ok(book);
    }

    [HttpGet("titles")]
    public async Task<IActionResult> GetBookTitles(
    CancellationToken cancellationToken)
    {
        var titles = await _bookService.GetBookTitlesAsync(
            cancellationToken);

        return Ok(titles);
    }

    [HttpGet]
    public async Task<IActionResult> GetBooks(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10,
    CancellationToken cancellationToken = default)
    {
        if (pageNumber < 1)
            return BadRequest("Page number must be greater than zero.");

        if (pageSize < 1 || pageSize > 100)
            return BadRequest("Page size must be between 1 and 100.");

        var (books, totalCount) = await _bookService.GetBooksAsync(
            pageNumber,
            pageSize,
            cancellationToken);

        var totalPages = (int)Math.Ceiling(
            (double)totalCount / pageSize);

        return Ok(new
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalCount = totalCount,
            TotalPages = totalPages,
            Books = books
        });
    }

    [HttpPatch("{id:int}/restore")]
    public async Task<IActionResult> RestoreBook(
    int id,
    CancellationToken cancellationToken)
    {
        var result = await _bookService.RestoreBookAsync(
            id,
            cancellationToken);

        return HandleBookOperationResult(
                 result,
                 NoContent());
    }

    // تبدیل BookOperationResult به پاسخ HTTP
    private IActionResult HandleBookOperationResult(
    BookOperationResult result,
    IActionResult successResponse)
    {
        return result switch
        {
            BookOperationResult.Success =>
                successResponse,

            BookOperationResult.BookNotFound =>
                NotFound("Book not found."),

            BookOperationResult.PublisherNotFound =>
                BadRequest("Publisher does not exist."),

            BookOperationResult.IsbnAlreadyExists =>
                Conflict("ISBN already exists."),

            BookOperationResult.InvalidAuthorIds =>
                BadRequest("One or more author IDs are invalid."),

            BookOperationResult.InvalidCategoryIds =>
                BadRequest("One or more category IDs are invalid."),

            BookOperationResult.BookHasOrderHistory =>
                Conflict("Book has order history and cannot be permanently deleted."),

            _ => StatusCode(500)
        };
    }
}