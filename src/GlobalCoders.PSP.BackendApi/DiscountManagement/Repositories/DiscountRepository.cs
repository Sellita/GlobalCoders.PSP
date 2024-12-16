using GlobalCoders.PSP.BackendApi.Base.Extensions;
using GlobalCoders.PSP.BackendApi.Data;
using GlobalCoders.PSP.BackendApi.DiscountManagement.Entities;
using GlobalCoders.PSP.BackendApi.DiscountManagement.ModelsDto;
using Microsoft.EntityFrameworkCore;

namespace GlobalCoders.PSP.BackendApi.DiscountManagement.Repositories;

public class DiscountRepository : IDiscountRepository
{
    private readonly ILogger<DiscountRepository> _logger;
    private readonly IDbContextFactory<BackendContext> _contextFactory;

    public DiscountRepository(ILogger<DiscountRepository> logger, IDbContextFactory<BackendContext> contextFactory)
    {
        _logger = logger;
        _contextFactory = contextFactory;
    }

    public async Task<bool> UpdateAsync(DiscountEntity updateModel)
    {
        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            context.Discount.Update(updateModel);
            return await context.SaveChangesAsync() > 0;
        }
        catch (Exception e)
        {
            _logger.LogExceptionError(e, nameof(UpdateAsync));
        }

        return false;
    }

    public async Task<bool> CreateAsync(DiscountEntity createModel)
    {
        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            context.Discount.Add(createModel);
            return await context.SaveChangesAsync() > 0;
        }
        catch (Exception e)
        {
            _logger.LogExceptionError(e, nameof(CreateAsync));
        }

        return false;
    }

    public async Task<(List<DiscountEntity> items, int totalItems)> GetAllAsync(DiscountFilter filter)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var query = context.Discount
            .Include(x=>x.ProductType)
            .Include(x=>x.Product)
            .Include(x=>x.Merchant)
            .AsSplitQuery();

        if (!string.IsNullOrWhiteSpace(filter.DisplayName))
        {
            query = query.Where(x => x.Name.Contains(filter.DisplayName));
        }

        if (filter.MerchantId != null)
        {
            query = query.Where(x => x.MerchantId == filter.MerchantId);
        }
        
        if(filter.Date!= null)
        {
            query = query.Where(x =>
                x.StartDate <= filter.Date && x.EndDate >= filter.Date || (x.StartDate == null && x.EndDate == null));
        }

        var totalItems = await query.CountAsync();

        var items = await query
            .OrderBy(x => x.Name)
            .Skip((filter.Page - 1) * filter.ItemsPerPage)
            .Take(filter.ItemsPerPage)
            .ToListAsync();

        return (items, totalItems);
    }

    public async Task<DiscountEntity?> GetAsync(Guid discountId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        return await context.Discount
            .Include(x=>x.ProductType)
            .Include(x=>x.Product)
            .Include(x=>x.Merchant)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Id == discountId);
    }

    public async Task<bool> DeleteAsync(Guid discountId, Guid? merchantId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var query = context.Discount
            .Where(x => x.Id == discountId);

        if (merchantId != null)
        {
            query = query.Where(x => x.MerchantId == merchantId);
        }

        var entity = await query.FirstOrDefaultAsync();

        if (entity == null)
        {
            return false;
        }

        entity.IsDeleted = true;

        return await context.SaveChangesAsync() > 0;
    }
}