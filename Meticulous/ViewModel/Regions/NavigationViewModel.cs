using Meticulous.Model;
using Meticulous.RelayCommand;
using Meticulous.Services;
using System.ComponentModel;
using System.Windows.Input;

namespace Meticulous.ViewModel.Regions
{
    public class NavigationViewModel : INotifyPropertyChanged
    {
        private readonly NavigationService _navigationService;
        public NavigationViewModel(NavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private ICommand _dashboardCommand;
        public ICommand DashboardCommand
        {
            get
            {
                if (_dashboardCommand == null)
                {
                    _dashboardCommand = new RelayCommands(IsCanExecute, ExecuteDashboardCommand);
                }
                return _dashboardCommand; // <- also fix: return the command
            }
        }

        private void ExecuteDashboardCommand(object parameter)
        {
            // Implement the logic to navigate to the dashboard view
            // For example, you can use a navigation service or set the current view model
            _navigationService.NavigateTo(NavigationItemType.Dashboard);
        }


        private ICommand _processesCommand;
        public ICommand ProcessesCommand
        {
            get
            {
                if (_processesCommand == null)
                {
                    _processesCommand = new RelayCommands(IsCanExecute, ExecuteProcessesCommand);
                }
                return _processesCommand; // <- also fix: return the command
            }
        }

        private void ExecuteProcessesCommand(object parameter)
        {
            // Implement the logic to navigate to the processes view
            // For example, you can use a navigation service or set the current view model
            _navigationService.NavigateTo(NavigationItemType.Processes);
        }

        private ICommand _alertsCommand;
        public ICommand AlertsCommand
        {
            get
            {
                if (_alertsCommand == null)
                {
                    _alertsCommand = new RelayCommands(IsCanExecute, ExecuteAlertsCommand);
                }
                return _alertsCommand; // <- also fix: return the command
            }
        }

        private void ExecuteAlertsCommand(object parameter)
        {
            // Implement the logic to navigate to the alerts view
            // For example, you can use a navigation service or set the current view model
            _navigationService.NavigateTo(NavigationItemType.Alerts);
        }


        private ICommand _ReportsCommand;
        public ICommand ReportsCommand
        {
            get
            {
                if (_ReportsCommand == null)
                {
                    _ReportsCommand = new RelayCommands(IsCanExecute, ExecuteReportsCommand);
                }
                return _ReportsCommand; // <- also fix: return the command
            }
        }

        private void ExecuteReportsCommand(object parameter)
        {
            // Implement the logic to navigate to the reports view
            // For example, you can use a navigation service or set the current view model
            _navigationService.NavigateTo(NavigationItemType.Reports);
        }


        private ICommand _SettingsCommand;
        public ICommand SettingsCommand
        {
            get
            {
                if (_SettingsCommand == null)
                {
                    _SettingsCommand = new RelayCommands(IsCanExecute, ExecuteSettingsCommand);
                }
                return _SettingsCommand; // <- also fix: return the command
            }
        }

        private void ExecuteSettingsCommand(object parameter)
        {
            // Implement the logic to navigate to the settings view
            // For example, you can use a navigation service or set the current view model
            _navigationService.NavigateTo(NavigationItemType.Settings);
        }

        private bool IsCanExecute(object parameter)
        {
            return true;
        }
    }
}
