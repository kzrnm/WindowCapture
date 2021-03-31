using Kzrnm.WindowCapture.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using CapApp.ViewModels;

namespace CapApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public static string ExePath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new InvalidOperationException();
        public App()
        {
            Ioc.Default.ConfigureServices(ServiceBuilder.Default
                .AddSingleton<MainWindowViewModel>()
                .BuildServiceProvider());
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
