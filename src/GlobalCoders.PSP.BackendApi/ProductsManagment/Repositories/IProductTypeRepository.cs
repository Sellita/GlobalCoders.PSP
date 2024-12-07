using GlobalCoders.PSP.BackendApi.ProductsManagment.Entities;
using GlobalCoders.PSP.BackendApi.ProductsManagment.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.ProductsManagment.Repositories;

public interface IProductTypeRepository
{
    Task<bool> UpdateAsync(ProductTypeEntity updateModel);
    Task<bool> CreateAsync(ProductTypeEntity createModel);
    Task<(List<ProductTypeEntity> items, int totalItems)> GetAllAsync(ProductTypeFilter filter);
    Task<ProductTypeEntity?> GetAsync(Guid organizationId);
    Task<bool> DeleteAsync(Guid organizationId);
}