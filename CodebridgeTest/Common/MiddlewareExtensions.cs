using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Common.Configs;
using Common.IoC;
using DAL.DbContexts;
using System.Text.Json;

namespace WebAPI.Common;
public static class MiddlewareExtensions
{
    public static WebApplicationBuilder DefaultConfiguration(this WebApplicationBuilder builder)
    {
        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
        {
            containerBuilder.RegisterBuildCallback(ctx => IoC.Container = ctx.Resolve<ILifetimeScope>());
            BLL.Startup.Bootstrapper.Bootstrap(containerBuilder);
        });
        builder.Host.ConfigureAppConfiguration(config =>
        {
            if (Path.GetFullPath(@"../").Split('\\').Where(x => !string.IsNullOrEmpty(x)).Last() == "CodebridgeTest")
                config.AddJsonFile(@$"{Path.GetFullPath(@"../")}/config.json");
            else
                config.AddJsonFile(@$"{Path.GetFullPath(@"../../../../")}/config-tests.json");
        });

        builder.Services.AddScoped<DbContextBase>();

        var connectionString = builder.Configuration.GetValue<string>("ConnectionString");
        builder.Services.AddSingleton(new ConnectionStringModel(connectionString));

        var requestSetting = builder.Configuration.GetSection("RequestSetting").Get<RequestSetting>();
        builder.Services.AddSingleton(requestSetting);

        builder.Services.AddControllers();

        #region Init Mapper Profiles

        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddMaps(new[] {
                "BLL.Models"
            });
        });

        var mapper = mapperConfig.CreateMapper();
        builder.Services.AddSingleton(mapper);

        #endregion

        return builder;
    }

    public static IApplicationBuilder UseDefaultErrorHandler(this IApplicationBuilder appBuilder)
    {
        return appBuilder.Use(DefaultHandleError);
    }

    #region Helpers

    private static async Task DefaultHandleError(HttpContext httpContext, Func<Task> next)
    {
        try
        {
            await next();
        }
        catch (Exception ex)
        {
            string message = null;

            switch (ex)
            {
                case ArgumentException _:
                    httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    message = ex.Message;
                    break;
                default:
                    message = ex.Message;
                    break;
            }

            await httpContext.Response.WriteAsync(JsonSerializer.Serialize(message, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                IgnoreNullValues = true
            }));
        }
    }

    #endregion
}
