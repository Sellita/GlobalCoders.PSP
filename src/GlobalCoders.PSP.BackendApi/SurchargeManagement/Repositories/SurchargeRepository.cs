using GlobalCoders.PSP.BackendApi.Base.Extensions;
using GlobalCoders.PSP.BackendApi.Data;
using GlobalCoders.PSP.BackendApi.OrganizationManagment.ModelsDto;
using GlobalCoders.PSP.BackendApi.SurchargeManagement.Entities;
using GlobalCoders.PSP.BackendApi.SurchargeManagement.ModelsDto;
using Microsoft.EntityFrameworkCore;

namespace GlobalCoders.PSP.BackendApi.SurchargeManagement.Repositories;

public class SurchargeRepository : ISurchargeRepository
{
    private readonly ILogger<SurchargeRepository> _logger;
    private readonly IDbContextFactory<BackendContext> _contextFactory;

    public SurchargeRepository(ILogger<SurchargeRepository> logger, IDbContextFactory<BackendContext> contextFactory)
    {
        _logger = logger;
        _contextFactory = contextFactory;
    }

    public async Task<bool> UpdateAsync(SurchargeEntity updateModel)
    {
        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            context.Surcharge.Update(updateModel);
            return await context.SaveChangesAsync() > 0;
        }
        catch (Exception e)
        {
            _logger.LogExceptionError(e, nameof(UpdateAsync));
        }

        return false;
    }

    public async Task<bool> CreateAsync(SurchargeEntity createModel)
    {
        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            context.Surcharge.Add(createModel);
            return await context.SaveChangesAsync() > 0;
        }
        catch (Exception e)
        {
            _logger.LogExceptionError(e, nameof(CreateAsync));
        }

        return false;
    }

    public async Task<(List<SurchargeEntity>, int)> GetAllAsync(SurchargeFilter filter)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var query = context.Surcharge.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(filter.DisplayName))
        {
            query = query.Where(x => x.Name.Contains(filter.DisplayName));
        }
        
        var totalItems = await query.CountAsync();

        var items = await query
            .OrderBy(x=>x.Name)
            .Skip((filter.Page - 1) * filter.ItemsPerPage)
            .Take(filter.ItemsPerPage)
            .ToListAsync();

        return (items, totalItems);
    }

    public async Task<SurchargeEntity?> GetAsync(Guid organizationId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        return await context.Surcharge.FirstOrDefaultAsync(x => x.Id == organizationId);
    }

    public async Task<bool> DeleteAsync(Guid organizationId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var entity = await context.Surcharge.FirstOrDefaultAsync(x => x.Id == organizationId);

        if (entity == null)
        {
            return false;
        }

        entity.IsDeleted = true;

        return await context.SaveChangesAsync() > 0;
    }
}