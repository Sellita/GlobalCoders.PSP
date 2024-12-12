using GlobalCoders.PSP.BackendApi.Base.Configuration;

namespace GlobalCoders.PSP.BackendApi.Base.Extensions;

public static class CorsExtension
{
    public static List<string> Origins { get; private set; } = new();

    public static void RegisterCors(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CorsConfiguration>(configuration.GetSection(CorsConfiguration.SectionName));

        Origins = configuration.GetSection(CorsConfiguration.SectionName).Get<CorsConfiguration>()?.Origins
                  ?? new List<string>();

        if (!Origins.Any())
        {
            return;
        }

        services.AddCors(
            options =>
            {
                options.AddDefaultPolicy(
                    policyBuilder =>
                    {
                        policyBuilder
                            .WithOrigins(Origins.ToArray())
                            .SetIsOriginAllowedToAllowWildcardSubdomains()
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                    });
            });
    }
}
