using Domain;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PomodoroTimerDesktop.Abstractions;
using PomodoroTimerDesktop.Models;
using System.Windows;

namespace PomodoroTimerDesktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = Host
                .CreateDefaultBuilder()
                .ConfigureServices((context, services) => ConfigureServices(services))
                .Build();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<TimerMainWindow>();
            services.AddSingleton<Settings>();

            services.AddSingleton<PomodoroTimer>();
            services.AddSingleton<TimerConfiguration>();
            services.AddSingleton<IFileSerializer, FileSerializer>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();

            var mainWindow = _host.Services.GetRequiredService<TimerMainWindow>();
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            using (_host)
            {
                _host.StopAsync();
            }

            base.OnExit(e);
        }
    }
}
