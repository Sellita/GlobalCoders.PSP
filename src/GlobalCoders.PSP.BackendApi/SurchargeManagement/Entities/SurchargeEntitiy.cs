using System.ComponentModel.DataAnnotations;
using GlobalCoders.PSP.BackendApi.SurchargeManagement.Constants;

namespace GlobalCoders.PSP.BackendApi.SurchargeManagement.Entities;

public class SurchargeEntity
{
    public Guid Id { get; set; } // Primary Key

    [StringLength(SurchargeConstants.DefaultStringLimitation)]
    public string Name { get; set; } = String.Empty; // Name of the surcharge

    public decimal Value { get; set; } // Value of the surcharge

    [StringLength(SurchargeConstants.DefaultStringLimitation)]
    public string Type { get; set; } = String.Empty; // "Percentage" or "Value"

    public DateTime CreationDateTime { get; set; } // When it was created

    [StringLength(SurchargeConstants.DefaultStringLimitation)]
    public string Status { get; set; } = String.Empty; // "Active" or "Inactive"

    [StringLength(SurchargeConstants.DefaultStringLimitation)]
    public string Minute { get; set; } = String.Empty;

    [StringLength(SurchargeConstants.DefaultStringLimitation)]
    public string Hour { get; set; } = String.Empty;

    [StringLength(SurchargeConstants.DefaultStringLimitation)]
    public string DayOfMonth { get; set; } = String.Empty;

    [StringLength(SurchargeConstants.DefaultStringLimitation)]
    public string Month { get; set; } = String.Empty;

    [StringLength(SurchargeConstants.DefaultStringLimitation)]
    public string DayOfWeek { get; set; } = String.Empty;
    public bool IsDeleted { get; set; }
}