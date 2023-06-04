﻿using Autofac;
using System.Reflection;

namespace BLL.Startup;
public class Bootstrapper
{
    public static void Bootstrap(ContainerBuilder builder)
    {
        DAL.Startup.Bootstrapper.Bootstrap(builder);
        RegisterServices(builder);
    }

    private static void RegisterServices(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(Assembly.Load("BLL"))
            .Where(x => x.Name.EndsWith("Service"))
            .AsImplementedInterfaces()
            .InstancePerLifetimeScope();
    }
}
