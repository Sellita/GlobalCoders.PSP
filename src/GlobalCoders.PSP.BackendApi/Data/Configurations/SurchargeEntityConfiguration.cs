using GlobalCoders.PSP.BackendApi.SurchargeManagement.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GlobalCoders.PSP.BackendApi.Data.Configurations;

public class SurchargeEntityConfiguration : IEntityTypeConfiguration<SurchargeEntity>
{
    public void Configure(EntityTypeBuilder<SurchargeEntity> builder)
    {
        builder.HasQueryFilter(u => !u.IsDeleted);
    }
}