using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProductManagement.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name)
            .HasColumnName("name")
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(p => p.Description)
            .HasColumnName("description")
            .HasMaxLength(500);

        builder.Property(p => p.StockQuantity)
            .HasColumnName("stock_quantity")
            .IsRequired();

        builder.Property(p => p.IsDeleted)
            .HasColumnName("is_deleted")
            .HasDefaultValue(false);

        builder.Property(p => p.CreatedAt)
            .HasColumnName("created_at");

        // 🆕 OwnsOne: flatten the Money value object into plain columns on the products table
        builder.OwnsOne(p => p.Price, money =>
  {
      money.Property(m => m.Amount)
          .HasColumnName("price")
          .HasPrecision(10, 2)
          .IsRequired();

      money.Property(m => m.Currency)
          .HasColumnName("currency")
          .HasMaxLength(3);
  });

        // 🆕 Global soft-delete filter — every query against Product automatically excludes deleted rows
        builder.HasQueryFilter(p => !p.IsDeleted);

        // relationship + index, matching your Day 22 schema
        builder.HasOne<Category>()
            .WithMany()
            .HasForeignKey(p => p.CategoryId);

        builder.HasIndex(p => p.CategoryId);
        builder.HasIndex(p => p.Name);
    }
}