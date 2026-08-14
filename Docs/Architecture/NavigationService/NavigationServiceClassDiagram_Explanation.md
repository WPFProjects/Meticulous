# Navigation Service Class Diagram —

## 1. 

> This class diagram shows the navigation architecture of my WPF application.
>
> The main purpose of this design is to keep navigation logic separate from the ViewModels and Views. I am using MVVM with Dependency Injection.
>
> At the center of the design, I have `INavigationService`. This is an interface which defines what navigation can do. It exposes the current ViewModel, a `CurrentViewChanged` event, and the `NavigateTo` methods.
>
> `NavigationService` is the concrete implementation of `INavigationService`. It receives `IServiceProvider` through its constructor. Instead of creating ViewModels directly using `new`, it asks the Dependency Injection container to create or resolve the required ViewModel.
>
> For example, when the user selects Dashboard, the `NavigationViewModel` calls the navigation service. The navigation service resolves `DashboardViewModel`, assigns it to `CurrentViewModel`, and raises the `CurrentViewChanged` event.
>
> `NavigationViewModel` is responsible for the navigation UI. It contains the navigation items and commands such as Dashboard, Processes, Alerts, Reports, and Settings. It does not create the actual ViewModels. It only asks `INavigationService` to navigate.
>
> `ContentRegionViewModel` is responsible for the content area. It subscribes to the `CurrentViewChanged` event from `INavigationService`. When navigation changes, it updates its own `CurrentViewModel`. The WPF view can then display the correct View through DataTemplates.
>
> All the screen ViewModels, such as `DashboardViewModel`, `ProcessesViewModel`, `AlertsViewModel`, `ReportsViewModel`, `SettingsViewModel`, and `NotificationViewModel`, inherit from `ViewModelBase`. `ViewModelBase` provides the common `INotifyPropertyChanged` functionality required for WPF data binding.
>
> `ServiceRegistration` is responsible for configuring the Dependency Injection container. It registers the navigation service, ViewModels, and other application services. This is part of the composition root concept.
>
> `App` is the application startup point. During startup, it configures the services, obtains the required objects from the DI container, creates the main window, and connects its ViewModel.
>
> `MainWindow` has a `DataContext` of `MainWindowViewModel`. The main window acts as the shell of the application, while the actual navigation logic remains in the navigation-related ViewModels and service.
>
> So, the overall flow is: user clicks a navigation item -> `NavigationViewModel` calls `INavigationService` -> `NavigationService` resolves the target ViewModel using DI -> `CurrentViewModel` changes -> `CurrentViewChanged` is raised -> `ContentRegionViewModel` receives the new ViewModel -> WPF DataTemplate displays the corresponding View.`

---

## 2. Main purpose of the design

The design separates responsibilities:

| Class | Responsibility |
|---|---|
| `INavigationService` | Defines the navigation contract |
| `NavigationService` | Implements navigation and resolves ViewModels |
| `NavigationViewModel` | Handles navigation menu and commands |
| `ContentRegionViewModel` | Manages the currently displayed ViewModel |
| `ViewModelBase` | Provides common ViewModel functionality and property change notification |
| Screen ViewModels | Contain screen-specific presentation logic |
| `NavigationItem` | Represents one navigation menu item |
| `ServiceRegistration` | Registers dependencies in the DI container |
| `App` | Application startup and composition root |
| `MainWindow` | Main WPF shell window |
| `MainWindowViewModel` | ViewModel for the main window |

---

## 3. `INavigationService`

`INavigationService` is the abstraction for navigation.

It contains:

```csharp
public interface INavigationService
{
    ViewModelBase CurrentViewModel { get; }

    event Action<ViewModelBase> CurrentViewChanged;

    void NavigateTo<TViewModel>()
        where TViewModel : ViewModelBase;

    void NavigateTo(Type viewModelType);
}
```

### Why use an interface?

The ViewModels depend on `INavigationService`, not on the concrete `NavigationService`.

For example:

```csharp
public NavigationViewModel(INavigationService navigationService)
{
    _navigationService = navigationService;
}
```

This follows Dependency Inversion Principle.

It also makes the ViewModel easier to unit test because a mock or fake `INavigationService` can be supplied.

---

## 4. `NavigationService`

`NavigationService` is the actual implementation.

It contains:

```csharp
private readonly IServiceProvider _serviceProvider;

