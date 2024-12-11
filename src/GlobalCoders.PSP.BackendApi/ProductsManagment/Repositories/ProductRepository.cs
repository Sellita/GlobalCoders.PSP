using GlobalCoders.PSP.BackendApi.Base.Extensions;
using GlobalCoders.PSP.BackendApi.Data;
using GlobalCoders.PSP.BackendApi.ProductsManagment.Entities;
using GlobalCoders.PSP.BackendApi.ProductsManagment.ModelsDto;
using Microsoft.EntityFrameworkCore;

namespace GlobalCoders.PSP.BackendApi.ProductsManagment.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ILogger<ProductRepository> _logger;
    private readonly IDbContextFactory<BackendContext> _contextFactory;

    public ProductRepository(ILogger<ProductRepository> logger, IDbContextFactory<BackendContext> contextFactory)
    {
        _logger = logger;
        _contextFactory = contextFactory;
    }

    public async Task<bool> UpdateAsync(ProductEntity updateModel)
    {
        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            context.Product.Update(updateModel);
            return await context.SaveChangesAsync() > 0;
        }
        catch (Exception e)
        {
            _logger.LogExceptionError(e, nameof(UpdateAsync));
        }

        return false;
    }

    public async Task<bool> CreateAsync(ProductEntity createModel)
    {
        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            context.Product.Add(createModel);
            return await context.SaveChangesAsync() > 0;
        }
        catch (Exception e)
        {
            _logger.LogExceptionError(e, nameof(CreateAsync));
        }

        return false;
    }

    public async Task<(List<ProductEntity>, int)> GetAllAsync(ProductFilter filter)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var query = context.Product.AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            query = query.Where(x => x.DisplayName.Contains(filter.Name));
        }

        if (filter.MerchantId != null)
        {
            query = query.Where(x => x.MerchantId == filter.MerchantId);
        }
        
        if(filter.CategoryId != null)
        {
            query = query.Where(x => x.ProductTypeId == filter.CategoryId);
        }
        
        var totalItems = await query.CountAsync();

        var items = await query
            .OrderBy(x=>x.DisplayName)
            .Skip((filter.Page - 1) * filter.ItemsPerPage)
            .Take(filter.ItemsPerPage)
            .ToListAsync();

        return (items, totalItems);
    }

    public async Task<ProductEntity?> GetAsync(Guid organizationId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        return await context.Product.FirstOrDefaultAsync(x => x.Id == organizationId);
    }

    public async Task<bool> DeleteAsync(Guid organizationId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var entity = await context.Product.FirstOrDefaultAsync(x => x.Id == organizationId);

        if (entity == null)
        {
            return false;
        }

        entity.IsDeleted = true;

        return await context.SaveChangesAsync() > 0;
    }
}