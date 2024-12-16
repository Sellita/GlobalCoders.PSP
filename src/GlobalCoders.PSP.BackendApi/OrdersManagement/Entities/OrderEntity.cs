using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GlobalCoders.PSP.BackendApi.EmployeeManagment.Constants;
using GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities;
using GlobalCoders.PSP.BackendApi.OrdersManagement.Enums;
using GlobalCoders.PSP.BackendApi.OrganizationManagment.Entities;

namespace GlobalCoders.PSP.BackendApi.OrdersManagement.Entities;

public class OrderEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public string ClientName { get; set; } = string.Empty;

    public decimal Tips { get; set; } //create method for this 
    public OrderStatus Status { get; set; } //create method for this

    public Guid EmployeeId { get; set; }
    public virtual EmployeeEntity? Employee { get; set; }

    public Guid MerchantId { get; set; }
    public virtual MerchantEntity? Merchant { get; set; }
    
    public virtual ICollection<OrderProductEntity> OrderProducts { get; set; } = [];
    public virtual ICollection<OrderPaymentsEntity> OrderPayments { get; set; } = [];
    public virtual ICollection<OrderDiscountsEntity> OrderDiscounts { get; set; } = [];
    
    public decimal Discount { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal TotalPriceWithDiscount { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public bool IsDeleted { get; set; }
}