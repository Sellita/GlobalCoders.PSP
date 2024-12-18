

using GlobalCoders.PSP.BackendApi.ReservationManagment.Repositories;
using GlobalCoders.PSP.BackendApi.ReservationManagment.Services;

namespace GlobalCoders.PSP.BackendApi.ReservationManagment.Extensions;

public static class ReservationManagementExtension
{
    public static void RegisterReservationManagementServices(this IServiceCollection services)
    {
        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddScoped<IReservationService, ReservationService>();
    }
    
    public static void RegisterReservationManagement(this WebApplication app)
    {

        //todo implement me
      
    }
}