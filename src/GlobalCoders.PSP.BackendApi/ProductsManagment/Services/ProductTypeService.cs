using GlobalCoders.PSP.BackendApi.Base.Factories;
using GlobalCoders.PSP.BackendApi.Base.ModelsDto;
using GlobalCoders.PSP.BackendApi.ProductsManagment.Entities;
using GlobalCoders.PSP.BackendApi.ProductsManagment.Factories;
using GlobalCoders.PSP.BackendApi.ProductsManagment.ModelsDto;
using GlobalCoders.PSP.BackendApi.ProductsManagment.Repositories;

namespace GlobalCoders.PSP.BackendApi.ProductsManagment.Services;

public class ProductTypeService : IProductTypeService
{
    private readonly IProductTypeRepository _productTypesRepository;

    public ProductTypeService(IProductTypeRepository productTypesRepository)
    {
        _productTypesRepository = productTypesRepository;
    }
    public async Task<bool> UpdateAsync(ProductTypeEntity updateModel)
    {
        return await _productTypesRepository.UpdateAsync(updateModel);
    }

    public async Task<bool> CreateAsync(ProductTypeEntity createModel)
    {
        return await _productTypesRepository.CreateAsync(createModel);
    }

    public async Task<BasePagedResponse<ProductTypeListModel>> GetAllAsync(ProductTypeFilter filter)
    {
        var entities = await _productTypesRepository.GetAllAsync(filter);
        
        var models = entities.items.Select(ProductTypeListModelFactory.Create).ToList();

        return BasePagedResopnseFactory.Create(models, filter, entities.totalItems);
    }

    public async Task<ProductTypeResponseModel?> GetAsync(Guid organizationId)
    {
        var entity = await _productTypesRepository.GetAsync(organizationId);
        
        return ProductTypeResponseModelFactory.Create(entity);
    }

    public Task<bool> DeleteAsync(Guid organizationId)
    {
        return _productTypesRepository.DeleteAsync(organizationId);
    }
}