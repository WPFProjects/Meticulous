using Meticulous.Shell.DependencyInjection;
using Meticulous.Shell.View;
using Meticulous.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace Meticulous
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var serviceProvider = ServiceRegistration.ConfigureServices();

            var mainWindow = new MainWindow
            {
                DataContext = serviceProvider.GetRequiredService<MainWindowViewModel>()
            };

            mainWindow.Show();
        }
    }
}
