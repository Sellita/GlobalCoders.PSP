namespace GlobalCoders.PSP.BackendApi.ProductsManagement.ModelsDto;

public class ProductTypeCreateModel
{
    public string DisplayName { get; set; } = string.Empty;
    public Guid? MerchantId { get; set; }
}