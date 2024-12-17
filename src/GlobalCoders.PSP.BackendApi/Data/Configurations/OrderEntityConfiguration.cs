using GlobalCoders.PSP.BackendApi.OrdersManagement.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GlobalCoders.PSP.BackendApi.Data.Configurations;

public class OrderProductsEntityConfiguration : IEntityTypeConfiguration<OrderEntity>
{
    public void Configure(EntityTypeBuilder<OrderEntity> builder)
    {
        builder
            .HasMany(x => x.OrderProducts)
            .WithOne()
            .HasForeignKey(x => x.OrderId)
            .IsRequired(false); 
        
        builder
            .HasMany(x => x.OrderPayments)
            .WithOne()
            .HasForeignKey(x => x.OrderId)
            .IsRequired(false);
        
        builder.HasQueryFilter(u => !u.IsDeleted);
    }
}