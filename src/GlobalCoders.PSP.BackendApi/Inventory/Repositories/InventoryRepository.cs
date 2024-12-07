using GlobalCoders.PSP.BackendApi.Base.Extensions;
using GlobalCoders.PSP.BackendApi.Base.Models;
using GlobalCoders.PSP.BackendApi.Data;
using GlobalCoders.PSP.BackendApi.Inventory.Entities;
using GlobalCoders.PSP.BackendApi.OrganizationManagment.Entities;
using GlobalCoders.PSP.BackendApi.OrganizationManagment.ModelsDto;
using Microsoft.EntityFrameworkCore;

namespace GlobalCoders.PSP.BackendApi.Inventory.Repositories;

public class InventoryRepository : IInventoryRepository
{
    private readonly ILogger<InventoryRepository> _logger;
    private readonly IDbContextFactory<BackendContext> _contextFactory;

    public InventoryRepository(ILogger<InventoryRepository> logger, IDbContextFactory<BackendContext> contextFactory)
    {
        _logger = logger;
        _contextFactory = contextFactory;
    }


    public async Task<decimal?> GetQuantityAsync(Guid organizationId, Guid productId)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();

        var productQuantity = await context.InventoryTransactions
            .Where(p => p.ProductId == productId && p.MerchantId == organizationId)
            .SumAsync(x=>x.Quantity);

        return productQuantity; // Assuming 'Quantity' is a property in ProductEntity
    }

    public async Task<bool> CreateTransactionAsync(InventoryTransactionEntity entity,
        CancellationToken cancellationToken)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
        try
        {
            // Add the new transaction to the database
            await context.InventoryTransactions.AddAsync(entity, cancellationToken);
                
            // Save changes
            var result = await context.SaveChangesAsync(cancellationToken);

            return result > 0; // Successfully created
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while creating the transaction");
            return false;
        }
    }
}