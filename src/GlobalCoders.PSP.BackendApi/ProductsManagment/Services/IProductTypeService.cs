using GlobalCoders.PSP.BackendApi.Base.ModelsDto;
using GlobalCoders.PSP.BackendApi.ProductsManagment.Entities;
using GlobalCoders.PSP.BackendApi.ProductsManagment.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.ProductsManagment.Services;

public interface IProductTypeService
{
    Task<bool> UpdateAsync(ProductTypeEntity updateModel);
    Task<bool> CreateAsync(ProductTypeEntity createModel);
    Task<BasePagedResponse<ProductTypeListModel>> GetAllAsync(ProductTypeFilter filter);
    Task<ProductTypeResponseModel?> GetAsync(Guid organizationId);
    Task<bool> DeleteAsync(Guid organizationId);
}