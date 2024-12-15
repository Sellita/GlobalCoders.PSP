using GlobalCoders.PSP.BackendApi.OrdersManagement.Entities;
using GlobalCoders.PSP.BackendApi.OrdersManagement.Enums;
using GlobalCoders.PSP.BackendApi.OrdersManagement.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.OrdersManagement.Factories;

public static class OrderEntityFactory
{
    public static OrderEntity Create(OrderCreateModel organizationCreateModel)
    {
        return new OrderEntity
        {
            ClientName = organizationCreateModel.ClientName,
            EmployeeId = organizationCreateModel.EmployeeId ?? throw new ArgumentException(nameof(organizationCreateModel.EmployeeId)),
            MerchantId = organizationCreateModel.MerchantId ?? throw new ArgumentException(nameof(organizationCreateModel.MerchantId)),
            Discount = organizationCreateModel.Discount ?? 0,
            Status = OrderStatus.Open, // Default Status
            CreatedAt = DateTime.UtcNow,
        };
    }  
    
    public static OrderEntity CreateUpdate(OrderUpdateModel updateModel)
    {
        var merchantEntity = Create(updateModel);

        merchantEntity.Id = updateModel.Id;
        
        return merchantEntity;
    }
}