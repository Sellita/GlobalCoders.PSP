using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GlobalCoders.PSP.BackendApi.PaymentsService.Enums;

namespace GlobalCoders.PSP.BackendApi.OrdersManagement.Entities;

public class OrderPaymentsEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public Guid? OrderId { get; set; }
    
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public PaymentType Type { get; set; }
}