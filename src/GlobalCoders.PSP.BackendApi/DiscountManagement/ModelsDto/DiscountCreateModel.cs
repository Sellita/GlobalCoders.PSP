namespace GlobalCoders.PSP.BackendApi.DiscountManagement.ModelsDto
{
    public class DiscountCreateModel
    {
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Percentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int UsageLimit { get; set; }
    }
}
