using GlobalCoders.PSP.BackendApi.Base.Extensions;
using GlobalCoders.PSP.BackendApi.Base.Services;

namespace GlobalCoders.PSP.BackendApi.Identity.Services.Initialization;

public sealed class IdentityServiceSetupInitializeRequired : IInitializeRequired
{
    private readonly ILogger<IdentityServiceSetupInitializeRequired> _logger;
    private readonly IRoleSetupService _roleSetupService;
    private readonly IDefaultUserSetupService _defaultUserSetupService;
    
    public int Priority => 5;
    
    public IdentityServiceSetupInitializeRequired(
        ILogger<IdentityServiceSetupInitializeRequired> logger,
        IRoleSetupService roleSetupService,
        IDefaultUserSetupService defaultUserSetupService)
    {
        _logger = logger;
        _roleSetupService = roleSetupService;
        _defaultUserSetupService = defaultUserSetupService;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        try
        {
            if (!await _roleSetupService.RunAsync(cancellationToken))
            {
                _logger.LogError("Something was wrong when run {Service}", nameof(RoleSetupService));

                Environment.Exit(1);

                return;
            }

            if (!await _defaultUserSetupService.RunAsync(cancellationToken))
            {
                _logger.LogError("Something was wrong when run {Service}", nameof(DefaultUserSetupService));

                Environment.Exit(1);

                return;
            }
        }
        catch (Exception exception)
        {
            _logger.LogExceptionError(exception, nameof(IdentityServiceSetupInitializeRequired));
        }
    }

    
}
