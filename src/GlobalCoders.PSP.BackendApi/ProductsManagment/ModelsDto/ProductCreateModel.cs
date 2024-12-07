using GlobalCoders.PSP.BackendApi.ProductsManagment.Enum;

namespace GlobalCoders.PSP.BackendApi.ProductsManagment.ModelsDto;

public class ProductCreateModel
{
    public string DisplayName { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;
    
    public decimal Price { get; set; }
    public ProductState ProductState { get; set; } 
    
    public Guid ProductTypeId { get; set; }
    
    public Guid MerchantId { get; set; }

    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    public DateTime LastUpdateDate { get; set; } = DateTime.UtcNow;

    public string Image { get; set; } = string.Empty;
}