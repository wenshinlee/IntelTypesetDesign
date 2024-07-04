using System;
using Autofac;
using IntelTypesetDesign.Models;
using IntelTypesetDesign.Modules.FileSystem.DotNet;
using IntelTypesetDesign.Modules.Log.Trace;
using IntelTypesetDesign.Modules.ServiceProvider;

namespace IntelTypesetDesign;

public class AppModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        // Container
        ILifetimeScope lifetimeScope = null!;
        builder.Register(_ => lifetimeScope).AsSelf().SingleInstance();
        builder.RegisterBuildCallback(x => lifetimeScope = x);
        
        // ServiceProvider
        builder.RegisterType<AutofacServiceProvider>().As<IServiceProvider>().InstancePerLifetimeScope();
        
        // Dependencies
        builder.RegisterType<TraceLog>().As<ILog>().SingleInstance();
        builder.RegisterType<DotNetFileSystem>().As<IFileSystem>().InstancePerLifetimeScope();
    }
}