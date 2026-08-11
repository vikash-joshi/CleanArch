namespace ProductManagement.Application.DTOs;

public record ProductListItemDto(
    Guid Id,
    string Name,
    decimal Price,
    string? CategoryName,
    string StockStatus   // "In Stock" / "Low Stock" / "Out of Stock" — computed, not raw stock count
);