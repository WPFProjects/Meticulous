using Meticulous.Core;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Meticulous.Infrastructure.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;

        private ViewModelBase _currentViewModel;
        public ViewModelBase CurrentViewModel
        {
            get { return _currentViewModel; }

            set { _currentViewModel = value; }
        }

        public event Action<ViewModelBase> CurrentViewChanged;

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void NavigateTo<TViewModel>() where TViewModel : ViewModelBase
        {
            NavigateTo(typeof(TViewModel));
        }

        public void NavigateTo(Type viewModelType)
        {
            CurrentViewModel = (ViewModelBase)_serviceProvider.GetRequiredService(viewModelType);

            CurrentViewChanged?.Invoke(CurrentViewModel);
        }
    }
}
