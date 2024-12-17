using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GlobalCoders.PSP.BackendApi.EmployeeManagment.Constants;
using GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities;
using GlobalCoders.PSP.BackendApi.OrganizationManagment.Entities;
using GlobalCoders.PSP.BackendApi.ServicesManagement.Enum;

namespace GlobalCoders.PSP.BackendApi.ServicesManagement.Entities;

public class ServiceEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [StringLength(EmployeeConstants.DefaultStringLimitation)]
    public string DisplayName { get; set; } = string.Empty; 
    [StringLength(EmployeeConstants.DefaultStringLimitation)]
    public string Description { get; set; } = string.Empty;
    
    public decimal Price { get; set; }
    public ServiceState ServiceState { get; set; }

    public int DurationMin { get; set; }

    public Guid EmployeeId { get; set; }
    public virtual EmployeeEntity? Employee { get; set; }
    
    public DateTime CreationDate { get; set; }
    public DateTime LastUpdateDate { get; set; }

    public string Image { get; set; } = string.Empty;

    public bool IsDeleted { get; set; }
}