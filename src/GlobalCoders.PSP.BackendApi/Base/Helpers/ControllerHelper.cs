using System.Reflection;
using GlobalCoders.PSP.BackendApi.Identity.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace GlobalCoders.PSP.BackendApi.Base.Helpers;

public static class ControllerHelper
{
    private const string MethodsAsyncSuffix = "async";

    public static string GetUrl<T>(string endpoint) where T : ControllerBase
    {
        var controllerName = typeof(T).Name.Replace(nameof(Microsoft.AspNetCore.Mvc.Controller), string.Empty).ToLower();

        endpoint = endpoint.ToLower().Replace(MethodsAsyncSuffix.ToLower(), string.Empty);

        return string.Join('/', string.Empty, controllerName, endpoint);
    }

    public static IReadOnlyDictionary<string, bool> GetActionRequiredPermissions<TController>()
    {
        var requiredPermissionDictionary = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);

        var assembly = Assembly.GetExecutingAssembly();

        var controllers = assembly
            .GetTypes()
            .Where(type => typeof(TController).IsAssignableFrom(type) && !type.IsAbstract)
            .ToList();

        foreach (var controller in controllers)
        {
            var methods = controller
                .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(m => m.IsPublic && !m.IsDefined(typeof(NonActionAttribute)))
                .ToList();

            foreach (var action in methods)
            {
                var controllerAttributes = controller
                    .GetCustomAttribute<AllowAnyAccessAttribute>();

                var methodAttributes = action
                    .GetCustomAttribute<AllowAnyAccessAttribute>();

                var allowAnyAccessAttribute = methodAttributes ?? controllerAttributes;

                var actionScope = GetControllerActionScope(controller, action).ToLower();

                if (requiredPermissionDictionary.ContainsKey(actionScope))
                {
                    continue;
                }

                requiredPermissionDictionary.Add(actionScope, allowAnyAccessAttribute == null);
            }
        }

        return requiredPermissionDictionary;
    }

    public static string GetControllerCustomScope<TModel>(MemberInfo controller, string propertyName)
    {
        const string prefix = "canSet";

        var controllerName = GetControllerName(controller);

        var modelName = typeof(TModel).Name;

        //CanSet

        return $"{controllerName}:{prefix}{modelName}{propertyName}".ToLower();
    }

    private static string GetControllerActionScope(MemberInfo controller, MemberInfo action)
    {
        return $"{GetControllerName(controller)}:{GetActionName(action)}".ToLower();
    }

    private static string GetControllerName(MemberInfo controller)
    {
        var controllerName = controller.Name;

        var controllerNameSuffixIndex =
            controllerName.LastIndexOf(nameof(Microsoft.AspNetCore.Mvc.Controller), StringComparison.OrdinalIgnoreCase);

        return controllerNameSuffixIndex != -1
            ? controllerName[..controllerNameSuffixIndex]
            : controllerName;
    }

    private static string GetActionName(MemberInfo action)
    {
        const string asyncSuffix = "Async";

        var actionName = action.Name;

        var actionNameSuffixIndex =
            actionName.LastIndexOf(asyncSuffix, StringComparison.OrdinalIgnoreCase);

        return actionNameSuffixIndex != -1
            ? actionName[..actionNameSuffixIndex]
            : actionName;
    }
}
