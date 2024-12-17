using GlobalCoders.PSP.BackendApi.Base.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.ServicesManagement.ModelsDto;

public class ServiceFilter : BaseFilter
{
    public Guid? MerchantId { get; set; }
    public string Name { get; set; } = string.Empty;
}