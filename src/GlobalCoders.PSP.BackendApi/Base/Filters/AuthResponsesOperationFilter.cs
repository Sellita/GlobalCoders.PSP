using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace GlobalCoders.PSP.BackendApi.Base.Filters;


public class AuthResponsesOperationFilter: IOperationFilter {
    const string BearerScheme = "Bearer";

    
    public void Apply(OpenApiOperation operation, OperationFilterContext context) {
        if (!context.MethodInfo.GetCustomAttributes(true).Any(x => x is AllowAnonymousAttribute) &&
            !context.MethodInfo.DeclaringType.GetCustomAttributes(true).Any(x => x is AllowAnonymousAttribute)) {
            operation.Security = new List < OpenApiSecurityRequirement > {
                new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Name = BearerScheme,
                            In = ParameterLocation.Header,
                            Reference = new OpenApiReference
                            {
                                Id = BearerScheme,
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                }
                
            };
        }

    }
}