using GlobalCoders.PSP.BackendApi.ProductsManagment.Entities;
using GlobalCoders.PSP.BackendApi.ProductsManagment.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.ProductsManagment.Repositories;

public interface IProductRepository
{
    Task<bool> UpdateAsync(ProductEntity updateModel);
    Task<bool> CreateAsync(ProductEntity createModel);
    Task<(List<ProductEntity> items, int totalItems)> GetAllAsync(ProductFilter filter);
    Task<ProductEntity?> GetAsync(Guid productId);
    Task<bool> DeleteAsync(Guid organizationId);
}