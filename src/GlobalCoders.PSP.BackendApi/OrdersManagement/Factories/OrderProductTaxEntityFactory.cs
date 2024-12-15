using GlobalCoders.PSP.BackendApi.OrdersManagement.Entities;
using GlobalCoders.PSP.BackendApi.TaxManagement.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.OrdersManagement.Factories;

public static class OrderProductTaxEntityFactory
{
    public static OrderProductTaxEntity Create(TaxListModel tax, decimal taxValue, Guid? productId)
    {
        return new OrderProductTaxEntity
        {
            OrderProductId = productId,
            Name = tax.DisplayName,
            Value = taxValue
        };
    }
}