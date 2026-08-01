namespace Meticulous.Model
{
    public enum NavigationItemType
    {
        Dashboard,
        Processes,
        Alerts,
        Reports,
        Settings,
    }
    internal class NavigationItem
    {
        private NavigationItemType Type { get; set; }
        private object ViewModel { get; set; }
        public NavigationItem(NavigationItemType type, object viewModel)
        {
            Type = type;
            ViewModel = viewModel;
        }
    }
}
