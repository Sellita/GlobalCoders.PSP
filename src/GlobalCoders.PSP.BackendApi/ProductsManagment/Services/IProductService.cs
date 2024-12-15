using GlobalCoders.PSP.BackendApi.Base.ModelsDto;
using GlobalCoders.PSP.BackendApi.ProductsManagment.Entities;
using GlobalCoders.PSP.BackendApi.ProductsManagment.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.ProductsManagment.Services;

public interface IProductService
{
    Task<bool> UpdateAsync(ProductEntity updateModel);
    Task<bool> CreateAsync(ProductEntity createModel);
    Task<BasePagedResponse<ProductListModel>> GetAllAsync(ProductFilter filter);
    Task<ProductResponseModel?> GetAsync(Guid productId);
    Task<bool> DeleteAsync(Guid organizationId);
}