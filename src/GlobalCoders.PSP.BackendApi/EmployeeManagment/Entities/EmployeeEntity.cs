using System.ComponentModel.DataAnnotations;
using GlobalCoders.PSP.BackendApi.Base.Enums;
using GlobalCoders.PSP.BackendApi.EmployeeManagment.Constants;
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

    public Roles Role { get; set; }

    public DateTime CreationDateTime { get; set; }
    public bool IsActive { get; set; } //todo in doc this is enum

    [StringLength(EmployeeConstants.DefaultStringLimitation)]
    public string Minute { get; set; } = string.Empty;

    [StringLength(EmployeeConstants.DefaultStringLimitation)]
    public string Hour { get; set; } = string.Empty;

    [StringLength(EmployeeConstants.DefaultStringLimitation)]
    public string DayMounth { get; set; } = string.Empty;

    [StringLength(EmployeeConstants.DefaultStringLimitation)]
    public string Mounth { get; set; } = string.Empty;

    [StringLength(EmployeeConstants.DefaultStringLimitation)]
    public string DayWeek { get; set; } = string.Empty;

    public virtual MerchantEntity Merchant { get; set; } = new();
    public virtual ICollection<PermisionEntity> UserPermissions { get; set; } = [];

    public bool IsDeleted { get; set; }

    public EmployeeEntity()
    {
        CreationDateTime = DateTime.UtcNow;
    }
}