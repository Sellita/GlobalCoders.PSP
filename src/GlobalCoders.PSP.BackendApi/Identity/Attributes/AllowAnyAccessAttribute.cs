using Microsoft.AspNetCore.Authorization;

namespace GlobalCoders.PSP.BackendApi.Identity.Attributes;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public sealed class AllowAnyAccessAttribute : AuthorizeAttribute;
