using System;
using System.Collections.ObjectModel;
using Autofac;
using Dock.Model.Controls;
using Dock.Model.Core;
using Dock.Model.Mvvm.Controls;
using IntelTypesetDesign.Configuration.Windows;
using IntelTypesetDesign.Docking;
using IntelTypesetDesign.Json;
using IntelTypesetDesign.Models;
using IntelTypesetDesign.ViewModels;
using IntelTypesetDesign.ViewModels.Editor;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace IntelTypesetDesign;

public class AppState : IDisposable
{
    private IContainer? Container { get; }

    public IServiceProvider? ServiceProvider { get; }

    private ILog? Log { get; }

    private IFileSystem? FileSystem { get; }

    private string BaseDirectory { get; }

    private string LayoutPath { get; }

    private string WindowConfigurationPath { get; }

    private string LogPath { get; }

    public WindowConfiguration? WindowConfiguration { get; set; }
    
    public ProjectEditorViewModel? Editor { get; }
    
    private static readonly JsonSerializerSettings JsonSettings =
        new()
        {
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Objects,
            PreserveReferencesHandling = PreserveReferencesHandling.Objects,
            ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
            ContractResolver = new ListContractResolver(typeof(ObservableCollection<>)),
            NullValueHandling = NullValueHandling.Ignore,
            Converters = { new KeyValuePairConverter() }
        };

    public AppState()
    {
        // Init
        LogPath = "IntelTypesetDesign.log";
        LayoutPath = "IntelTypesetDesign.layout";
        WindowConfigurationPath = "IntelTypesetDesign.window";

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
        
        // editor
        Editor = ServiceProvider.GetService<ProjectEditorViewModel>();
        InitializeEditor();
        
        // configuration
        WindowConfiguration = LoadWindowSettings();
    }

    private void InitializeEditor()
    {
        if (Editor is null)
            return;

        Editor.DockFactory = new DockFactory(Editor);
        
        RootDock? rootDock = default(RootDock);;
        if (FileSystem is not null)
        {
            var rootDockPath = System.IO.Path.Combine(BaseDirectory, LayoutPath);
            if (FileSystem.Exists(rootDockPath))
            {
                var jsonRootDock = FileSystem?.ReadUtf8Text(rootDockPath);
                if (!string.IsNullOrEmpty(jsonRootDock))
                {
                    rootDock = JsonConvert.DeserializeObject<RootDock>(jsonRootDock, JsonSettings);
                    if (rootDock is not null)
                    {
                        LoadLayout(Editor, rootDock);
                    }
                }
            }
        }
        
        if (rootDock is null)
        {
            CreateLayout(Editor);
        }
    }
    
    private static void LoadLayout(ProjectEditorViewModel editor, IRootDock layout)
    {
        if (editor.DockFactory is not IFactory dockFactory) return;
        editor.RootDock = layout;

        if (editor.RootDock is not IDock dock) return;
        dockFactory.InitLayout(dock);
        
        // editor.NavigateTo = id => dock.Navigate.Execute(id);
        // dock.Navigate.Execute("Dashboard");
        
    }
    private static void CreateLayout(ProjectEditorViewModel editor)
    {
        if (editor.DockFactory is not IFactory dockFactory) return;
        editor.RootDock = dockFactory.CreateLayout();

        if (editor.RootDock is not IDock dock) return;
        dockFactory.InitLayout(dock);

        // editor.NavigateTo = id => dock.Navigate.Execute(id);
        //
        // dock.Navigate.Execute("Dashboard");
        //
        // var pages = dockFactory.GetDockable<IDocumentDock>("Pages");
        // pages?.CreateDocument?.Execute(null);
    }
    private WindowConfiguration? LoadWindowSettings()
    {
        if (FileSystem is null)
        {
            return null;
        }

        var windowSettings = default(WindowConfiguration);
        var windowSettingsPath = System.IO.Path.Combine(BaseDirectory, WindowConfigurationPath);

        if (!FileSystem.Exists(windowSettingsPath))
            return windowSettings;
        
        var jsonWindowSettings = FileSystem.ReadUtf8Text(windowSettingsPath);
        if (!string.IsNullOrEmpty(jsonWindowSettings))
        {
            windowSettings = JsonConvert.DeserializeObject<WindowConfiguration>(
                jsonWindowSettings,
                JsonSettings
            );
        }

        return windowSettings;
    }

    public void Save() 
    {
        if (Editor is null)
        {
            return;
        }

        var jsonWindowSettings = JsonConvert.SerializeObject(WindowConfiguration, JsonSettings);
        if (!string.IsNullOrEmpty(jsonWindowSettings))
        {
            FileSystem?.WriteUtf8Text(WindowConfigurationPath, jsonWindowSettings);
        }

        // var jsonRootDock = JsonConvert.SerializeObject(Editor.RootDock, JsonSettings);
        // if (!string.IsNullOrEmpty(jsonRootDock))
        // {
        //     FileSystem?.WriteUtf8Text(LayoutPath, jsonRootDock);
        // } 
    }

    public void Dispose()
    {
        Container?.Dispose();
        Log?.Dispose();
    }
}
