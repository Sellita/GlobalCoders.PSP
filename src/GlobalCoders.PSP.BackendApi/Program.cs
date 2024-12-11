using GlobalCoders.PSP.BackendApi.Base.Configuration;
using GlobalCoders.PSP.BackendApi.Base.Extensions;
using GlobalCoders.PSP.BackendApi.Base.Filters;
using GlobalCoders.PSP.BackendApi.Base.Services;
using GlobalCoders.PSP.BackendApi.Data;
using GlobalCoders.PSP.BackendApi.Data.Initialization;
using GlobalCoders.PSP.BackendApi.DiscountManagment.Extensions;
using GlobalCoders.PSP.BackendApi.EmployeeManagment.Extensions;
using GlobalCoders.PSP.BackendApi.Identity.Extensions;
using GlobalCoders.PSP.BackendApi.OrganizationManagment.Extensions;
using GlobalCoders.PSP.BackendApi.DiscountManagment.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;
using Swashbuckle.AspNetCore.Filters;

const string BearerScheme = "Bearer";

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(
    (hostBuilder, loggerConfiguration) => loggerConfiguration.ReadFrom.Configuration(hostBuilder.Configuration));


RegisterSwagger(builder);

builder.Services.AddControllers()
    .AddNewtonsoftJson(
        options =>
        {
            options.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();

            options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Ignore;
        });

builder.Services.AddDbContextFactory<BaseDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("YourDatabaseConnection")));


RegisterServices(builder.Services, builder.Configuration, builder.Environment.IsDevelopment());

var app = builder.Build();

await app.InitializeRequiredServicesAsync();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.DisplayRequestDuration();
    });
}

app.UseSerilogRequestLogging();
app.MapControllers()
    .WithOpenApi();
app.RegisterEmployees();



await app.RunAsync();

void RegisterServices(IServiceCollection services, ConfigurationManager configuration, bool isDevelopment)
{
    RegisterDataBaseServices(services, configuration);
    
    services.RegisterIdentityServices(configuration, isDevelopment);
    services.RegisterEmployeeServices();
    services.RegisterOrganizationServices();
    services.RegisterDiscountServices();

    
    
}

void RegisterDataBaseServices(IServiceCollection services, ConfigurationManager configuration)
{
    services.Configure<DbSettings>(configuration.GetSection(DbSettings.SectionName));

    var connectionString = configuration.GetSection(DbSettings.SectionName).Get<DbSettings>()?.ConnectionString;

    if (string.IsNullOrWhiteSpace(connectionString))
    {
        throw new InvalidOperationException($"{nameof(DbSettings.ConnectionString)} is not found");
    }

    services.AddDbContextFactory<BackendContext>(options=> options.UseNpgsql(connectionString), ServiceLifetime.Scoped);
    
    services.AddScoped<IInitializeRequired, DbMigrationService>();
}

void RegisterSwagger(WebApplicationBuilder webApplicationBuilder)
{
    webApplicationBuilder.Services.AddEndpointsApiExplorer();
    webApplicationBuilder.Services.AddSwaggerGen(
        options =>
        {
            options.CustomSchemaIds(type => type.ToString());

            options.AddSecurityDefinition(name: BearerScheme, new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description = "Enter the Bearer Authorization string as following: `Bearer Generated-Token`",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = BearerScheme
            });
            
            options.OperationFilter<AuthResponsesOperationFilter>();
        });
}