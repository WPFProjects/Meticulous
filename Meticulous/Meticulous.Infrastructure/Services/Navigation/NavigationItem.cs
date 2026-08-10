using System;

namespace Meticulous.Infrastructure.Services.Navigation
{
    public sealed class NavigationItem
    {
        public string Name { get; }
        public Type ViewModelType { get; }

        public NavigationItem(string name, Type viewModelType)
        {
            Name = name;
            ViewModelType = viewModelType;
        }
    }
}
