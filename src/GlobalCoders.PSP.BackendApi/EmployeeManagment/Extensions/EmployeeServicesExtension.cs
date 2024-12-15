using GlobalCoders.PSP.BackendApi.EmployeeManagment.Controllers;
using GlobalCoders.PSP.BackendApi.EmployeeManagment.Repositories;
using GlobalCoders.PSP.BackendApi.EmployeeManagment.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace GlobalCoders.PSP.BackendApi.EmployeeManagment.Extensions;

public static class EmployeeServicesExtension
{
    public static void RegisterEmployeeServices(this IServiceCollection services)
    {
        //todo implement me
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<EmployeeController>();
    }
    
    public static void RegisterEmployees(this WebApplication app)
    {

        //todo implement me
      
    }
}