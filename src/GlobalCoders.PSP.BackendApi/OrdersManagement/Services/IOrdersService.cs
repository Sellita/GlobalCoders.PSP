using GlobalCoders.PSP.BackendApi.Base.ModelsDto;
using GlobalCoders.PSP.BackendApi.OrdersManagement.Entities;
using GlobalCoders.PSP.BackendApi.OrdersManagement.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.OrdersManagement.Services;

public interface IOrdersService
{
    Task<bool> UpdateAsync(OrderEntity updateModel);
    Task<bool> CreateAsync(OrderEntity createModel, CancellationToken cancellationToken);
    Task<BasePagedResponse<OrdersListModel>> GetAllAsync(OrdersFilter filter);
    Task<OrderResponseModel?> GetAsync(Guid organizationId);
    Task<bool> DeleteAsync(Guid organizationId);
    Task<bool> HasPermissionAsync(Guid orderId, Guid? userMerchantId);
    Task<(bool result, string message)> ChangeStatusAsync(OrderChangeStatusRequestModel orderChangeStatusRequest);
    Task<(bool result, string message)> ChangeOrderProductAsync(OrderChangeProductRequestModel orderChangeStatusRequest,
        CancellationToken cancellationToken);

    Task<(bool result, string message)> MakePaymentAsync(OrderMakePaymentRequestModel orderMakePaymentRequest, CancellationToken cancellationToken);
}