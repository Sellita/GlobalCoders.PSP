using GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities;

namespace GlobalCoders.PSP.BackendApi.Identity.Factories;

public static class AppRoleModelsFactory
{
    public static PermisionTemplateEntity CreateAppRole(string name, bool enable = true)
    {
        var role = new PermisionTemplateEntity(name)
        {
            Enable = enable
        };

        return role;
    }
}
