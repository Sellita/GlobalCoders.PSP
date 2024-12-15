using System.ComponentModel.DataAnnotations;
using GlobalCoders.PSP.BackendApi.OrganizationManagment.Entities;
using GlobalCoders.PSP.BackendApi.TaxManagement.Enums;
using GlobalCoders.PSP.BackendApi.TaxManagement.Constants;

namespace GlobalCoders.PSP.BackendApi.TaxManagement.Entities;

public class TaxEntity
{
    public Guid Id { get; set; } // Primary Key
    
    [StringLength(Constants.TaxConstants.DefaultStringLimitation)]
    public string Name { get; set; } = string.Empty; // Name of the tax
    
    public decimal Value { get; set; }
    
    public TaxType Type { get; set; }
    
    public DateTime CreationDateTime { get; set; } // When it was created
    
    [StringLength(TaxConstants.DefaultStringLimitation)]
    public TaxStatus Status { get; set; }
    
    [StringLength(TaxConstants.DefaultStringLimitation)]
    public string Minute { get; set; } = String.Empty;
    
    [StringLength(TaxConstants.DefaultStringLimitation)]
    public string Hour { get; set; } = string.Empty;
    
    [StringLength(TaxConstants.DefaultStringLimitation)]
    public string DayOfMonth { get; set; } = string.Empty;
    
    [StringLength(TaxConstants.DefaultStringLimitation)]
    public string Month { get; set; } = string.Empty;
    
    [StringLength(TaxConstants.DefaultStringLimitation)]
    public string DayOfWeek { get; set; } = string.Empty;
    
    public bool IsDeleted { get; set; } 
    
    public Guid MerchantId { get; set; }
    public virtual MerchantEntity? Merchant { get; set; }
    
    
    
}