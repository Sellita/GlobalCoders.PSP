using Microsoft.AspNetCore.Identity;

namespace GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities;

public class PermisionEntity : IdentityUserRole<Guid>
{
    public override Guid RoleId { get; set; }
    public virtual EmployeeEntity? Employee { get; set; }
    public virtual PermisionTemplateEntity? AppRole { get; set; }

    public DateTime CreationDateTime { get; set; }
}
