using GlobalCoders.PSP.BackendApi.Base.Services;

namespace GlobalCoders.PSP.BackendApi.Base.Extensions;

public static class InitializeRequiredExtension
{
    public static async Task InitializeRequiredServicesAsync(this IHost host)
    {
        var hostApplication = host.Services.GetRequiredService<IHostApplicationLifetime>();

        await using var serviceScope = host.Services.CreateAsyncScope();

        var services = serviceScope.ServiceProvider.GetServices<IInitializeRequired>().OrderBy(x=>x.Priority);

        foreach (var service in services)
        {
            await service.InitializeAsync(hostApplication.ApplicationStopped);
        }
    }
}
