using GlobalCoders.PSP.BackendApi.Base.ModelsDto;
using GlobalCoders.PSP.BackendApi.ProductsManagement.Entities;
using GlobalCoders.PSP.BackendApi.ProductsManagement.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.ProductsManagement.Services;

public interface IProductService
{
    Task<bool> UpdateAsync(ProductEntity updateModel);
    Task<bool> CreateAsync(ProductEntity createModel);
    Task<BasePagedResponse<ProductListModel>> GetAllAsync(ProductFilter filter);
    Task<ProductResponseModel?> GetAsync(Guid productId);
    Task<bool> DeleteAsync(Guid organizationId);
}