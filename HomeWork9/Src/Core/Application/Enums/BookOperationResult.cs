namespace HomeWork9.Application.Enums;

public enum BookOperationResult
{
    Success,
    BookNotFound,
    PublisherNotFound,
    IsbnAlreadyExists,
    InvalidAuthorIds,
    InvalidCategoryIds,
    BookHasOrderHistory
}