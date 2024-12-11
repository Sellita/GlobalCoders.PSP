using GlobalCoders.PSP.BackendApi.OrganizationManagment.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GlobalCoders.PSP.BackendApi.Data.Configurations;

public class MerhantEntityConfiguration : IEntityTypeConfiguration<MerchantEntity>
{
    public void Configure(EntityTypeBuilder<MerchantEntity> builder)
    {
        builder.HasQueryFilter(u => !u.IsDeleted);
    }
}