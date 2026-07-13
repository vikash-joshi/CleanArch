public static class ProductMappingExtensions
{
    public static ProductDto ToDto(this Product product) =>
        new(product.Id, product.Name, product.Description, product.Price.Amount, product.StockQuantity);
}