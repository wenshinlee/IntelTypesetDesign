using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using IntelTypesetDesign.Configuration.Windows;
using IntelTypesetDesign.ViewModels;
using IntelTypesetDesign.ViewModels.Editor;

namespace IntelTypesetDesign;

public partial class App : Application
{
    public static string DefaultTheme { get; set; }

    static App()
    {
        DefaultTheme = "FluentDark";
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
        {
            InitializationClassicDesktopStyle(desktopLifetime, out var editor);
            DataContext = editor;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static void InitializationClassicDesktopStyle(
        IClassicDesktopStyleApplicationLifetime desktopLifetime,
        out ProjectEditorViewModel? editor
    )
    {
        var appState = new AppState();

        var mainWindow = appState.ServiceProvider.GetService<Window>();
        if (mainWindow is null)
        {
            editor = null;
            return;
        }
        
        if (appState.WindowConfiguration is not null)
        {
            WindowConfigurationFactory.Load(mainWindow, appState.WindowConfiguration);
        }
        
        mainWindow.DataContext = appState.Editor;

        mainWindow.Closing += (_, _) =>
        {
            appState.WindowConfiguration = WindowConfigurationFactory.Save(mainWindow);
            appState.Save();
        };

        desktopLifetime.MainWindow = mainWindow;
        desktopLifetime.Exit += (_, _) => appState.Dispose();

        editor = appState.Editor;
    }
}
