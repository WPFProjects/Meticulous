using Meticulous.Infrastructure.Services.Navigation;
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