private ViewModelBase _currentViewModel;

public ViewModelBase CurrentViewModel
{
    get => _currentViewModel;
}
```

The important dependency is:

```csharp
IServiceProvider
```

The navigation service uses the DI container to resolve the requested ViewModel.

Conceptually:

```text
NavigationService
       |
       | asks DI container
       v
IServiceProvider
       |
       v
DashboardViewModel
```

This avoids code such as:

```csharp
new DashboardViewModel();
new ProcessesViewModel();
new AlertsViewModel();
```

inside the navigation service.

---

## 5. Navigation flow

A typical navigation operation works like this:

```text
User
  |
  v
Navigation UI
  |
  v
NavigationViewModel
  |
  | NavigateTo<DashboardViewModel>()
  v
INavigationService
  |
  v
NavigationService
  |
  | resolve ViewModel
  v
IServiceProvider
  |
  v
DashboardViewModel
  |
  v
CurrentViewModel changes
  |
  v
CurrentViewChanged event
  |
  v
ContentRegionViewModel
  |
  v
WPF ContentControl
  |
  v
Dashboard View
```

This is the most important flow to understand.

---

## 6. `NavigationViewModel`

`NavigationViewModel` represents the navigation menu.

It has:

```csharp
private readonly INavigationService _navigationService;

public ObservableCollection<NavigationItem> NavigationItems { get; }

public ICommand DashboardCommand { get; }
public ICommand ProcessesCommand { get; }
public ICommand AlertsCommand { get; }
public ICommand ReportsCommand { get; }
public ICommand SettingsCommand { get; }
```

The important point is that it depends on the interface:

```csharp
INavigationService
```

It does not depend directly on:

```csharp
NavigationService
```

This keeps the ViewModel loosely coupled.

### Example

When Dashboard is selected:

```csharp
_navigationService.NavigateTo<DashboardViewModel>();
```

The `NavigationViewModel` does not know how `DashboardViewModel` is created.

That responsibility belongs to the navigation service and DI container.

---

## 7. `ContentRegionViewModel`

`ContentRegionViewModel` controls what is currently shown in the main content area.

It receives `INavigationService` through constructor injection:

```csharp
public ContentRegionViewModel(INavigationService navigationService)
{
    navigationService.CurrentViewChanged += OnCurrentViewChanged;
}
```

When the navigation service raises:

```csharp
CurrentViewChanged
```

the method:

```csharp
OnCurrentViewChanged(ViewModelBase viewModel)
```

updates:

```csharp
CurrentViewModel
```

The WPF UI can then bind to this property.

---

## 8. `ViewModelBase`

All screen ViewModels inherit from `ViewModelBase`.

For example:

```text
             ViewModelBase
                  ^
                  |
     +------------+-------------+
     |            |             |
DashboardVM  ProcessesVM   AlertsVM
     |
ReportsVM
SettingsVM
NotificationVM
```

`ViewModelBase` implements `INotifyPropertyChanged`.

This is required so that when a ViewModel property changes, WPF can update the UI automatically.

Example:

```csharp
public abstract class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName));
    }
}
```

---

## 9. Screen ViewModels

The diagram contains these screen-level ViewModels:

- `DashboardViewModel`
- `ProcessesViewModel`
- `AlertsViewModel`
- `ReportsViewModel`
- `SettingsViewModel`
- `NotificationViewModel`

They inherit from `ViewModelBase`.

Their responsibility is to contain presentation logic for their corresponding screen.

For example:

```text
DashboardViewModel
       |
       v
DashboardUI
```

The ViewModel should not directly create or manipulate the View.

The connection between ViewModel and View is normally handled by WPF DataTemplates.

---

## 10. `NavigationItem`

`NavigationItem` represents one navigation entry.

It contains:

```csharp
public string Name { get; }

public Type ViewModelType { get; }
```

For example:

```csharp
new NavigationItem(
    "Dashboard",
    typeof(DashboardViewModel));
