using GlobalCoders.PSP.BackendApi.Base.Extensions;
using GlobalCoders.PSP.BackendApi.Data;
using GlobalCoders.PSP.BackendApi.OrdersManagement.Entities;
using GlobalCoders.PSP.BackendApi.OrdersManagement.ModelsDto;
using Microsoft.EntityFrameworkCore;

namespace GlobalCoders.PSP.BackendApi.OrdersManagement.Repositories;

public class OrdersRepository : IOrdersRepository
{
    private readonly ILogger<OrdersRepository> _logger;
    private readonly IDbContextFactory<BackendContext> _contextFactory;

    public OrdersRepository(ILogger<OrdersRepository> logger, IDbContextFactory<BackendContext> contextFactory)
    {
        _logger = logger;
        _contextFactory = contextFactory;
    }

    public async Task<bool> UpdateAsync(OrderEntity updateModel)
    {
        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            context.Orders.Update(updateModel);
            return await context.SaveChangesAsync() > 0;
        }
        catch (Exception e)
        {
            _logger.LogExceptionError(e, nameof(UpdateAsync));
        }

        return false;
    }

    public async Task<bool> CreateAsync(OrderEntity createModel)
    {
        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync();
            context.Orders.Add(createModel);
            return await context.SaveChangesAsync() > 0;
        }
        catch (Exception e)
        {
            _logger.LogExceptionError(e, nameof(CreateAsync));
        }

        return false;
    }

    public async Task<(List<OrderEntity>, int)> GetAllAsync(OrdersFilter filter)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var query = context.Orders
            .Include(x=>x.Merchant)
            .Include(x=>x.OrderProducts)
            .Include(x=>x.OrderPayments).AsQueryable();
        
        if (!string.IsNullOrWhiteSpace(filter.Client))
        {
            query = query.Where(x => x.ClientName.Contains(filter.Client));
        }
        
        if (filter.MerchantId.HasValue)
        {
            query = query.Where(x => x.MerchantId == filter.MerchantId);
        }
        
        if (filter.OrderStatus.HasValue)
        {
            query = query.Where(x => x.Status == filter.OrderStatus);
        }
        
        var totalItems = await query.CountAsync();

        var items = await query
            .OrderByDescending(x=>x.CreatedAt)
            .Skip((filter.Page - 1) * filter.ItemsPerPage)
            .Take(filter.ItemsPerPage)
            .ToListAsync();

        return (items, totalItems);
    }

    public async Task<OrderEntity?> GetAsync(Guid organizationId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        return await context.Orders
            .Include(x=>x.Employee)
            .Include(x=>x.Merchant)
            .Include(x=>x.OrderProducts)
            .ThenInclude(x=>x.Product)
            .Include(x=>x.OrderProducts)
            .ThenInclude(x=>x.OrderProductTaxes)
            .Include(x=>x.OrderPayments)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Id == organizationId);
    }

    public async Task<bool> DeleteAsync(Guid organizationId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var entity = await context.Orders
            .Include(x=>x.Merchant)
            .Include(x=>x.OrderProducts)
            .Include(x=>x.OrderPayments)
            .FirstOrDefaultAsync(x => x.Id == organizationId);

        if (entity == null)
        {
            return false;
        }

        entity.IsDeleted = true;

        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> OrderBelongsToMerchantAsync(Guid orderIdOrderId, Guid? userMerchantId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        return await context.Orders.AnyAsync(x => x.Id == orderIdOrderId && x.MerchantId == userMerchantId);
    }

    public async Task<(bool result, string message)> ChangeStatusAsync(OrderChangeStatusRequestModel orderChangeStatusRequest)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var order = await context.Orders
            .Include(x=>x.Merchant)
            .Include(x=>x.OrderProducts)
            .Include(x=>x.OrderPayments)
            .FirstOrDefaultAsync(x => x.Id == orderChangeStatusRequest.OrderId);
        
        if(order == null)
        {
            return (false, "Order not found");
        }
        
        order.Status = orderChangeStatusRequest.NewStatus;
        
        return (await context.SaveChangesAsync() > 0, "Failed to change status");
    }

    public async Task<bool> DeleteProductFromLustAsync(OrderProductEntity productFromList)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        
        context.OrderProducts.Remove(productFromList);
        
        return await context.SaveChangesAsync() > 0;
    }
}