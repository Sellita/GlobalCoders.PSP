using GlobalCoders.PSP.BackendApi.ProductsManagement.Entities;
using GlobalCoders.PSP.BackendApi.ServicesManagement.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GlobalCoders.PSP.BackendApi.Data.Configurations;

public class ServiceEntityConfiguration : IEntityTypeConfiguration<ServiceEntity>
{
    public void Configure(EntityTypeBuilder<ServiceEntity> builder)
    {
        builder.HasQueryFilter(u => !u.IsDeleted);
    }
}