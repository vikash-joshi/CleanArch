using System.Diagnostics;
using ProductManagement.Application.DTOs;

public static class ProductMappingExtensions
{
    public static ProductDto ToDto(this Product product) =>
        new(product.Id, product.Name, product.Description, product.Price.Amount, product.StockQuantity);

    public static CategoryDTO ToDto(this Category category) =>
        new(category.Id, category.Name, category.Description);


    public static ProductListItemDto ToListItemDto(this Product product) =>
        new(
        product.Id,
        product.Name,
        product.Price.Amount,
        product.CategoryName,
        ProductMappingExtensions.Get_Stock_Status((product.StockQuantity))
         );

    public static ProductDetailDto ToDetailDto(this Product product)
    => new(
    product.Id,
    product.Name,
    product.Description,
    product.Price.Amount,
    product.StockQuantity,
    product.CategoryId,
    product.CategoryName,
    product.CreatedAt,
    product.UpdatedAt
     );

    public static string Get_Stock_Status(int Quantity) => Quantity switch
    {
        <= 0 => "Out Of Stock",
        <= 10 => "Low Stock",
        _ => "In Stock"

    };


}