using GlobalCoders.PSP.BackendApi.Base.Extensions;
using GlobalCoders.PSP.BackendApi.Data;
using GlobalCoders.PSP.BackendApi.ServicesManagement.Entities;
using GlobalCoders.PSP.BackendApi.ServicesManagement.ModelsDto;
using Microsoft.EntityFrameworkCore;

namespace GlobalCoders.PSP.BackendApi.ServicesManagement.Repositories;

public class ServicesRepository : IServicesRepository
{
    private readonly ILogger<ServicesRepository> _logger;
    private readonly IDbContextFactory<BackendContext> _contextFactory;

    public ServicesRepository(ILogger<ServicesRepository> logger, IDbContextFactory<BackendContext> contextFactory)
    {
        _logger = logger;
        _contextFactory = contextFactory;
    }

    public async Task<bool> UpdateAsync(ServiceEntity updateModel)
    {
        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            context.Services.Update(updateModel);
            return await context.SaveChangesAsync() > 0;
        }
        catch (Exception e)
        {
            _logger.LogExceptionError(e, nameof(UpdateAsync));
        }

        return false;
    }

    public async Task<bool> CreateAsync(ServiceEntity createModel)
    {
        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            context.Services.Add(createModel);
            return await context.SaveChangesAsync() > 0;
        }
        catch (Exception e)
        {
            _logger.LogExceptionError(e, nameof(CreateAsync));
        }

        return false;
    }

    public async Task<(List<ServiceEntity>, int)> GetAllAsync(ServiceFilter filter)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var query = context.Services
            .Include(x=>x.Employee)
            .AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            query = query.Where(x => x.DisplayName.Contains(filter.Name));
        }

        if (filter.MerchantId != null)
        {
            query = query.Where(x => x.EmployeeId == filter.MerchantId);
        }
        
        var totalItems = await query.CountAsync();

        var items =  query
            .OrderBy(x=>x.DisplayName)
            .Skip((filter.Page - 1) * filter.ItemsPerPage)
            .Take(filter.ItemsPerPage)
            .ToList();

        return (items, totalItems);
    }

    public async Task<ServiceEntity?> GetAsync(Guid serviceId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var service =  await context.Services
            .Include(x=>x.Employee)
            .FirstOrDefaultAsync(x => x.Id == serviceId);

        return service;
    }

    public async Task<bool> DeleteAsync(Guid organizationId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var entity = await context.Services.FirstOrDefaultAsync(x => x.Id == organizationId);

        if (entity == null)
        {
            return false;
        }

        entity.IsDeleted = true;

        return await context.SaveChangesAsync() > 0;
    }
}