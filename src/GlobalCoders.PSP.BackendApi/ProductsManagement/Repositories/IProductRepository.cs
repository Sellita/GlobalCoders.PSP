using GlobalCoders.PSP.BackendApi.ProductsManagement.Entities;
using GlobalCoders.PSP.BackendApi.ProductsManagement.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.ProductsManagement.Repositories;

public interface IProductRepository
{
    Task<bool> UpdateAsync(ProductEntity updateModel);
    Task<bool> CreateAsync(ProductEntity createModel);
    Task<(List<ProductEntity> items, int totalItems)> GetAllAsync(ProductFilter filter);
    Task<ProductEntity?> GetAsync(Guid productId, Guid? merchant);
    Task<bool> DeleteAsync(Guid organizationId);
}