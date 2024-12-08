namespace GlobalCoders.PSP.BackendApi.ProductsManagment.ModelsDto;

public class ProductTypeCreateModel
{
    public string DisplayName { get; set; } = string.Empty;
    public Guid? MerchantId { get; set; }
}