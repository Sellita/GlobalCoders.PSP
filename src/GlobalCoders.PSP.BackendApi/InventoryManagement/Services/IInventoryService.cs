using GlobalCoders.PSP.BackendApi.Base.Models;

namespace GlobalCoders.PSP.BackendApi.InventoryManagement.Services;

public interface IInventoryService
{
    Task<decimal?> GetQuantityAsync(Guid organizationId, Guid productId);
    Task<(ValidationDetails result, decimal quantity)> ChangeQuantityAsync(Guid organizationId, Guid productId, decimal quantityChange, CancellationToken cancellationToken);
}