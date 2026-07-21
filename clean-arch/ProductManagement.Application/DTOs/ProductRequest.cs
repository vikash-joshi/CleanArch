public record CreateProductRequest(string Name, string Description, decimal Price, int StockQuantity);
public record UpdateProductRequest(string id, string Name, string Description, decimal Price, int StockQuantity);

public record DeleteProductRequest(string id);