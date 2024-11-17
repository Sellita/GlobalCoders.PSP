using GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities;

namespace GlobalCoders.PSP.BackendApi.Identity.Factories;

public static class AppUserModelsFactory
{
    public static EmployeeEntity Create(
        string email,
        bool emailConfirmed,
        bool isActive = true)
    {
        return new EmployeeEntity
        {
            UserName = email,
            NormalizedUserName = email.ToUpper(),
            Email = email,
            NormalizedEmail = email.ToUpper(),
            EmailConfirmed = emailConfirmed,

            IsActive = isActive
        };
    }
    
}
