using GlobalCoders.PSP.BackendApi.ProductsManagement.Enum;

namespace GlobalCoders.PSP.BackendApi.ProductsManagement.ModelsDto;

public class ProductResponseModel
{
    public Guid Id { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public decimal? Stock { get; set; }
    public string? TaxName { get; set; } = string.Empty;
    public decimal? TaxValue { get; set; }
    
    public Guid CategoryId { get; set; }
    public string Category { get; set; } = string.Empty;
    
    public decimal Price { get; set; }
    public ProductState ProductState { get; set; }
    public Guid MerchantId { get; set; }
    public string Merchant { get; set; } = string.Empty;
    
    public DateTime CreationDate { get; set; }
    public DateTime LastUpdateDate { get; set; }
}