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
            var oldSchedule = context.EmployeeScheduleEntity.Where(x=>x.EmployeeEntityId == appUser.Id).ToList();
            context.RemoveRange(oldSchedule);
            
            var roles = context.Roles
                .AsNoTracking()
                .Where(x => updateRequestRole.Contains(x.Name))
                .ToList();

            var appUserEntity = await context.Users
                .Where(x => x.Id == appUser.Id)
                .Include(x => x.UserPermissions)
                .ThenInclude(x => x.AppRole)
                .FirstOrDefaultAsync(cancellationToken);
            
            var remove = appUserEntity.UserPermissions
                .Where(x => x.AppRole != null && !roles.Select(r => r.Name).Contains(x.AppRole.Name))
                .ToList();

            context.UserRoles.RemoveRange(remove);

            var userRoles = appUserEntity.UserPermissions
                .Where(x => x.AppRole != null)
                .Select(x => x.AppRole)
                .ToList();

            var add = roles
                .Where(x => !userRoles.Select(r => r.Id).Contains(x.Id))
                .Select(
                    x => new PermisionEntity()
                    {
                        UserId = appUserEntity.Id,
                        RoleId = x.Id
                    })
                .ToList();

            context.UserRoles.AddRange(add);
            
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
            .Include(x=>x.WorkingSchedule)
            .Where(x => x.Id == user)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> UpdateScheduleAsync(EmployeeEntity user, CancellationToken cancellationToken)
    {
        try
        {
            var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            
            context.Users.Update(user);
            
            var oldSchedule = context.EmployeeScheduleEntity.Where(x=>x.EmployeeEntityId == user.Id).ToList();
            context.RemoveRange(oldSchedule);
            
            var result = await context.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to update user by Id {Id}", user.Id);

            return false;
        }
    }
}