```

This allows the navigation system to store the target ViewModel type instead of storing a View instance.

That is useful because the application follows a ViewModel-first navigation approach.

---

## 11. `ServiceRegistration`

`ServiceRegistration` is responsible for configuring Dependency Injection.

Conceptually:

```csharp
public static ServiceProvider ConfigureServices()
{
    var services = new ServiceCollection();

    services.AddSingleton<INavigationService, NavigationService>();

    services.AddTransient<DashboardViewModel>();
    services.AddTransient<ProcessesViewModel>();
    services.AddTransient<AlertsViewModel>();
    services.AddTransient<ReportsViewModel>();
    services.AddTransient<SettingsViewModel>();

    services.AddTransient<NavigationViewModel>();
    services.AddTransient<ContentRegionViewModel>();
    services.AddTransient<MainWindowViewModel>();

    return services.BuildServiceProvider();
}
```

The exact lifetime can depend on the application's requirements.

The important idea is:

```text
ServiceRegistration
       |
       v
IServiceCollection
       |
       v
ServiceProvider
       |
       v
Application objects
```

---

## 12. `App` as the Composition Root

`App` is the startup point of the WPF application.

It is responsible for putting the object graph together.

A simplified flow is:

```text
App.OnStartup()
      |
      v
ServiceRegistration.ConfigureServices()
      |
      v
ServiceProvider
      |
      v
Resolve MainWindowViewModel
      |
      v
Resolve MainWindow
      |
      v
Set DataContext
      |
      v
Show MainWindow
```

### explanation

> I consider `App.xaml.cs` or the startup configuration as the composition root because this is where the application's dependencies are configured and the object graph is created. The individual classes do not need to know how their dependencies are constructed.

---

## 13. `MainWindow` and `MainWindowViewModel`

`MainWindow` is the WPF shell window.

It has:

```csharp
DataContext
```

which points to:

```text
MainWindowViewModel
```

The important relationship is:

```text
MainWindow
    |
    | DataContext
    v
MainWindowViewModel
```

The `MainWindowViewModel` can coordinate the major shell ViewModels such as:

```text
Header
Navigation
Content
Status Bar
```

The exact composition depends on how the shell is implemented.

---

# 14. Relationship types in the diagram

Understanding UML relationship types is important.

## 14.1 Inheritance / Generalization

Example:

```text
DashboardViewModel
        |
        | inherits
        v
   ViewModelBase
```

Meaning:

> `DashboardViewModel` is a type of `ViewModelBase`.

The same applies to the other screen ViewModels.

---

## 14.2 Interface implementation / Realization

Example:

```text
NavigationService
        |
        | implements
        v
INavigationService
```

Meaning:

> `NavigationService` provides the implementation of the contract defined by `INavigationService`.

This should be represented as a **realization** relationship in UML, not normal inheritance.

---

## 14.3 Dependency

Example:

```text
NavigationViewModel
        - - - - >
INavigationService
```

Meaning:

> `NavigationViewModel` depends on `INavigationService` because it receives and uses the service through constructor injection.

The dependency is intentionally toward the abstraction.

---

## 14.4 Association

Example:

```text
MainWindow
    |
    | DataContext
    v
MainWindowViewModel
```

This represents that the window has a reference to its ViewModel through `DataContext`.

---

## 14.5 Registration dependency

`ServiceRegistration` has relationships with the services and ViewModels it registers.

This is better understood as:

> `ServiceRegistration` configures the DI container with these types.

It does not mean that `ServiceRegistration` owns or creates these classes permanently.

---

# 15. Important correction to the diagram

There are a few things I would improve in the current diagram.

## Correction 1 — `NavigationService` and `INavigationService`

The relationship should be:

```text
NavigationService
        - - - -▷
