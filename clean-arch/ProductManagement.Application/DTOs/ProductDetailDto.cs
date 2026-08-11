namespace ProductManagement.Application.DTOs;

public record ProductDetailDto(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    int StockQuantity,
    Guid? CategoryId,
    string? CategoryName,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);