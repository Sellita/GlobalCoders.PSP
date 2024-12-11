using GlobalCoders.PSP.BackendApi.Base.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.EmployeeManagment.ModelsDto;

public class EmployeeFilter: BaseFilter
{   
    public string? SearchValue { get; set; }
    public Guid? OrganizationId { get; set; }
    public bool? IsActive { get; set; }
}