using System.Reflection;

namespace WebAPI.Common;
public static class EndpointExtensions
{
    public static void MapPingEndpoint(this IEndpointRouteBuilder endpoints)
    {
        var assembly = Assembly.GetEntryAssembly();
        var title = assembly?.GetCustomAttribute<AssemblyTitleAttribute>()?.Title;
        var version = assembly?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
        var assenblyInfo = string.Format("{0} Version {1}", title, version);

        endpoints.MapGet("/ping", async context =>
        {
            await context.Response.WriteAsync(assenblyInfo);
        });
    }
}
