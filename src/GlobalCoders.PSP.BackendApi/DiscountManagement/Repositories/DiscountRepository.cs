using GlobalCoders.PSP.BackendApi.DiscountManagment.Entities;
using GlobalCoders.PSP.BackendApi.Data;
using GlobalCoders.PSP.BackendApi.DiscountManagement.ModelsDto;
using Microsoft.EntityFrameworkCore;

namespace GlobalCoders.PSP.BackendApi.DiscountManagment.Repositories;

public class DiscountRepository : IDiscountRepository
{
    private readonly IDbContextFactory<BaseDbContext> _contextFactory;

    public DiscountRepository(IDbContextFactory<BaseDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<DiscountEntity> GetByIdAsync(Guid id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Discounts.FindAsync(id);
    }

    public async Task<List<DiscountEntity>> GetAllAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Discounts.AsNoTracking().ToListAsync();
    }

    public async Task<(List<DiscountEntity> items, int totalItems)> GetFilteredAsync(DiscountFilter filter)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var query = context.Discounts.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Code))
        {
            query = query.Where(d => d.Code.Contains(filter.Code));
        }

        if (!string.IsNullOrWhiteSpace(filter.Status))
        {
            query = query.Where(d => d.Status == filter.Status);
        }

        if (filter.Percentage.HasValue)
        {
            query = query.Where(d => d.Percentage == (double)filter.Percentage.Value);
        }

        var totalItems = await query.CountAsync();
        var items = await query
            .Skip((filter.Page - 1) * filter.ItemsPerPage)
            .Take(filter.ItemsPerPage)
            .ToListAsync();

        return (items, totalItems);
    }

    public async Task CreateAsync(DiscountEntity discount)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        await context.Discounts.AddAsync(discount);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(DiscountEntity discount)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        context.Discounts.Update(discount);
        await context.SaveChangesAsync();
    }
    
    public async Task<DiscountEntity?> GetAsync(Guid id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Discounts.FindAsync(id);
    }


    public async Task DeleteAsync(Guid id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var discount = await context.Discounts.FindAsync(id);
        if (discount != null)
        {
            context.Discounts.Remove(discount);
            await context.SaveChangesAsync();
        }
    }
}
