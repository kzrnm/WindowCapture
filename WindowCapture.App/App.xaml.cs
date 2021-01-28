using Prism.Ioc;
using System.Windows;
using CapApp.Views;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using Prism.Modularity;
using Kzrnm.WindowCapture;
using Kzrnm.WindowCapture.Images;

namespace CapApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell() => Container.Resolve<MainWindow>();
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ImageProvider>();
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<WindowCaptureModule<MainControl>>();
        }

        public static string ExePath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new InvalidOperationException();
        public App()
        {
#if !DEBUG
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
#endif
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is not Exception exception) return;

            Debug.WriteLine(exception);
            Debug.WriteLine(exception.StackTrace);
            var logdir = Path.Combine(ExePath, "Log");
            Directory.CreateDirectory(logdir);

            var filePath = Path.Combine(logdir, $"{Assembly.GetExecutingAssembly().GetName().Name}-{DateTime.Now:yyyyMMddHHmmss}.log");
            using var fw = new StreamWriter(filePath, true, new UTF8Encoding(false));
            using var sr = new StringReader(exception.StackTrace ?? "");

            fw.WriteLine("Message： " + exception.Message);
            while (sr.ReadLine() is string line)
                fw.WriteLine(line);
            fw.Flush();

            MessageBox.Show("Error", "UnhandledException", MessageBoxButton.OK, MessageBoxImage.Stop);
            Environment.Exit(1);
        }
    }
}
