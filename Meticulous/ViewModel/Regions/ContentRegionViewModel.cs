using Meticulous.Services;
using System.ComponentModel;

namespace Meticulous.ViewModel.Regions
{
    public class ContentRegionViewModel : INotifyPropertyChanged
    {
        private NavigationService _navigationService;

        private object _currentView;

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                if (_currentView != value)
                {
                    _currentView = value;
                    // Raise property changed event if implementing INotifyPropertyChanged
                    OnPropertyChanged("CurrentView");
                }
            }
        }

        public ContentRegionViewModel(NavigationService navigationService)
        {
            // Initialize properties and commands for the content region
            _navigationService = navigationService;
            _navigationService.CurrentNavigationViewChanged += OnCurrentNavigationViewChanged;
        }

        private void OnCurrentNavigationViewChanged(object newView)
        {
            CurrentView = newView;
        }
    }
}
