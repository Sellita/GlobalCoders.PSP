using GlobalCoders.PSP.BackendApi.Base.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.SurchargeManagement.ModelsDto;

public class SurchargeFilter : BaseFilter
{
    public string DisplayName { get; set; } = string.Empty;
}