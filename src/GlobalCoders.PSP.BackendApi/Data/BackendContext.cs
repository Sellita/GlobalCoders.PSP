using GlobalCoders.PSP.BackendApi.Data.Configurations;
using GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities;
using GlobalCoders.PSP.BackendApi.InventoryManagement.Entities;
using GlobalCoders.PSP.BackendApi.OrdersManagement.Entities;
using GlobalCoders.PSP.BackendApi.OrganizationManagment.Entities;
using GlobalCoders.PSP.BackendApi.ProductsManagment.Entities;
using GlobalCoders.PSP.BackendApi.SurchargeManagement.Entities;
using GlobalCoders.PSP.BackendApi.TaxManagement.Entities;
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

        builder.ApplyConfiguration(new EmployeeEntityConfiguration());
        builder.ApplyConfiguration(new SurchargeEntityConfiguration());
        builder.ApplyConfiguration(new ProductTypeEntityConfiguration());
        builder.ApplyConfiguration(new ProductEntityConfiguration());
        builder.ApplyConfiguration(new TaxEntityConfiguration());
        builder.ApplyConfiguration(new OrderProductsEntityConfiguration());
        builder.ApplyConfiguration(new OrderEntityConfiguration());
    }

    //Empoloee Managment

    #region Employee Managment

    public DbSet<MerchantEntity> Merchant => Set<MerchantEntity>();
    public DbSet<PermisionEntity> Permission => Set<PermisionEntity>();

    #endregion
    
    //Surcharge Management
    
    #region Surcharge Management
    
    public DbSet<SurchargeEntity> Surcharge => Set<SurchargeEntity>();
    
    #endregion
    
    public DbSet<ProductTypeEntity> ProductType => Set<ProductTypeEntity>();
    public DbSet<ProductEntity> Product => Set<ProductEntity>();
    
    
    public DbSet<InventoryTransactionEntity> InventoryTransactions => Set<InventoryTransactionEntity>();
    
    public DbSet<OrderEntity> Orders => Set<OrderEntity>();
    public DbSet<OrderProductEntity> OrderProducts => Set<OrderProductEntity>();
    public DbSet<OrderPaymentsEntity> OrderPayments => Set<OrderPaymentsEntity>();
    
    // Tax Management
    #region Tax Management
    
    public DbSet<TaxEntity> Tax => Set<TaxEntity>();
    
    #endregion
    
}