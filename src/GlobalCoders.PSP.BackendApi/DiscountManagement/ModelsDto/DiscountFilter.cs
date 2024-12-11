namespace GlobalCoders.PSP.BackendApi.DiscountManagement.ModelsDto;
public class DiscountFilter
{
    public string? Code { get; set; }
    public string? Status { get; set; }
    public decimal? Percentage { get; set; }
    public int Page { get; set; } = 1;
    public int ItemsPerPage { get; set; } = 10;
}
