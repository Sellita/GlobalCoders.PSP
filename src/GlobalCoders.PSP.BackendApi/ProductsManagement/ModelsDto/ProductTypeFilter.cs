using GlobalCoders.PSP.BackendApi.Base.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.ProductsManagement.ModelsDto;

public class ProductTypeFilter : BaseFilter
{
    public string DisplayName { get; set; } = string.Empty;
}