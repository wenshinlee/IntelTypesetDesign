using System;
using Autofac;
using Avalonia.Controls;
using IntelTypesetDesign.Models;
using IntelTypesetDesign.Modules.FileSystem.DotNet;
using IntelTypesetDesign.Modules.Log.Trace;
using IntelTypesetDesign.Modules.ServiceProvider;
using IntelTypesetDesign.ViewModels.Editor;
using IntelTypesetDesign.Views;

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
        
        // Views
        builder.RegisterType<MainWindow>().As<Window>().InstancePerLifetimeScope();
        
        // viewModel
        builder
            .RegisterType<ProjectEditorViewModel>()
            .As<ProjectEditorViewModel>()
            .InstancePerLifetimeScope();
    }
}