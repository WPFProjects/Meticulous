using Meticulous.Model;
using Meticulous.Modules.Alerts.View;
using Meticulous.Modules.Dashboard.View;
using Meticulous.Modules.Reports.View;
using Meticulous.Modules.Settings.View;
using Processes.View;
using System;

namespace Meticulous.Services
{
    public class NavigationService
    {
        public event Action<Object> CurrentNavigationViewChanged;

        public NavigationService()
        {

        }

        public void NavigateTo(NavigationItemType navigationItem)
        {
            Object CurrentView = null;
            switch (navigationItem)
            {
                case NavigationItemType.Dashboard:
                    // Navigate to dashboard
                    CurrentView = new DashboardUI();
                    break;
                case NavigationItemType.Processes:
                    // Navigate to processes
                    CurrentView = new ProcessScreenUI();
                    break;
                case NavigationItemType.Alerts:
                    // Navigate to alerts
                    CurrentView = new AlertsUI();
                    break;
                case NavigationItemType.Reports:
                    // Navigate to reports
                    CurrentView = new ReportUI();
                    break;
                case NavigationItemType.Settings:
                    // Navigate to settings
                    CurrentView = new SettingsUI();
                    break;
            }

            CurrentNavigationViewChanged?.Invoke(CurrentView);
        }
    }
}
