namespace GlobalCoders.PSP.BackendApi.DiscountManagement.ModelsDto;

public class DiscountResponseModel
{
    public Guid Id { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }
    public double Percentage { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Status { get; set; }
    public int UsageLimit { get; set; }
    public int UsageCount { get; set; }
}
