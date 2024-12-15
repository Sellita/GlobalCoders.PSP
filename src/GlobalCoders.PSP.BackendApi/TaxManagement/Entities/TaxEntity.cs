using System.ComponentModel.DataAnnotations;
using GlobalCoders.PSP.BackendApi.OrganizationManagment.Entities;
using GlobalCoders.PSP.BackendApi.ProductsManagment.Entities;
using GlobalCoders.PSP.BackendApi.TaxManagement.Enums;
using GlobalCoders.PSP.BackendApi.TaxManagement.Constants;

namespace GlobalCoders.PSP.BackendApi.TaxManagement.Entities;

public class TaxEntity
{
    public Guid Id { get; set; } // Primary Key
    
    [StringLength(Constants.TaxConstants.DefaultStringLimitation)]
    public string Name { get; set; } = string.Empty; // Name of the tax
    
    public Guid? ProductTypeId { get; set; } // Foreign Key
    public virtual ProductTypeEntity? ProductType { get; set; } // Navigation Property
    
    public decimal Value { get; set; }
    
    public TaxType Type { get; set; }
    
    public DateTime CreationDateTime { get; set; } // When it was created
    
    [StringLength(TaxConstants.DefaultStringLimitation)]
    public TaxStatus Status { get; set; }
    
    public bool IsDeleted { get; set; } 
    
    public Guid MerchantId { get; set; }
    public virtual MerchantEntity? Merchant { get; set; }
}