using GlobalCoders.PSP.BackendApi.Base.Services;
using GlobalCoders.PSP.BackendApi.Data;
using GlobalCoders.PSP.BackendApi.Email.Configuration;
using GlobalCoders.PSP.BackendApi.Email.Services;
using GlobalCoders.PSP.BackendApi.EmployeeManagment.Entities;
using GlobalCoders.PSP.BackendApi.Identity.Configuration;
using GlobalCoders.PSP.BackendApi.Identity.Handlers;
using GlobalCoders.PSP.BackendApi.Identity.Mediators;
using GlobalCoders.PSP.BackendApi.Identity.Services;
using GlobalCoders.PSP.BackendApi.Identity.Services.Initialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using IAuthorizationService = GlobalCoders.PSP.BackendApi.Identity.Services.IAuthorizationService;


namespace GlobalCoders.PSP.BackendApi.Identity.Extensions;

public static class IdentityServicesExtension
{
    public static void RegisterIdentityServices(this IServiceCollection services, ConfigurationManager configuration,
        bool isDevelopment)
    {
        services.Configure<IdentityConfiguration>(configuration.GetSection(IdentityConfiguration.SectionName));
        services.Configure<RolesConfiguration>(configuration.GetSection(RolesConfiguration.SectionName));

        var identityConfiguration = configuration.GetSection(IdentityConfiguration.SectionName).Get<IdentityConfiguration>();

        if (identityConfiguration == null || !identityConfiguration.IsValid())
        {
            throw new InvalidOperationException(nameof(identityConfiguration));
        }

        services.AddAuthentication(IdentityConstants.BearerScheme)
            .AddBearerToken(IdentityConstants.BearerScheme);

        services.AddAuthorization();

        if (isDevelopment)
        {
            services.AddAuthorization();
        }
        else
        {
            services.AddAuthorizationBuilder()
                .SetFallbackPolicy(new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build());
        }

        services.AddIdentityCore<EmployeeEntity>()
            .AddRoles<PermisionTemplateEntity>()
            .AddEntityFrameworkStores<BackendContext>()
            .AddApiEndpoints();

        services.AddTransient<IIdentityMediator, IdentityMediator>();

        services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();
        
        services.AddScoped<IAuthorizationService, AuthorizationService>();
        
        //todo move next to mail extensions
        services.Configure<MailConfiguration>(configuration.GetSection(MailConfiguration.SectionName));
        services.Configure<SmtpConfiguration>(configuration.GetSection(SmtpConfiguration.SectionName));
        services.AddScoped<IMailService, MailService>();
        services.AddScoped<IMailProvider, SmtpMailProvider>();
        
        services.AddScoped<IInitializeRequired, IdentityServiceSetupInitializeRequired>();
        services.AddScoped<IRoleSetupService, RoleSetupService>();
        services.AddScoped<IDefaultUserSetupService, DefaultUserSetupService>();
    }
    
    
    public static void RegisterIdentity(this WebApplication app)
    {

        //todo implement me
      
    }
}