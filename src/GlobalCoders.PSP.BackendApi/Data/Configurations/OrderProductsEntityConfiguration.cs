using GlobalCoders.PSP.BackendApi.OrdersManagement.Entities;
using GlobalCoders.PSP.BackendApi.ProductsManagment.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GlobalCoders.PSP.BackendApi.Data.Configurations;

public class OrderEntityConfiguration : IEntityTypeConfiguration<OrderProductEntity>
{
    public void Configure(EntityTypeBuilder<OrderProductEntity> builder)
    {
        builder
            .HasMany(x => x.OrderProductTaxes)
            .WithOne()
            .HasForeignKey(x => x.OrderProductId)
            .IsRequired(false); 
    }
}