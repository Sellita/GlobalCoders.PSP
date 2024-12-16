using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GlobalCoders.PSP.BackendApi.EmployeeManagment.Constants;
using GlobalCoders.PSP.BackendApi.ProductsManagement.Entities;

namespace GlobalCoders.PSP.BackendApi.OrdersManagement.Entities;

public class OrderProductEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public Guid? OrderId { get; set; }

    public Guid ProductId { get; set; }
    public virtual ProductEntity? Product { get; set; }

    [StringLength(EmployeeConstants.DefaultStringLimitation)]
    public string ProductName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    
    public virtual ICollection<OrderProductTaxEntity> OrderProductTaxes { get; set; } = [];
    public decimal Discount { get; set; }
    
    public decimal Quantity { get; set; }
}