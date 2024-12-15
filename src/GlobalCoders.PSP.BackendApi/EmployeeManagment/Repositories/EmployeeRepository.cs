using GlobalCoders.PSP.BackendApi.Data;
using GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities;
using Microsoft.EntityFrameworkCore;

namespace GlobalCoders.PSP.BackendApi.EmployeeManagment.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly ILogger<EmployeeRepository> _logger;
    private readonly IDbContextFactory<BackendContext> _contextFactory;

    public EmployeeRepository(ILogger<EmployeeRepository> logger, IDbContextFactory<BackendContext> contextFactory)
    {
        _logger = logger;
        _contextFactory = contextFactory;
    }
    public async Task<bool> UpdateAsync(EmployeeEntity appUser, string updateRequestRole, CancellationToken cancellationToken)
    {
        try
        {
            var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            context.Users.Update(appUser);

            var result = await context.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to update user by Id {Id}", appUser.Id);

            return false;
        }
    }

    public async Task<bool> DeleteAsync(Guid employeeId)
    {
        try
        {
            var context = await _contextFactory.CreateDbContextAsync();

            var user = await context.Users
                .Include(x => x.UserPermissions)
                .Where(x => x.Id == employeeId)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                _logger.LogWarning("User not found by Id {Id}", employeeId);
                return false;
            }
            
            user.IsActive = false;
            
            context.UserRoles.RemoveRange(user.UserPermissions);
            
            user.IsDeleted = true;
            var result = await context.SaveChangesAsync();

            return result > 0;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while deleting user by Id {Id}", employeeId);

            return false;
        }
    }

    public Task<EmployeeEntity?> GetUserAsync(Guid user)
    {
        var context = _contextFactory.CreateDbContext();
        
        return context.Users
            .Include(x => x.UserPermissions)
            .Include(x=>x.Merchant)
            .Where(x => x.Id == user)
            .FirstOrDefaultAsync();
    }
}