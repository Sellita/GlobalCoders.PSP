using System.ComponentModel.DataAnnotations;
using GlobalCoders.PSP.BackendApi.DiscountManagement.Constants;
using GlobalCoders.PSP.BackendApi.DiscountManagement.Enums;
using GlobalCoders.PSP.BackendApi.OrganizationManagment.Entities;
using GlobalCoders.PSP.BackendApi.ProductsManagment.Entities;

namespace GlobalCoders.PSP.BackendApi.DiscountManagement.Entities;

public class DiscountEntity
{
    public Guid Id { get; set; } // Primary Key
    
    [StringLength(DiscountConstants.DefaultStringLimitation)]
    public string Name { get; set; } = string.Empty; 
    
    public Guid? ProductTypeId { get; set; } // Foreign Key
    public virtual ProductTypeEntity? ProductType { get; set; } // Navigation Property
    
    public Guid? ProductId { get; set; } // Foreign Key
    public virtual ProductEntity? Product { get; set; } // Navigation Property
    
    public decimal Value { get; set; }
    
    public DiscountType Type { get; set; }
    
    public DateTime CreationDateTime { get; set; } // When it was created
    
    public DiscountStatus Status { get; set; }
    
    public bool IsDeleted { get; set; } 
    
    public Guid MerchantId { get; set; }
    public virtual MerchantEntity? Merchant { get; set; }
    
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}