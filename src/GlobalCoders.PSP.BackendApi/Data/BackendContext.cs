using GlobalCoders.PSP.BackendApi.Data.Configurations;
using GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities;
using GlobalCoders.PSP.BackendApi.OrganizationManagment.Entities;
using Microsoft.EntityFrameworkCore;

namespace GlobalCoders.PSP.BackendApi.Data;

public sealed class BackendContext : BaseDbContext
{
    public BackendContext(DbContextOptions<BackendContext> options)
        : base(options)
    {
    }

    [ActivatorUtilitiesConstructor]
    public BackendContext()
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.ApplyConfiguration(new MerhantEntityConfiguration());
    }

    //Empoloee Managment

    #region Employee Managment

    public DbSet<MerchantEntity> Merchant => Set<MerchantEntity>();
    public DbSet<PermisionEntity> Permission => Set<PermisionEntity>();

    #endregion
}