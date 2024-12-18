using GlobalCoders.PSP.BackendApi.Base.ModelsDto;
using GlobalCoders.PSP.BackendApi.OrdersManagement.Entities;
using GlobalCoders.PSP.BackendApi.OrdersManagement.ModelsDto;
using GlobalCoders.PSP.BackendApi.PaymentsService.Models;

namespace GlobalCoders.PSP.BackendApi.OrdersManagement.Services;

public interface IOrdersService
{
    Task<(bool, string)> UpdateAsync(OrderEntity updateModel);
    Task<bool> CreateAsync(OrderEntity createModel, CancellationToken cancellationToken);
    Task<BasePagedResponse<OrdersListModel>> GetAllAsync(OrdersFilter filter);
    Task<OrderResponseModel?> GetAsync(Guid orderId);
    Task<bool> DeleteAsync(Guid organizationId);
    Task<bool> HasPermissionAsync(Guid orderId, Guid? userMerchantId);
    Task<(bool result, string message)> ChangeStatusAsync(OrderChangeStatusRequestModel orderChangeStatusRequest);
    Task<(bool result, string message)> ChangeOrderProductAsync(OrderChangeProductRequestModel orderChangeStatusRequest,
        CancellationToken cancellationToken);

    Task<(PaymentInfo? result, string message)> MakePaymentAsync(OrderMakePaymentRequestModel orderMakePaymentRequest, CancellationToken cancellationToken);
    Task<(bool result, string message)> ChangeTipsAsync(TipsRequestModel tipsRequest, CancellationToken cancellationToken);
    Task<bool> ConfirmPaymentAsync(Guid orderId, string sessionId);
    Task<bool> CancelPaymentAsync(Guid orderId, string sessionId);
    Task<(PaymentInfo? result, string message)> ResumePaymentAsync(OrderResumePaymentModel resumePaymentRequest, CancellationToken cancellationToken);
}