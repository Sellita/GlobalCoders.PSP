using GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities;
using GlobalCoders.PSP.BackendApi.Identity.Models;
using Microsoft.AspNetCore.Mvc;

namespace GlobalCoders.PSP.BackendApi.OrdersManagement.Factories;

public static class ProfileUserInfoModelsFactory
{
    public static ProfileUserInfo CreateProfileUserInfo(EmployeeEntity employee)
    {
        return new ProfileUserInfo
        {
            UserId = employee.Id,
            UserName = employee.Name,
            Email = employee.Email ?? string.Empty,
            MerchantName = employee.Merchant?.DisplayName,
            MerchantId = employee.MerchantId,

        };
    }
}
