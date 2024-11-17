namespace GlobalCoders.PSP.BackendApi.Identity.Services.Initialization;

public interface IDefaultUserSetupService
{
    Task<bool> RunAsync(CancellationToken cancellationToken);
}
