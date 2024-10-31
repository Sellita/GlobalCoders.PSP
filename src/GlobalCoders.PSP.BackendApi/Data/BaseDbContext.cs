using GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace GlobalCoders.PSP.BackendApi.Data;

public abstract class BaseDbContext : IdentityDbContext<EmployeeEntity, PermisionTemplateEntity, Guid, IdentityUserClaim<Guid>, PermisionEntity,
    IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
{
    private readonly string _connectionString;

    protected BaseDbContext(DbContextOptions options) : base(options)
    {
        _connectionString = options.FindExtension<RelationalOptionsExtension>()?.ConnectionString ?? string.Empty;
    }

    [ActivatorUtilitiesConstructor]
    protected BaseDbContext()
    {
        _connectionString = nameof(_connectionString);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured && !string.IsNullOrWhiteSpace(_connectionString))
        {
            optionsBuilder.UseNpgsql(_connectionString);
        }
    }
}