INavigationService
```

This represents **interface realization / implementation**.

It should not be described as normal class inheritance.

---

## Correction 2 — ViewModel naming

Use singular class names:

```text
DashboardViewModel
ProcessesViewModel
AlertsViewModel
ReportsViewModel
SettingsViewModel
```

Avoid names such as:

```text
DashboardViewModels
ReportsViewModels
```

because the class represents one ViewModel.

---

## Correction 3 — `ServiceRegistration` should not contain a generic field

The diagram currently contains a placeholder similar to:

```text
+ field : type
```

This should be removed.

Instead, show the meaningful operation:

```text
+ ConfigureServices() : ServiceProvider
```

If needed, show the registrations as notes or dependencies.

---

## Correction 4 — `IServiceProvider`

`IServiceProvider` is a .NET framework abstraction.

It is useful to show it because `NavigationService` receives it through constructor injection, but it does not need to be treated as an application-owned class.

In a high-level architecture diagram, it can also be shown as an external/framework dependency.

---

## Correction 5 — DataTemplate mapping

The class diagram intentionally does not need to contain all View-to-ViewModel DataTemplate relationships.

For example:

```xml
<DataTemplate DataType="{x:Type vm:DashboardViewModel}">
    <dashboard:DashboardUI />
</DataTemplate>
```

These mappings are better represented in a separate **ViewModel-to-View mapping diagram** or documented separately.

This keeps the structural class diagram easier to understand.

---

# 16. SOLID principles shown by this design

## Single Responsibility Principle

Each component has a clear responsibility.

```text
NavigationViewModel
    -> navigation UI and commands

NavigationService
    -> navigation operation

ContentRegionViewModel
    -> current content state

ServiceRegistration
    -> dependency configuration
```

This avoids putting all navigation logic inside one ViewModel.

---

## Dependency Inversion Principle

High-level ViewModels depend on:

```csharp
INavigationService
```

instead of:

```csharp
NavigationService
```

So the dependency direction is:

```text
NavigationViewModel
        |
        v
INavigationService
        ^
        |
NavigationService
```

The abstraction is in the middle.

---

## Open/Closed Principle

Adding a new screen can be done with limited changes.

For example, if we add:

```text
UsersViewModel
```

we can register it with DI and add a navigation item.

The navigation service itself does not need a large `switch` statement like:

```csharp
switch(type)
{
    case Dashboard:
    case Reports:
    case Alerts:
}
```

This is one of the advantages of using type-based navigation with DI.

---

# 17. Why not create ViewModels directly?

A less flexible implementation would be:

```csharp
if (type == typeof(DashboardViewModel))
{
    CurrentViewModel = new DashboardViewModel();
}
```

This makes the navigation service responsible for constructing every ViewModel.

With DI:

```csharp
CurrentViewModel =
    (ViewModelBase)_serviceProvider.GetRequiredService(viewModelType);
```

the container handles construction and dependency injection.

For example, if `DashboardViewModel` later needs another service:

```csharp
public DashboardViewModel(
    IDataService dataService)
{
}
```

the navigation service does not need to change.

The DI container resolves the dependency.

---

# 18. Complete runtime flow

The complete application flow can be explained as:

```text
1. Application starts
        |
        v
2. App / ServiceRegistration configures DI
        |
        v
3. ServiceProvider is created
        |
        v
4. MainWindowViewModel is resolved
        |
        v
5. NavigationViewModel receives INavigationService
        |
        v
6. ContentRegionViewModel receives INavigationService
        |
        v
7. MainWindow is created and receives its DataContext
        |
        v
8. User clicks Dashboard
        |
        v
9. NavigationViewModel executes command
        |
        v
10. INavigationService.NavigateTo<DashboardViewModel>()
        |
        v
11. NavigationService asks IServiceProvider
        |
        v
12. DashboardViewModel is resolved
        |
        v
13. CurrentViewModel is updated
        |
        v
14. CurrentViewChanged event is raised
        |
        v
15. ContentRegionViewModel updates CurrentViewModel
        |
        v
16. WPF DataTemplate selects DashboardUI
        |
        v
