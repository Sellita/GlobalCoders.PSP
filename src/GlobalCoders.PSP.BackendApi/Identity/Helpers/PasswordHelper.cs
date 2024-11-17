using GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities;
using Microsoft.AspNetCore.Identity;

namespace GlobalCoders.PSP.BackendApi.Identity.Helpers;

public static class PasswordHelper
{
    public static string GetPasswordHash(EmployeeEntity user, string password)
    {
        var hasher = new PasswordHasher<EmployeeEntity>();

        return hasher.HashPassword(user, password);
    }
}
