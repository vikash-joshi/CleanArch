using ProductManagement.Domain.Factories;

namespace ProductManagement.Application.Tests.Products.FactoryProduct;

public class ProductFactoryTests
{
    [Fact]
    public  void Create_Valid_Input_Create_Product()
    {
        var factory = new ProductFactory();
        var prodt = ProductFactory.Create("Home 2 bhk", "virar west", 10, 1, null);

        Assert.Equal("Home 2 bhk", prodt.Name);
        Assert.NotEqual(Guid.Empty,prodt.Id);
    }

    [Fact]
    public void Create_Invlaid_Product()
    {
        Assert.Throws<ArgumentException>(() => ProductFactory.Create("", "", 10, 1) );
    }

    [Fact]
    public  void Create_InValid_Price_Product()
    {
        Assert.Throws<ArgumentException>(() => ProductFactory.Create("aff", "vfsafafs", -110, 1) );
    }

    [Fact]
    public void Create_WithCategoryId_AssignsCategory()
    {
        var catid = new Guid();
        var prod = ProductFactory.Create("avc", "avc", 10, 1, catid);
        Assert.Equal(catid,prod.CategoryId);
    }


}
