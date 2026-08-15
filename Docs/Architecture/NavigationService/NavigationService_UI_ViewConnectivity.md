# Navigation Service UI View Connectivity

This document explains how the Meticulous WPF shell connects its navigation UI, view models, navigation service, and WPF DataTemplates. It is based on `NavigationService_UI_ViewConnectivity.drawio` and verified against the current implementation.

## Runtime view connectivity

```mermaid
flowchart TD
    MainWindow["MainWindow"] -->|DataContext| MainWindowVM["MainWindowViewModel"]
    MainWindowVM -->|Navigation property| NavigationVM["NavigationViewModel"]
    MainWindowVM -->|ContentRegion property| ContentRegionVM["ContentRegionViewModel"]

    NavigationView["NavigationView"] -->|DataContext: Navigation| NavigationVM
    ContentRegionView["ContentRegionView"] -->|DataContext: ContentRegion| ContentRegionVM

    NavigationView -->|button commands| NavigationVM
    NavigationVM -->|NavigateTo<TViewModel>()| NavigationContract["INavigationService"]
    NavigationService["NavigationService"] -. implements .-> NavigationContract
    NavigationService -->|resolves target| ServiceProvider["IServiceProvider"]
    NavigationService -->|updates and raises CurrentViewChanged| CurrentVM["CurrentViewModel"]

    ContentRegionVM -. subscribes to .-> NavigationContract
    CurrentVM -->|event handler updates| ContentRegionVM
    ContentRegionVM -->|CurrentViewModel binding| ContentRegionView
    ContentRegionView -->|ContentControl selects| DataTemplates["NavDataTemplate.xaml"]
    DataTemplates -->|ViewModel type mapping| ScreenViews["DashboardUI / ProcessScreenUI / AlertsUI / ReportUI / SettingsUI"]
```

## Navigation sequence

1. `App.OnStartup` creates the DI container by calling `ServiceRegistration.ConfigureServices()`.
2. `MainWindowViewModel` is resolved from the container and assigned as `MainWindow.DataContext`.
3. `MainWindow` binds `NavigationView` to `MainWindowViewModel.Navigation` and `ContentRegionView` to `MainWindowViewModel.ContentRegion`.
4. The user clicks a button in `NavigationView`. Each button binds to a command on `NavigationViewModel`.
5. The command calls `INavigationService.NavigateTo<TViewModel>()` for the selected screen.
6. `NavigationService` resolves that view model through `IServiceProvider`, assigns it to `CurrentViewModel`, and raises `CurrentViewChanged`.
7. `ContentRegionViewModel` receives the event and updates its own `CurrentViewModel` property, which raises `PropertyChanged`.
8. The `ContentControl` in `ContentRegionView` receives the new content value. WPF selects a matching `DataTemplate` from `NavDataTemplate.xaml` and creates the associated view.

## Responsibilities

| Component | Responsibility |
| --- | --- |
| `MainWindow` | Hosts the navigation and content regions. |
| `MainWindowViewModel` | Exposes the shell view models: `Navigation`, `ContentRegion`, `Header`, `Toolbar`, and `StatusBar`. |
| `NavigationView` | Presents navigation buttons and binds them to navigation commands. |
| `NavigationViewModel` | Owns navigation commands and requests navigation through `INavigationService`. |
| `INavigationService` | Defines the navigation contract: current view model, change event, and `NavigateTo` methods. |
| `NavigationService` | Implements navigation and resolves requested screen view models from DI. |
| `ContentRegionViewModel` | Tracks the active screen view model and notifies WPF when it changes. |
| `ContentRegionView` | Hosts a `ContentControl` bound to the active screen view model. |
| `NavDataTemplate.xaml` | Maps screen view-model types to WPF views. |

## View-model to view mapping

| View model | View selected by the DataTemplate |
| --- | --- |
| `DashboardViewModel` | `DashboardUI` |
| `ProcessesViewModel` | `ProcessScreenUI` |
| `AlertsViewModel` | `AlertsUI` |
| `ReportsViewModel` | `ReportUI` |
| `SettingsViewModel` | `SettingsUI` |

## Dependency injection registrations

`ServiceRegistration` registers `INavigationService` and `NavigationService` as a singleton pair. It also registers the screen view models, `NavigationViewModel`, `ContentRegionViewModel`, and `MainWindowViewModel` as singletons. Because the navigation service resolves screen view models from `IServiceProvider`, every navigable screen type must be registered.

## Implementation notes

- `NavigationViewModel` contains a `NavigationItems` collection with a display name and target `ViewModelType`. The current `NavigationView.xaml` renders fixed buttons bound directly to `DashboardCommand`, `ProcessesCommand`, `AlertsCommand`, `ReportsCommand`, and `SettingsCommand`; it does not currently bind its UI to `NavigationItems`.
- The DataTemplates are merged by both `App.xaml` and `ContentRegionView.xaml`. The content region therefore has access to the mappings it needs to resolve the current view model into a view.
- The navigation flow is ViewModel-first: navigation chooses a view-model type, and WPF chooses the concrete view through a `DataTemplate`.