17. Dashboard is displayed
```

---

# 19. Short answer

**"Explain your navigation architecture."**

I would answer:

> I implemented ViewModel-first navigation using MVVM and Dependency Injection.
>
> `INavigationService` defines the navigation contract, while `NavigationService` implements it. The navigation service uses `IServiceProvider` to resolve the required ViewModel instead of creating ViewModels directly.
>
> `NavigationViewModel` handles the navigation menu and calls the navigation service when the user selects an item. The navigation service updates `CurrentViewModel` and raises a `CurrentViewChanged` event.
>
> `ContentRegionViewModel` listens to this event and updates its own `CurrentViewModel`. WPF then uses DataTemplates to map the ViewModel to the corresponding View.
>
> All screen ViewModels inherit from `ViewModelBase`, which provides `INotifyPropertyChanged`.
>
> Dependency registration is handled in `ServiceRegistration`, and the application startup acts as the composition root.
>
> The main benefit is loose coupling. The ViewModels depend on `INavigationService` rather than the concrete `NavigationService`, and the navigation service does not need to know how every ViewModel is constructed.

---

# 20. One-line explanation of every important class

| Class | one-liner |
|---|---|
| `App` | Application startup and composition root |
| `ServiceRegistration` | Configures the DI container |
| `INavigationService` | Navigation abstraction/contract |
| `NavigationService` | Performs navigation and resolves ViewModels |
| `NavigationViewModel` | Handles navigation menu and commands |
| `NavigationItem` | Represents a navigation entry and target ViewModel type |
| `ContentRegionViewModel` | Tracks the ViewModel displayed in the content region |
| `ViewModelBase` | Common base class for WPF ViewModels |
| `DashboardViewModel` | Presentation logic for Dashboard |
| `ProcessesViewModel` | Presentation logic for Processes |
| `AlertsViewModel` | Presentation logic for Alerts |
| `ReportsViewModel` | Presentation logic for Reports |
| `SettingsViewModel` | Presentation logic for Settings |
| `NotificationViewModel` | Presentation logic for notifications |
| `MainWindowViewModel` | ViewModel for the main application shell |
| `MainWindow` | Main WPF application window |
| `IServiceProvider` | Resolves registered dependencies from the DI container |

---

# 21. Key points to remember

1. **NavigationViewModel does not create ViewModels.**
2. **NavigationService does not directly create every ViewModel.**
3. **DI container is responsible for object creation.**
4. **ViewModels depend on `INavigationService`, not `NavigationService`.**
5. **`CurrentViewChanged` notifies the content region about navigation changes.**
6. **`ViewModelBase` provides `INotifyPropertyChanged`.**
7. **DataTemplates map ViewModels to Views.**
8. **`App`/startup configuration is the composition root.**
9. **`ServiceRegistration` configures the DI container.**
10. **This design reduces coupling and avoids a large navigation `switch` statement.**

---

## Final architecture

```text
                         ┌─────────────────────┐
                         │        App          │
                         │  Composition Root   │
                         └──────────┬──────────┘
                                    │
                                    v
                         ┌─────────────────────┐
                         │ ServiceRegistration │
                         │   ConfigureServices │
                         └──────────┬──────────┘
                                    │
                                    v
                         ┌─────────────────────┐
                         │  IServiceProvider  │
                         └──────┬────────┬─────┘
                                │        │
                   resolves    │        │ resolves
                                v        v
                   ┌──────────────┐   ┌──────────────────┐
                   │ Navigation   │   │ MainWindow       │
                   │ Service      │   │ ViewModel        │
                   └──────┬───────┘   └──────────────────┘
                          │
                          │ implements
                          v
                 ┌─────────────────────┐
                 │ INavigationService  │
                 └───────┬─────────────┘
                         │
             ┌───────────┴────────────┐
             │                        │
             v                        v
┌────────────────────────┐  ┌────────────────────────┐
│ NavigationViewModel    │  │ ContentRegionViewModel │
│                        │  │                        │
│ Navigation commands    │  │ CurrentViewModel       │
└───────────┬────────────┘  └───────────┬────────────┘
            │                           │
            │ NavigateTo<T>()           │ listens to
            │                           │ CurrentViewChanged
            └──────────────┬────────────┘
                           v
                  ┌──────────────────┐
                  │  Screen          │
                  │  ViewModels      │
                  ├──────────────────┤
                  │ Dashboard        │
                  │ Processes        │
                  │ Alerts           │
                  │ Reports          │
                  │ Settings         │
                  └────────┬─────────┘
                           │
                           │ DataTemplate
                           v
                  ┌──────────────────┐
                  │      Views       │
                  │ DashboardUI etc. │
                  └──────────────────┘
```

This is the explanation I would use as the basis for an discussion. The most important part is being able to explain the runtime flow from **user click → NavigationViewModel → INavigationService → NavigationService → DI → CurrentViewModel → ContentRegionViewModel → DataTemplate → View**.
