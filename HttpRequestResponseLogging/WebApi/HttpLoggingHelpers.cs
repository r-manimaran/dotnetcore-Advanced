using System.Net.Http.Headers;

namespace WebApi;

public static class HttpLoggingHelpers
{
    public static IEnumerable<KeyValuePair<string, IEnumerable<string>>> ExceptSensitiveHeaders(
        this HttpRequestHeaders headers)
    {
        return headers.Where(x => !x.Key.Contains("Authorization", StringComparison.OrdinalIgnoreCase));
    }
    public static IEnumerable<KeyValuePair<string, IEnumerable<string>>> ExceptSensitiveHeaders(
        this HttpContentHeaders headers)
    {
        return headers.Where(x => !x.Key.Contains("secret-token",
            StringComparison.OrdinalIgnoreCase));
    }
    private static readonly List<string> NotAllowedRequestUrls = new()
    {
        "/api/users/login",
        "/api/users/refresh"
    };

    private static readonly List<string> NotAllowedResponseUrls = new()
    {
        "/api/users/login",
        "/api/users/refresh"
    };

    public static bool RequestCanBeLogged(string? url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return false;
        }

        return !NotAllowedRequestUrls.Any(x => x.Contains(url, StringComparison.OrdinalIgnoreCase));
    }

    public static bool ResponseCanBeLogged(string? url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return false;
        }

        return !NotAllowedResponseUrls.Any(x => x.Contains(url, StringComparison.OrdinalIgnoreCase));
    }
}
