using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GlobalCoders.PSP.BackendApi.EmployeeManagment.Constants;
using GlobalCoders.PSP.BackendApi.OrganizationManagment.Entities;
using GlobalCoders.PSP.BackendApi.ProductsManagment.Enum;

namespace GlobalCoders.PSP.BackendApi.ProductsManagment.Entities;

public class ProductEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [StringLength(EmployeeConstants.DefaultStringLimitation)]
    public string DisplayName { get; set; } = string.Empty; 
    public string Description { get; set; } = string.Empty;
    
    public decimal Price { get; set; }
    public ProductState ProductState { get; set; } 
    
    public Guid ProductTypeId { get; set; }
    public virtual ProductTypeEntity ProductType { get; set; } = new ();
    
    public Guid MerchantId { get; set; }
    public virtual MerchantEntity Merchant { get; set; } = new ();
    
    public DateTime CreationDate { get; set; }
    public DateTime LastUpdateDate { get; set; }

    public string Image { get; set; } = string.Empty;

    public bool IsDeleted { get; set; }
}