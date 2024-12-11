namespace GlobalCoders.PSP.BackendApi.DiscountManagment.ModelsDto
{
    public class DiscountUpdateModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Percentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int UsageLimit { get; set; }
        public string Status { get; set; } = "Active";
    }
}