using GlobalCoders.PSP.BackendApi.Base.Configuration;
using GlobalCoders.PSP.BackendApi.Base.Extensions;
using GlobalCoders.PSP.BackendApi.Base.Filters;
using GlobalCoders.PSP.BackendApi.Base.Services;
using GlobalCoders.PSP.BackendApi.Data;
using GlobalCoders.PSP.BackendApi.Data.Initialization;
using GlobalCoders.PSP.BackendApi.DiscountManagement.Extensions;
using GlobalCoders.PSP.BackendApi.EmployeeManagment.Extensions;
using GlobalCoders.PSP.BackendApi.Identity.Extensions;
using GlobalCoders.PSP.BackendApi.InventoryManagement.Extensions;
using GlobalCoders.PSP.BackendApi.OrdersManagement.Extensions;
using GlobalCoders.PSP.BackendApi.OrganizationManagment.Extensions;
using GlobalCoders.PSP.BackendApi.PaymentsService.Extensions;
using GlobalCoders.PSP.BackendApi.ProductsManagement.Extensions;
using GlobalCoders.PSP.BackendApi.ReservationManagment.Extensions;
using GlobalCoders.PSP.BackendApi.ServicesManagement.Extensions;
using GlobalCoders.PSP.BackendApi.SurchargeManagement.Extensions;
using GlobalCoders.PSP.BackendApi.TaxManagement.Extensions;
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

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
RegisterSwagger(builder);

builder.Services.AddControllers()
    .AddNewtonsoftJson(
        options =>
        {
            options.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();

            options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
        });

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

app.UseCors();
app.MapControllers()
    .WithOpenApi();
app.RegisterEmployees();

app.UseSerilogRequestLogging();


await app.RunAsync();

void RegisterServices(IServiceCollection services, ConfigurationManager configuration, bool isDevelopment)
{
    services.RegisterCors(configuration);
    
    RegisterDataBaseServices(services, configuration);
    
    services.RegisterIdentityServices(configuration, isDevelopment);
    services.RegisterEmployeeServices();
    services.RegisterOrganizationServices();
    services.RegisterSurChargeManagementServices();
    services.RegisterProductsManagmentServices();
    services.RegisterInventoryServices();
    services.RegisterOrdersServices();
    services.RegisterTaxManagementServices();
    services.RegisterDiscountManagementServices();
    services.RegisterServicesManagementServices();
    services.RegisterReservationManagementServices();
    services.RegisterPaymentManagementServices(configuration);
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