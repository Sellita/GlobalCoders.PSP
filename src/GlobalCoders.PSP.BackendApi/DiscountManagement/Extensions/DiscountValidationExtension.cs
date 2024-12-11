using GlobalCoders.PSP.BackendApi.DiscountManagment.Entities;

namespace GlobalCoders.PSP.BackendApi.DiscountManagement.Extensions;

public static class DiscountValidationExtensions
{
    /// <summary>
    /// Determines if the discount is valid for use based on its status, end date, and usage limits.
    /// </summary>
    /// <param name="discount">The discount entity to validate.</param>
    /// <returns>True if the discount is valid; otherwise, false.</returns>
    public static bool IsValidDiscount(this DiscountEntity discount)
    {
        if (discount == null) throw new ArgumentNullException(nameof(discount));

        return discount.Status == "Active" &&
               discount.EndDate >= DateTime.UtcNow &&
               (discount.UsageLimit == 0 || discount.UsageCount < discount.UsageLimit);
    }

    /// <summary>
    /// Validates the properties of a discount entity and returns error messages.
    /// </summary>
    /// <param name="discount">The discount entity to validate.</param>
    /// <returns>A string containing error messages, or an empty string if valid.</returns>
    public static string ValidateDiscount(this DiscountEntity discount)
    {
        if (discount == null) throw new ArgumentNullException(nameof(discount));

        var errors = new List<string>();

        if (string.IsNullOrEmpty(discount.Code))
            errors.Add("Discount code cannot be empty.");
        if (discount.Percentage < 0 || discount.Percentage > 100)
            errors.Add("Discount percentage must be between 0 and 100.");
        if (discount.UsageLimit < 0)
            errors.Add("Usage limit cannot be negative.");
        if (discount.UsageCount < 0)
            errors.Add("Usage count cannot be negative.");
        if (discount.StartDate > discount.EndDate)
            errors.Add("Start date cannot be later than end date.");
        if (discount.EndDate < DateTime.UtcNow)
            errors.Add("Discount end date must be in the future.");

        return string.Join(" ", errors);
    }
}
