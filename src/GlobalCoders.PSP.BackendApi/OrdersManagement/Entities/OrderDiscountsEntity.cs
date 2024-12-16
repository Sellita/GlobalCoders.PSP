using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GlobalCoders.PSP.BackendApi.DiscountManagement.Entities;
using GlobalCoders.PSP.BackendApi.TaxManagement.Constants;

namespace GlobalCoders.PSP.BackendApi.OrdersManagement.Entities;

public class OrderDiscountsEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    
    public Guid? OrderDiscountId { get; set; }
    
    public Guid DiscountId { get; set; }
    public virtual DiscountEntity? Discount { get; set; }

}