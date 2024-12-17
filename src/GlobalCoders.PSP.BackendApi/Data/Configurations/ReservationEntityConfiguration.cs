using GlobalCoders.PSP.BackendApi.ProductsManagement.Entities;
using GlobalCoders.PSP.BackendApi.ReservationManagment.Entities;
using GlobalCoders.PSP.BackendApi.ServicesManagement.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GlobalCoders.PSP.BackendApi.Data.Configurations;

public class ReservationEntityConfiguration : IEntityTypeConfiguration<ReservationEntity>
{
    public void Configure(EntityTypeBuilder<ReservationEntity> builder)
    {
        builder.HasQueryFilter(u => !u.IsDeleted);
    }
}