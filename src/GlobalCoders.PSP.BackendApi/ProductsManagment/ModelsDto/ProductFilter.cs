using GlobalCoders.PSP.BackendApi.Base.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.ProductsManagment.ModelsDto;

public class ProductFilter : BaseFilter
{
    public Guid? MerchantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid? CategoryId { get; set; }
}