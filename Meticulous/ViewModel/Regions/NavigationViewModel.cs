using Meticulous.Core;
using Meticulous.Infrastructure.Services.Navigation;
using Meticulous.Modules.Alerts.ViewModel;
using Meticulous.Modules.Dashboard.ViewModel;
using Meticulous.Modules.Processes.ViewModel;
using Meticulous.Modules.Reports.ViewModel;
using Meticulous.Modules.Settings.ViewModel;
using Meticulous.Shell.RelayCommand;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Meticulous.ViewModel.Regions
{
    public class NavigationViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public ObservableCollection<NavigationItem> NavigationItems { get; }

        public ICommand DashboardCommand { get; }
        public ICommand ProcessesCommand { get; }
        public ICommand AlertsCommand { get; }
        public ICommand ReportsCommand { get; }
        public ICommand SettingsCommand { get; }

        public NavigationViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            NavigationItems = new ObservableCollection<NavigationItem>
            {
                new NavigationItem("Dashboard", typeof(DashboardViewModel)),
                new NavigationItem("Processes", typeof(ProcessesViewModel)),
                new NavigationItem("Alerts", typeof(AlertsViewModel)),
                new NavigationItem("Reports", typeof(ReportsViewModel)),
                new NavigationItem("Settings", typeof(SettingsViewModel))
            };

            DashboardCommand = new RelayCommand(_ => _navigationService.NavigateTo<DashboardViewModel>());
            ProcessesCommand = new RelayCommand(_ => _navigationService.NavigateTo<ProcessesViewModel>());
            AlertsCommand = new RelayCommand(_ => _navigationService.NavigateTo<AlertsViewModel>());
            ReportsCommand = new RelayCommand(_ => _navigationService.NavigateTo<ReportsViewModel>());
            SettingsCommand = new RelayCommand(_ => _navigationService.NavigateTo<SettingsViewModel>());
        }
    }
}
