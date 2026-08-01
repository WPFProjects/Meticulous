using Meticulous.Services;
using Meticulous.ViewModel.Regions;

namespace Meticulous.ViewModel
{
    internal class MainWindowViewModel
    {
        public HeaderViewModel Header { get; }

        public NavigationViewModel Navigation { get; }

        public ToolbarViewModel Toolbar { get; }

        public ContentRegionViewModel ContentRegion { get; }

        public StatusBarViewModel StatusBar { get; }

        public MainWindowViewModel()
        {
            var navigationService = new NavigationService();
            Header = new HeaderViewModel();
            Navigation = new NavigationViewModel(navigationService);
            Toolbar = new ToolbarViewModel();
            ContentRegion = new ContentRegionViewModel(navigationService);
            StatusBar = new StatusBarViewModel();
        }
    }
}
