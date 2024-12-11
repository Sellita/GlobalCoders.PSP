namespace GlobalCoders.PSP.BackendApi.Base.Services;

public interface IInitializeRequired
{
    Task InitializeAsync(CancellationToken cancellationToken);
    int Priority { get; }
}
