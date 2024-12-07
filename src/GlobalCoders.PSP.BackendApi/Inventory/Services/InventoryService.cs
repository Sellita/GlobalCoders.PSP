using GlobalCoders.PSP.BackendApi.Base.Factories;
using GlobalCoders.PSP.BackendApi.Base.Models;
using GlobalCoders.PSP.BackendApi.Inventory.Factories;
using GlobalCoders.PSP.BackendApi.Inventory.Repositories;

namespace GlobalCoders.PSP.BackendApi.Inventory.Services;

public class InventoryService : IInventoryService
{
    private readonly IInventoryRepository _inventoryRepository;

    public InventoryService(ILogger<InventoryService> logger, IInventoryRepository inventoryRepository)
    {
        _inventoryRepository = inventoryRepository;
    }
    public async Task<decimal?> GetQuantityAsync(Guid organizationId, Guid productId)
    {
        return await _inventoryRepository.GetQuantityAsync(organizationId, productId);
    }

    public async Task<(ValidationDetails result, decimal quantity)> ChangeQuantityAsync(Guid organizationId, Guid productId, decimal quantityChange,
        CancellationToken cancellationToken)
    {
        if (quantityChange < 0)
        {
            var currentQuantity = await GetQuantityAsync(organizationId, productId);

            if (currentQuantity == null)
            {
                return (ValidationDetailsFactory.Fail("Failed find item"), 0);
            }
            
            if(currentQuantity < quantityChange)
            {
                return (ValidationDetailsFactory.Fail("Not enough quantity"), currentQuantity.Value);
            }
        }
        
        var entity = InventoryTransactionFactory.Create(organizationId, productId, quantityChange);

        if (await _inventoryRepository.CreateTransactionAsync(entity, cancellationToken))
        {
            return (ValidationDetailsFactory.Ok(), await GetQuantityAsync(organizationId, productId) ?? throw new InvalidOperationException());
        }

        return (ValidationDetailsFactory.Fail("Failed to update quantity"), 0);
    }
}