using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GlobalCoders.PSP.BackendApi.EmployeeManagment.Constants;
using GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities;
using GlobalCoders.PSP.BackendApi.ServicesManagement.Entities;
using GlobalCoders.PSP.BackendApi.ServicesManagement.Enum;

namespace GlobalCoders.PSP.BackendApi.ReservationManagment.Entities;

public class ReservationEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    [StringLength(EmployeeConstants.DefaultStringLimitation)]
    public string DisplayName { get; set; } = string.Empty; 
    [StringLength(EmployeeConstants.DefaultStringLimitation)]
    public string Description { get; set; } = string.Empty;
    
    [StringLength(EmployeeConstants.DefaultStringLimitation)]
    public string CustomerName { get; set; } = string.Empty;
    
    public decimal Price { get; set; }

    public DateTime CreateTime { get; set; }
    public DateTime ReservationTime { get; set; }
    public DateTime ReservationEndTime { get; set; }
    public int DurationMin { get; set; }
    
    public Guid ServiceId { get; set; }
    public virtual ServiceEntity? Service { get; set; }

    public Guid EmployeeId { get; set; }
    public virtual EmployeeEntity? Employee { get; set; }
    
    public bool IsDeleted { get; set; }
}