using GlobalCoders.PSP.BackendApi.Base.Extensions;
using GlobalCoders.PSP.BackendApi.Data;
using GlobalCoders.PSP.BackendApi.ProductsManagment.Entities;
using GlobalCoders.PSP.BackendApi.ProductsManagment.ModelsDto;
using Microsoft.EntityFrameworkCore;

namespace GlobalCoders.PSP.BackendApi.ProductsManagment.Repositories;

public class ProductTypeRepository : IProductTypeRepository
{
    private readonly ILogger<ProductTypeRepository> _logger;
    private readonly IDbContextFactory<BackendContext> _contextFactory;

    public ProductTypeRepository(ILogger<ProductTypeRepository> logger, IDbContextFactory<BackendContext> contextFactory)
    {
        _logger = logger;
        _contextFactory = contextFactory;
    }

    public async Task<bool> UpdateAsync(ProductTypeEntity updateModel)
    {
        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            context.ProductType.Update(updateModel);
            return await context.SaveChangesAsync() > 0;
        }
        catch (Exception e)
        {
            _logger.LogExceptionError(e, nameof(UpdateAsync));
        }

        return false;
    }

    public async Task<bool> CreateAsync(ProductTypeEntity createModel)
    {
        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            context.ProductType.Add(createModel);
            return await context.SaveChangesAsync() > 0;
        }
        catch (Exception e)
        {
            _logger.LogExceptionError(e, nameof(CreateAsync));
        }

        return false;
    }

    public async Task<(List<ProductTypeEntity>, int)> GetAllAsync(ProductTypeFilter filter)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var query = context.ProductType.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(filter.DisplayName))
        {
            query = query.Where(x => x.DisplayName.Contains(filter.DisplayName));
        }
        
        var totalItems = await query.CountAsync();

        var items = await query
            .OrderBy(x=>x.DisplayName)
            .Skip((filter.Page - 1) * filter.ItemsPerPage)
            .Take(filter.ItemsPerPage)
            .ToListAsync();

        return (items, totalItems);
    }

    public async Task<ProductTypeEntity?> GetAsync(Guid organizationId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        return await context.ProductType.FirstOrDefaultAsync(x => x.Id == organizationId);
    }

    public async Task<bool> DeleteAsync(Guid organizationId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var entity = await context.ProductType.FirstOrDefaultAsync(x => x.Id == organizationId);

        if (entity == null)
        {
            return false;
        }

        entity.IsDeleted = true;

        return await context.SaveChangesAsync() > 0;
    }
}