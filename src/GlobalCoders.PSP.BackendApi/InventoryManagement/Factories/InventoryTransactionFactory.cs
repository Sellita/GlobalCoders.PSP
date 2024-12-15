using GlobalCoders.PSP.BackendApi.InventoryManagement.Entities;

namespace GlobalCoders.PSP.BackendApi.InventoryManagement.Factories;

public static class InventoryTransactionFactory
{
    public static InventoryTransactionEntity Create(Guid merchantId, Guid productId, decimal quantity)
    {
        return new InventoryTransactionEntity
        {
            Quantity = quantity,
            ProductId = productId,
            MerchantId = merchantId,

        };
    }
}