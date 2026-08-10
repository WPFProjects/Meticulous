using Meticulous.Core;
using Meticulous.Infrastructure.Services.Navigation;

namespace Meticulous.ViewModel.Regions
{
    public class ContentRegionViewModel : ViewModelBase
    {
        private ViewModelBase _currentViewModel;

        public ViewModelBase CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                if (_currentViewModel != value)
                {
                    _currentViewModel = value;
                    OnPropertyChanged(nameof(CurrentViewModel));
                }
            }
        }

        public ContentRegionViewModel(INavigationService navigationService)
        {
            CurrentViewModel = navigationService.CurrentViewModel as ViewModelBase;
            navigationService.CurrentViewChanged += OnCurrentViewChanged;
        }

        private void OnCurrentViewChanged(ViewModelBase @base)
        {
            CurrentViewModel = @base;
        }
    }
}
