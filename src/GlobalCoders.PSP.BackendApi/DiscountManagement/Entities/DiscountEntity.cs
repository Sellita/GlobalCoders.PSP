using System.ComponentModel.DataAnnotations;
using GlobalCoders.PSP.BackendApi.DiscountManagment.Constants;

namespace GlobalCoders.PSP.BackendApi.DiscountManagment.Entities;

public class DiscountEntity
{
    public Guid Id { get; set; }

    [StringLength(DiscountConstants.DefaultStringLimitation)]
    public string Code { get; set; } = string.Empty;

    [StringLength(DiscountConstants.DefaultStringLimitation)]
    public string Description { get; set; } = string.Empty;

    public double Percentage { get; set; } // Representing percentage discounts

    public int UsageLimit { get; set; } = 0; // Default to unlimited usage

    public int UsageCount { get; set; } = 0; // Tracks how many times the discount has been used

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;

    [StringLength(DiscountConstants.DefaultStringLimitation)]
    public string Status { get; set; } = "Active";
}