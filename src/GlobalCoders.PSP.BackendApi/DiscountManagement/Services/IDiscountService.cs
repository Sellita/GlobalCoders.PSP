using GlobalCoders.PSP.BackendApi.Base.ModelsDto;
using GlobalCoders.PSP.BackendApi.DiscountManagement.ModelsDto;
using GlobalCoders.PSP.BackendApi.DiscountManagment.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.DiscountManagment.Services;


public interface IDiscountService
{
    Task<bool> CreateAsync(DiscountCreateModel model);
    Task<bool> UpdateAsync(DiscountUpdateModel model);
    Task<BasePagedResponse<DiscountListModel>> GetAllAsync(DiscountFilter filter);
    Task<DiscountResponseModel?> GetAsync(Guid id);
    Task<bool> DeleteAsync(Guid id);
}

