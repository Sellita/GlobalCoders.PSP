using GlobalCoders.PSP.BackendApi.ServicesManagement.Enum;

namespace GlobalCoders.PSP.BackendApi.ServicesManagement.ModelsDto;

public class ServiceListModel
{
    public Guid Id { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public int DurationMin { get; set; }
    
    public decimal Price { get; set; }
    public ServiceState ServiceState { get; set; }
    public Guid EmployeeId { get; set; }
    public string Employee { get; set; } = string.Empty;
    
    public DateTime CreationDate { get; set; }
    public DateTime LastUpdateDate { get; set; }
    
}