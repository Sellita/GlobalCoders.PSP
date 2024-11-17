using System.Text;

namespace GlobalCoders.PSP.BackendApi.Base.Helpers;

public static class UrlHelper
{
    private const char Slash = '/';

    public static string BuildUrl(string baseUrl, string? path = null, RouteValueDictionary? routeValues = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(baseUrl);

        var urlBuilder = new StringBuilder(baseUrl.TrimEnd(Slash));

        if (!string.IsNullOrWhiteSpace(path))
        {
            urlBuilder.Append(Slash);
            urlBuilder.Append(path.TrimStart(Slash));
        }

        if (routeValues is not {Count: > 0})
        {
            return urlBuilder.ToString();
        }

        var queryString = string.Join("&", routeValues.Select(kvp => $"{kvp.Key}={kvp.Value}"));

        urlBuilder.Append('?');

        urlBuilder.Append(queryString);

        return urlBuilder.ToString();
    }

    public static string BuildUrl(HttpRequest request, params string[] paths)
    {
        var urlBuilder = new StringBuilder($"{request.Scheme}://{request.Host}{request.PathBase}".TrimEnd(Slash));

        AppendPaths(urlBuilder, paths);

        return urlBuilder.ToString();
    }

    public static string BuildUrl(string url, params string[] paths)
    {
        var urlBuilder = new StringBuilder(url.TrimEnd(Slash));

        AppendPaths(urlBuilder, paths);

        return urlBuilder.ToString();
    }

    public static string BuildUrl(string url, IEnumerable<string> paths, RouteValueDictionary routeValues)
    {
        var urlBuilder = new StringBuilder(url.TrimEnd(Slash));

        AppendPaths(urlBuilder, paths);

        if (routeValues is not {Count: > 0})
        {
            return urlBuilder.ToString();
        }

        var queryString = string.Join("&", routeValues.Select(kvp => $"{kvp.Key}={kvp.Value}"));

        urlBuilder.Append('?');

        urlBuilder.Append(queryString);

        return urlBuilder.ToString();
    }

    private static void AppendPaths(StringBuilder urlBuilder, IEnumerable<string> paths)
    {
        foreach (var path in paths)
        {
            var normalizedPath = NormalizePath(path);

            if (string.IsNullOrWhiteSpace(normalizedPath))
            {
                continue;
            }

            urlBuilder.Append(Slash);
            urlBuilder.Append(normalizedPath.TrimStart(Slash).TrimEnd(Slash));
        }
    }

    private static string NormalizePath(string path)
    {
        var segments = path.Split('/', StringSplitOptions.RemoveEmptyEntries);
        return string.Join("/", segments);
    }
}
