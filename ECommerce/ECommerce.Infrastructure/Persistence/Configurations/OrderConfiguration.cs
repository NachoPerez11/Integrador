using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ECommerce.Domain.Entities;

namespace ECommerce.Infrastructure.Persistence.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable("Orders");
        builder.HasKey(o => o.Id);
                builder.Property(o => o.Id)
               .ValueGeneratedNever();
        builder.Property(o => o.UserId)
               .IsRequired();
        builder.Property(o => o.TotalAmount)
               .IsRequired()
               .HasColumnType("decimal(18,2)");
        builder.Property(o => o.Status)
               .IsRequired()
               .HasMaxLength(50);
    }
}