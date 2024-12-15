using System.ComponentModel.DataAnnotations;
using GlobalCoders.PSP.BackendApi.Base.Enums;
using GlobalCoders.PSP.BackendApi.EmployeeManagment.Constants;
using GlobalCoders.PSP.BackendApi.OrganizationManagment.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities;

[Index(nameof(IsActive))]
[Index(nameof(Name))]
[Index(nameof(IsDeleted))]
public class EmployeeEntity : IdentityUser<Guid>
{
    [StringLength(EmployeeConstants.DefaultStringLimitation)]
    public string Name { get; set; } = string.Empty;
    
    public DateTime CreationDateTime { get; set; }
    public bool IsActive { get; set; } 
    
    public Guid? MerchantId { get; set; }
    public virtual MerchantEntity? Merchant { get; set; }
    public virtual ICollection<PermisionEntity> UserPermissions { get; set; } = [];
    public virtual ICollection<EmployeeScheduleEntity> WorkingSchedule { get; set; } = [];

    public bool IsDeleted { get; set; }

    public EmployeeEntity()
    {
        CreationDateTime = DateTime.UtcNow;
    }
}