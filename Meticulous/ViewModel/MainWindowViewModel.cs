using Meticulous.Infrastructure.Services.Navigation;
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

        public MainWindowViewModel(INavigationService navigationService,
            HeaderViewModel headerViewModel,
            NavigationViewModel navigationViewModel,
            ToolbarViewModel toolbarViewModel,
            ContentRegionViewModel contentRegionViewModel,
            StatusBarViewModel statusBarViewModel)
        {
            Header = headerViewModel;
            Navigation = navigationViewModel;
            Toolbar = toolbarViewModel;
            ContentRegion = contentRegionViewModel;
            StatusBar = statusBarViewModel;
        }
    }
}
