namespace GlobalCoders.PSP.BackendApi.InventoryManagement.ModelsDto;

public class InventoryQuantityChangeRequest
{
    public Guid OrganizationId { get; set; }
    public Guid ProductId { get; set; }
    
    public decimal QuantityChange { get; set; }
}