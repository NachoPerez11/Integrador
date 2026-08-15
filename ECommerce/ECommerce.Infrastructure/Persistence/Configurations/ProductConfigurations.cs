using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ECommerce.Domain.Entities;

namespace ECommerce.Infrastructure.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        // Nombre de la tabla en la BD
        builder.ToTable("Products");

        // Clave primaria
        builder.HasKey(p => p.Id);
        
       
       builder.Property(p => p.Id)
              .ValueGeneratedNever();

       builder.Property(p => p.Name)
              .IsRequired()
              .HasMaxLength(150);

       builder.Property(p => p.Description)
              .HasMaxLength(500);

       builder.Property(p => p.Price)
              .IsRequired()
              .HasColumnType("decimal(18,2)");

       builder.Property(p => p.Stock)
              .IsRequired();

       builder.Property(p => p.IsActive)
              .IsRequired();
    }
}