using HomeWork9.Application.DTOs.Books;
using HomeWork9.Application.Enums;

namespace HomeWork9.Application.Interfaces;

public interface IBookService
{
    Task<BookOperationResult> AddBookAsync(
        BookDto bookDto,
        CancellationToken cancellationToken = default);

    Task<BookOperationResult> UpdateBookAsync(
        int bookId,
        BookDto bookDto,
        CancellationToken cancellationToken = default);

    Task<BookOperationResult> DeleteBookAsync(
        int bookId,
        BookDeletionMode mode,
        CancellationToken cancellationToken = default);

    Task<BookDto?> GetBookByIdAsync(
        int bookId,
        CancellationToken cancellationToken = default);

    Task<List<string>> GetBookTitlesAsync(
        CancellationToken cancellationToken = default);

    Task<(List<BookDto> Books, int TotalCount)> GetBooksAsync(
        int pageNumber,
        int pageSize,
        CancellationToken cancellationToken = default);

    Task<BookOperationResult> RestoreBookAsync(
        int bookId,
        CancellationToken cancellationToken = default);
}