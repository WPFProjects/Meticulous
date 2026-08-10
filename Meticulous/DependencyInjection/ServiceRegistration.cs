using Meticulous.Infrastructure.Services.Navigation;
using Meticulous.Modules.Alerts.ViewModel;
using Meticulous.Modules.Dashboard.ViewModel;
using Meticulous.Modules.Notification.ViewModel;
using Meticulous.Modules.Processes.ViewModel;
using Meticulous.Modules.Reports.ViewModel;
using Meticulous.Modules.Settings.ViewModel;
using Meticulous.ViewModel;
using Meticulous.ViewModel.Regions;
using Microsoft.Extensions.DependencyInjection;

namespace Meticulous.Shell.DependencyInjection
{
    public static class ServiceRegistration
    {
        public static ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Register dependency services first, then dependent services
            services.AddSingleton<DashboardViewModel>();
            services.AddSingleton<AlertsViewModel>();
            services.AddSingleton<ProcessesViewModel>();
            services.AddSingleton<ReportsViewModel>();
            services.AddSingleton<SettingsViewModel>();
            services.AddSingleton<NotificaitonViewModel>();

            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<NavigationViewModel>();
            services.AddSingleton<ToolbarViewModel>();
            services.AddSingleton<ContentRegionViewModel>();
            services.AddSingleton<HeaderViewModel>();
            services.AddSingleton<StatusBarViewModel>();

            // Register MainWindowViewModel last (after all its dependencies)
            services.AddSingleton<MainWindowViewModel>();

            return services.BuildServiceProvider();
        }
    }
}
