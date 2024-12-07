using GlobalCoders.PSP.BackendApi.Base.Models;
using GlobalCoders.PSP.BackendApi.Inventory.Entities;
using GlobalCoders.PSP.BackendApi.OrganizationManagment.Entities;
using GlobalCoders.PSP.BackendApi.OrganizationManagment.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.Inventory.Repositories;

public interface IInventoryRepository
{
    Task<decimal?> GetQuantityAsync(Guid organizationId, Guid productId);
    Task<bool> CreateTransactionAsync(InventoryTransactionEntity entity, CancellationToken cancellationToken);
}