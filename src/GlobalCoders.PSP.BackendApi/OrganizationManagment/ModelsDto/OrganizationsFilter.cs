using GlobalCoders.PSP.BackendApi.Base.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.OrganizationManagment.ModelsDto;

public class OrganizationsFilter : BaseFilter
{
    public string DisplayName { get; set; } = string.Empty;
}