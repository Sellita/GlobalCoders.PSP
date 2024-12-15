using System.ComponentModel.DataAnnotations;

namespace GlobalCoders.PSP.BackendApi.OrdersManagement.ModelsDto;

public class OrderChangeProductRequestModel
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public decimal Quantity { get; set; }
}