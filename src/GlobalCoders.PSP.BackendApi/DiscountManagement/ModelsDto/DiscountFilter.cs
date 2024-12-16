using GlobalCoders.PSP.BackendApi.Base.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.DiscountManagement.ModelsDto;

public class DiscountFilter : BaseFilter
{
    public string DisplayName { get; set; } = string.Empty;
    public Guid? MerchantId { get; set; }
    public DateTime? Date { get; set; }
}