using GlobalCoders.PSP.BackendApi.ServicesManagement.Enum;

namespace GlobalCoders.PSP.BackendApi.ServicesManagement.ModelsDto;

public class ServiceCreateModel
{
    public string DisplayName { get; set; } = string.Empty;
    
    public string Description { get; set; } = string.Empty;

    public int DurationMin { get; set; }
    public decimal Price { get; set; }
    public ServiceState ServiceState { get; set; } 
    
    public Guid EmployeeId { get; set; }

    public DateTime CreationDate { get; set; } = DateTime.UtcNow;
    public DateTime LastUpdateDate { get; set; } = DateTime.UtcNow;

    public string Image { get; set; } = string.Empty;
}