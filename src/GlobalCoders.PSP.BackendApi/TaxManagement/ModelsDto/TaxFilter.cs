using GlobalCoders.PSP.BackendApi.Base.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.TaxManagement.ModelsDto;

public class TaxFilter : BaseFilter
{
    public string DisplayName { get; set; } = string.Empty;
}