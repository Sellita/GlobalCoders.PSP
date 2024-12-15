using GlobalCoders.PSP.BackendApi.InventoryManagement.Entities;

namespace GlobalCoders.PSP.BackendApi.InventoryManagement.Repositories;

public interface IInventoryRepository
{
    Task<decimal?> GetQuantityAsync(Guid organizationId, Guid productId);
    Task<bool> CreateTransactionAsync(InventoryTransactionEntity entity, CancellationToken cancellationToken);
}