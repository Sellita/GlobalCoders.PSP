using GlobalCoders.PSP.BackendApi.Base.Extensions;
using GlobalCoders.PSP.BackendApi.Data;
using GlobalCoders.PSP.BackendApi.TaxManagement.Entities;
using GlobalCoders.PSP.BackendApi.TaxManagement.ModelsDto;
using Microsoft.EntityFrameworkCore;

namespace GlobalCoders.PSP.BackendApi.TaxManagement.Repositories;

public class TaxRepository : ITaxRepository
{
    private readonly ILogger<TaxRepository> _logger;
    private readonly IDbContextFactory<BackendContext> _contextFactory;

    public TaxRepository(ILogger<TaxRepository> logger, IDbContextFactory<BackendContext> contextFactory)
    {
        _logger = logger;
        _contextFactory = contextFactory;
    }
        
    public async Task<bool> UpdateAsync(TaxEntity updateModel)
    {
        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            context.Tax.Update(updateModel);
            return await context.SaveChangesAsync() > 0;
        }
        catch (Exception e)
        {
            _logger.LogExceptionError(e, nameof(UpdateAsync));
        }

        return false;
    }

    public async Task<bool> CreateAsync(TaxEntity createModel)
    {
        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            context.Tax.Add(createModel);
            return await context.SaveChangesAsync() > 0;
        }
        catch (Exception e)
        {
            _logger.LogExceptionError(e, nameof(CreateAsync));
        }

        return false;
    }

    public async Task<(List<TaxEntity> items, int totalItems)> GetAllAsync(TaxFilter filter)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var query = context.Tax.AsQueryable();
        
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

        return (items, totalItems);    }

    public async Task<TaxEntity?> GetAsync(Guid taxId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        return await context.Tax.FirstOrDefaultAsync(x => x.Id == taxId);    }

    public async Task<bool> DeleteAsync(Guid taxId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var entity = await context.Tax.FirstOrDefaultAsync(x => x.Id == taxId);

        if (entity == null)
        {
            return false;
        }

        entity.IsDeleted = true;

        return await context.SaveChangesAsync() > 0;    }
}