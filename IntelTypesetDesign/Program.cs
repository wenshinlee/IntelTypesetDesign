using System;
using System.CommandLine;
using System.CommandLine.NamingConventionBinder;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Threading;

namespace IntelTypesetDesign;

internal static class Program
{
    /// <summary>
    /// APP入口函数
    /// </summary>
    /// <param name="args"></param>
    [STAThread]
    public static void Main(string[] args)
    {
        var settings = CreateRootCommand(args);
        if (settings is not null)
        {
            StartAvaloniaApp(settings, args);
        }
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    private static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder.Configure<App>().UsePlatformDetect().WithInterFont().LogToTrace();
    }

    /// <summary>
    /// 命令行参数
    /// </summary>
    /// <returns></returns>
    private static Settings? CreateRootCommand(string[] args)
    {
        // 定义根命令
        var rootCommand = new RootCommand()
        {
            Description = "A multi-platform intelligent typesetting design project."
        };

        // 定义可选参数
        rootCommand.AddOption(
            new Option<string>(name: "--theme", description: "Set application theme.")
        );

        // 获取应用设置
        Settings? rootSetting = null;
        rootCommand.Handler = CommandHandler.Create(
            (Settings settings) =>
            {
                rootSetting = settings;
            }
        );

        // 解析命令参数
        rootCommand.Invoke(args);

        return rootSetting;
    }

    /// <summary>
    /// 启动APP
    /// </summary>
    /// <param name="settings"></param>
    /// <param name="args"></param>
    private static void StartAvaloniaApp(Settings settings, string[] args)
    {
        var builder = BuildAvaloniaApp();
        try
        {
            if (settings.Theme != null)
            {
                App.DefaultTheme = settings.Theme;
            }

            builder
                .AfterSetup(async _ => await ProcessSettings(settings))
                .StartWithClassicDesktopLifetime(args);
        }
        catch (Exception ex)
        {
            Log(ex);
        }
    }

    /// <summary>
    /// 加载APP设置
    /// </summary>
    /// <param name="settings"></param>
    private static async Task ProcessSettings(Settings settings)
    {
        await Dispatcher.UIThread.InvokeAsync(() => { });
    }

    /// <summary>
    /// 控制台打印异常
    /// </summary>
    /// <param name="ex"></param>
    private static void Log(Exception ex)
    {
        Console.WriteLine(ex.Message);
        Console.WriteLine(ex.StackTrace);

        if (ex.InnerException is not null)
        {
            Log(ex.InnerException);
        }
    }
}
