using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GlobalCoders.PSP.BackendApi.TaxManagement.Constants;

namespace GlobalCoders.PSP.BackendApi.OrdersManagement.Entities;

public class OrderProductTaxEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public Guid? OrderProductId { get; set; }
    
    [StringLength(TaxConstants.DefaultStringLimitation)]
    public string Name { get; set; } = string.Empty; 
    public decimal Value { get; set; } 

    
}