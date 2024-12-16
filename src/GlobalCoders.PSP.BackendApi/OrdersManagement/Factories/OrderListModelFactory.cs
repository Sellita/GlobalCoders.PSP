using GlobalCoders.PSP.BackendApi.OrdersManagement.Entities;
using GlobalCoders.PSP.BackendApi.OrdersManagement.Enums;
using GlobalCoders.PSP.BackendApi.OrdersManagement.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.OrdersManagement.Factories;

public static class OrderListModelFactory
{
    public static OrdersListModel Create(OrderEntity merchantEntity)
    {
        return new OrdersListModel
        {
            Id = merchantEntity.Id,
            Client = merchantEntity.ClientName,
            Date = merchantEntity.CreatedAt,
            Merchant = merchantEntity.Merchant?.DisplayName ?? string.Empty,
            Status = merchantEntity.Status,

        };
    }

    public static OrdersListModel Create (OrderResponseModel orderResponseModel)
    {
        return new OrdersListModel
        {
            Id = orderResponseModel.Id,
            Client = orderResponseModel.ClientName,
            Date = orderResponseModel.Date,
            Merchant = orderResponseModel.MerchantName,
            Status = orderResponseModel.Status,
        };
    }
}