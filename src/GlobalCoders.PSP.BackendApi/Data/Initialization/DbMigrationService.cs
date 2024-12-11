using GlobalCoders.PSP.BackendApi.Base.Services;
using Microsoft.EntityFrameworkCore;

namespace GlobalCoders.PSP.BackendApi.Data.Initialization;

public sealed class DbMigrationService : IInitializeRequired
{
    private readonly ILogger<DbMigrationService> _logger;
    private readonly IDbContextFactory<BackendContext> _dbContextFactory;
    public int Priority => -1;

    public DbMigrationService(
        ILogger<DbMigrationService> logger,
        IDbContextFactory<BackendContext> dbContextFactory)
    {
        _logger = logger;
        _dbContextFactory = dbContextFactory;
    }

    public async Task<bool> MigrateAsync(CancellationToken cancellationToken)
    {
        try
        {
            var context = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
            await context.Database.MigrateAsync(cancellationToken);
            
            _logger.LogInformation("Database migrated successfully");

            return true;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while migrating database");
        }

        return false;
    }
    

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        var result = await MigrateAsync(cancellationToken);
        
        if (!result)
        {
            _logger.LogError("Error while migrating database");
            Environment.Exit(1);
        }
    }
}
