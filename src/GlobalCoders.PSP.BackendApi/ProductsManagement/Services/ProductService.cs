using GlobalCoders.PSP.BackendApi.Base.Factories;
using GlobalCoders.PSP.BackendApi.Base.ModelsDto;
using GlobalCoders.PSP.BackendApi.ProductsManagement.Entities;
using GlobalCoders.PSP.BackendApi.ProductsManagement.Factories;
using GlobalCoders.PSP.BackendApi.ProductsManagement.ModelsDto;
using GlobalCoders.PSP.BackendApi.ProductsManagement.Repositories;

namespace GlobalCoders.PSP.BackendApi.ProductsManagement.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<bool> UpdateAsync(ProductEntity updateModel)
    {
        return await _productRepository.UpdateAsync(updateModel);
    }

    public async Task<bool> CreateAsync(ProductEntity createModel)
    {
        return await _productRepository.CreateAsync(createModel);
    }

    public async Task<BasePagedResponse<ProductListModel>> GetAllAsync(ProductFilter filter)
    {
        var entities = await _productRepository.GetAllAsync(filter);
        
        var models = entities.items.Select(ProductListModelFactory.Create).ToList();

        return BasePagedResopnseFactory.Create(models, filter, entities.totalItems);
    }

    public async Task<ProductResponseModel?> GetAsync(Guid productId)
    {
        var entity = await _productRepository.GetAsync(productId);
        
        if(entity == null)
        {
            return null;
        }
        
        return ProductResponseModelFactory.Create(entity);
    }

    public Task<bool> DeleteAsync(Guid organizationId)
    {
        return _productRepository.DeleteAsync(organizationId);
    }
}