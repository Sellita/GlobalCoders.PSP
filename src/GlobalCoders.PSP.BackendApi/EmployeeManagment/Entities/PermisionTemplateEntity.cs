using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities;

[Index(nameof(Enable))]
public class PermisionTemplateEntity : IdentityRole<Guid>
{
    public bool Enable { get; set; }//todo in doc we have enum

    public virtual ICollection<PermisionEntity> UserRoles { get; set; } = new List<PermisionEntity>();

    public PermisionTemplateEntity(string name) : base(name)
    {
    }
}
