using GlobalCoders.PSP.BackendApi.Base.ModelsDto;
using GlobalCoders.PSP.BackendApi.ProductsManagement.Entities;
using GlobalCoders.PSP.BackendApi.ProductsManagement.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.ProductsManagement.Services;

public interface IProductTypeService
{
    Task<bool> UpdateAsync(ProductTypeEntity updateModel);
    Task<bool> CreateAsync(ProductTypeEntity createModel);
    Task<BasePagedResponse<ProductTypeListModel>> GetAllAsync(ProductTypeFilter filter);
    Task<ProductTypeResponseModel?> GetAsync(Guid organizationId);
    Task<bool> DeleteAsync(Guid organizationId);
}