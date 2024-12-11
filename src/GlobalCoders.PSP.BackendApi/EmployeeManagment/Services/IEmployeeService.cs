using GlobalCoders.PSP.BackendApi.Base.Models;
using GlobalCoders.PSP.BackendApi.Base.ModelsDto;
using GlobalCoders.PSP.BackendApi.EmployeeManagment.ModelsDto;
using GlobalCoders.PSP.BackendApi.OrganizationManagment.ModelsDto;

namespace GlobalCoders.PSP.BackendApi.EmployeeManagment.Services;

public interface IEmployeeService
{
    Task<EmployeeResponseModel?> GetAsync(Guid employeeId, CancellationToken cancellationToken);
    
    Task<BasePagedResponse<EmployeeResponseListModel>> GetAllAsync(EmployeeFilter filter, CancellationToken cancellationToken);

    Task<bool> DeleteAsync(Guid employeeId);
    Task<ValidationDetails> CreateAsync(EmployeeCreateRequest createRequest, CancellationToken cancellationToken);
    Task<ValidationDetails> UpdateAsync(EmployeeUpdateRequest updateRequest, CancellationToken cancellationToken);
}