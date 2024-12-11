namespace GlobalCoders.PSP.BackendApi.DiscountManagment.ModelsDto;

public class DiscountApplicationResponseModel
{
    /// <summary>
    /// The original amount before the discount is applied.
    /// </summary>
    public decimal OriginalAmount { get; set; }

    /// <summary>
    /// The amount of discount applied.
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// The final amount after the discount is applied.
    /// </summary>
    public decimal FinalAmount { get; set; }

    /// <summary>
    /// The name or code of the discount applied.
    /// </summary>
    public string? DiscountCode { get; set; }

    /// <summary>
    /// Optional message or additional information (e.g., success or failure details).
    /// </summary>
    public string? Message { get; set; }
}