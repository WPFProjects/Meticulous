using Meticulous.Core;
using System;

namespace Meticulous.Infrastructure.Services.Navigation
{
    public interface INavigationService
    {
        ViewModelBase CurrentViewModel { get; }

        event Action<ViewModelBase> CurrentViewChanged;

        void NavigateTo<TViewModel>() where TViewModel : ViewModelBase;

        void NavigateTo(Type viewModelType);
    }
};
