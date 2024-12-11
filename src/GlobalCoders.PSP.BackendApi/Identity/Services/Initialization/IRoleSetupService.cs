namespace GlobalCoders.PSP.BackendApi.Identity.Services.Initialization;

public interface IRoleSetupService
{
    Task<bool> RunAsync(CancellationToken cancellationToken);
}
