using GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GlobalCoders.PSP.BackendApi.Data.Configurations;

public sealed class AppUserRoleConfigurations : IEntityTypeConfiguration<PermisionEntity>
{
    public void Configure(EntityTypeBuilder<PermisionEntity> builder)
    {
        builder
            .HasOne(x => x.Employee)
            .WithMany(x => x.UserPermissions)
            .HasForeignKey(nameof(IdentityUserRole<Guid>.UserId))
            .IsRequired(false);

        builder
            .HasOne(x => x.AppRole)
            .WithMany(x => x.UserRoles)
            .HasForeignKey(nameof(IdentityUserRole<Guid>.RoleId))
            .IsRequired(false);
    }
}
