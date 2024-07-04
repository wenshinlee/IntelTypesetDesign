using System;
using Autofac;
using IntelTypesetDesign.Models;
using IntelTypesetDesign.ViewModels;

namespace IntelTypesetDesign;

public class AppState: IDisposable
{
    private IContainer? Container { get; }

    private IServiceProvider? ServiceProvider { get; }

    private ILog? Log { get; }
    
    private IFileSystem? FileSystem { get; }
    
    public string BaseDirectory { get; }
    public string LogPath { get; }
    
    public AppState()
    {
        // Init
        LogPath = "IntelTypesetDesign.log";
        
        
        // Container
        var builder = new ContainerBuilder();
        builder.RegisterModule<AppModule>();
        Container = builder.Build();
        
        // ServiceProvider
        ServiceProvider = Container.Resolve<IServiceProvider>();
        Log = ServiceProvider.GetService<ILog>();
        FileSystem = ServiceProvider.GetService<IFileSystem>();


        BaseDirectory = FileSystem?.GetBaseDirectory() ?? "";
        Log?.Initialize(System.IO.Path.Combine(BaseDirectory, LogPath));
    }
    
    public void Dispose()
    {
        Container?.Dispose();
        Log?.Dispose();
    }
}