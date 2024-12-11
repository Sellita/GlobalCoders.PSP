namespace GlobalCoders.PSP.BackendApi.DiscountManagment.ModelsDto;

public class DiscountListModel
{
    public Guid Id { get; set; } 
    public string Code { get; set; } = string.Empty; // Discount code
    public string Description { get; set; } = string.Empty;
    public double Percentage { get; set; } // Percentage discount value (if applicable)
    public int UsageLimit { get; set; } // Maximum number of times the discount can be used
    public int UsageCount { get; set; } // Number of times the discount has already been used
    public string Status { get; set; } = "Active"; 
    public DateTime StartDate { get; set; } // Date when the discount starts
    public DateTime EndDate { get; set; } 
}