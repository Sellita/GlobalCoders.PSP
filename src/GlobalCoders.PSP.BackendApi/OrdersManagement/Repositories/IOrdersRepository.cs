using GlobalCoders.PSP.BackendApi.OrdersManagement.Entities;
using GlobalCoders.PSP.BackendApi.OrdersManagement.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.OrdersManagement.Repositories;

public interface IOrdersRepository
{
    Task<bool> UpdateAsync(OrderEntity updateModel);
    Task<bool> CreateAsync(OrderEntity createModel);
    Task<(List<OrderEntity> items, int totalItems)> GetAllAsync(OrdersFilter filter);
    Task<OrderEntity?> GetAsync(Guid organizationId);
    Task<bool> DeleteAsync(Guid organizationId);
    Task<bool> OrderBelongsToMerchantAsync(Guid orderIdOrderId, Guid? userMerchantId);
    Task<(bool result, string message)> ChangeStatusAsync(OrderChangeStatusRequestModel orderChangeStatusRequest);
    Task<bool> DeleteProductFromLustAsync(OrderProductEntity productFromList);
}