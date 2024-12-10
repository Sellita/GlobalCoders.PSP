using GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities;
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

    //Employee Managment

    #region Employee Managment

    public DbSet<MerchantEntity> Merchant => Set<MerchantEntity>();
    public DbSet<PermisionEntity> Permission => Set<PermisionEntity>();

    #endregion
}