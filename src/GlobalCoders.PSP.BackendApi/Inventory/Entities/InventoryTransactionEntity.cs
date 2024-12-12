using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GlobalCoders.PSP.BackendApi.EmployeeManagment.Constants;
using GlobalCoders.PSP.BackendApi.OrganizationManagment.Entities;
using GlobalCoders.PSP.BackendApi.ProductsManagment.Entities;
using Microsoft.EntityFrameworkCore;

namespace GlobalCoders.PSP.BackendApi.Inventory.Entities;

[Index(nameof(ProductId), nameof(MerchantId))]
public class InventoryTransactionEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public decimal Quantity { get; set; }

    public Guid ProductId { get; set; }
    public virtual ProductEntity Product { get; set; } = new ();
    public Guid MerchantId { get; set; }
    public virtual MerchantEntity Merchant { get; set; } = new ();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}