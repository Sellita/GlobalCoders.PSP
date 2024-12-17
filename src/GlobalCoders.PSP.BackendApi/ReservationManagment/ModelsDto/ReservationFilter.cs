using GlobalCoders.PSP.BackendApi.Base.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.ReservationManagment.ModelsDto;

public class ReservationFilter : BaseFilter
{
    public Guid? EmployeeId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public Guid? MerchantId { get; set; }
    public string Name { get; set; } = string.Empty;
}