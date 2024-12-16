using GlobalCoders.PSP.BackendApi.ProductsManagement.Entities;
using GlobalCoders.PSP.BackendApi.ProductsManagement.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.ProductsManagement.Repositories;

public interface IProductTypeRepository
{
    Task<bool> UpdateAsync(ProductTypeEntity updateModel);
    Task<bool> CreateAsync(ProductTypeEntity createModel);
    Task<(List<ProductTypeEntity> items, int totalItems)> GetAllAsync(ProductTypeFilter filter);
    Task<ProductTypeEntity?> GetAsync(Guid organizationId);
    Task<bool> DeleteAsync(Guid organizationId);
}