# Why is `App.xaml.cs` Called the Composition Root?

## Definition

The **Composition Root** is the single place in an application where all the application's objects are created, their dependencies are wired together, and the application's object graph is assembled.

In a WPF application, this responsibility is typically handled by **`App.xaml.cs`** because it is the entry point of the application startup process.

> **Definition :**
>
> *The Composition Root is the location in an application where dependencies are composed, services are registered, and the application's object graph is created.*

---

# Why is it called "Composition"?

The word **Composition** means:

> **Combining multiple independent objects into a complete application.**

Suppose your application contains:

- NavigationService
- HeaderViewModel
- NavigationViewModel
- ToolbarViewModel
- ContentRegionViewModel
- StatusBarViewModel
- MainWindowViewModel

Individually, these classes do nothing.

The application starts working only after they are connected together.

Example:

```text
NavigationService
        │
        ▼
NavigationViewModel
        │
        ▼
MainWindowViewModel
        │
        ▼
MainWindow
```

This process of connecting objects together is called **Composition**.

---

# Why is it called "Root"?

Everything starts from one place.

```text
Application Starts
        │
        ▼
App.xaml.cs
        │
        ▼
Creates Services
        │
        ▼
Creates ViewModels
        │
        ▼
Creates MainWindow
        │
        ▼
Shows Application
```

Since every object originates from this location, it is called the **Root**.

Think of a tree:

```text
           App.xaml.cs
               │
      ─────────┼─────────
      │        │         │
      ▼        ▼         ▼
Navigation   Logging   Settings
      │
      ▼
NavigationViewModel
      │
      ▼
MainWindowViewModel
```

Just like the root of a tree supplies every branch, the Composition Root creates the entire application object graph.

---

# Without Dependency Injection

Without a DI container, the Composition Root manually creates every object.

Example:

```csharp
protected override void OnStartup(StartupEventArgs e)
{
    base.OnStartup(e);

    var navigationService = new NavigationService();

    var header = new HeaderViewModel();

    var navigation = new NavigationViewModel(navigationService);

    var toolbar = new ToolbarViewModel();

    var content = new ContentRegionViewModel(navigationService);

    var status = new StatusBarViewModel();

    var mainWindowViewModel = new MainWindowViewModel(
        header,
        navigation,
        toolbar,
        content,
        status);

    MainWindow mainWindow = new MainWindow();

    mainWindow.DataContext = mainWindowViewModel;

    mainWindow.Show();
}
```

Here, **App.xaml.cs** creates every object manually.

Therefore, it is the Composition Root.

---

# With Dependency Injection

When using `Microsoft.Extensions.DependencyInjection`, App.xaml.cs becomes much simpler.

```csharp
protected override void OnStartup(StartupEventArgs e)
{
    base.OnStartup(e);

    ServiceProvider provider = ServiceRegistration.ConfigureServices();

    MainWindow window = provider.GetRequiredService<MainWindow>();

    window.Show();
}
```

Although object creation is delegated to the DI container, **App.xaml.cs** still starts the process by:

1. Building the DI container
2. Requesting the root object
3. Starting the application

Therefore, it is still the Composition Root.

---

# What Actually Happens?

When App.xaml.cs requests:

```csharp
provider.GetRequiredService<MainWindow>();
```

The DI container builds the complete object graph automatically.

```text
MainWindow
      │
      ▼
MainWindowViewModel
      │
      ├──────────────┐
      │              │
      ▼              ▼
NavigationVM    HeaderVM
      │
      ▼
NavigationService
```

This complete hierarchy is called the **Object Graph**.

---

# Responsibilities of the Composition Root

The Composition Root should be responsible for:

- Registering services
- Registering ViewModels
- Configuring the DI container
- Creating the root window
- Starting the application

The Composition Root **should not** contain business logic.

---

# Why Should Object Creation Be Centralized?

Imagine object creation is scattered across the application.

Example:

```text
MainWindowViewModel
        │
        ├── new NavigationService()
        │
NavigationViewModel
        │
        ├── new Logger()
        │
DashboardViewModel
        │
        ├── new DatabaseService()
```

Problems:

- Tight coupling
- Difficult maintenance
- Difficult testing
- Duplicate object creation
- Hidden dependencies

Instead,

All object creation should happen in one place.

```text
App.xaml.cs
       │
       ▼
DI Container
       │
       ▼
Entire Object Graph
```

---

# Benefits of a Composition Root

- Centralized object creation
- Loose coupling
- Better maintainability
- Better readability
- Easier testing
- Cleaner architecture
- Easy replacement of implementations
- Supports Dependency Injection
- Follows SOLID principles

---

# Relationship Between Composition Root and Dependency Injection

Many developers think these are the same thing.

They are not.

## Composition Root

A design concept.

It is the place where the application is assembled.

## Dependency Injection

A design pattern used to supply dependencies.

## DI Container

A framework that automates Dependency Injection.

Example:

```text
Composition Root
        │
        ▼
Registers Services
        │
        ▼
Builds DI Container
        │
        ▼
Requests Root Object
```

---

# Can an Application Have Multiple Composition Roots?

No.

A well-designed application should have **one Composition Root**.

Having multiple places creating application objects leads to inconsistent lifetimes and hidden dependencies.

---

# Common Locations of the Composition Root

## WPF

```text
App.xaml.cs
```

## WinForms

```text
Program.cs
```

## ASP.NET Core

```text
Program.cs
```

## Console Application

```text
Program.cs
```

The startup file of the application is usually the Composition Root.

---

# Composition Root vs Object Factory

## Object Factory

Creates one or a few related objects.

Example:

```csharp
EmployeeFactory
```

## Composition Root

Creates the **entire application object graph**.

It may internally use factories and a DI container.

---

# Questions

## Q1. What is a Composition Root?

**Answer:**

A Composition Root is the single location where an application's dependencies are configured and the complete object graph is assembled before the application starts running.

---

## Q2. Why is App.xaml.cs called the Composition Root?

**Answer:**

Because it is responsible for:

- Configuring services
- Building the DI container
- Creating or resolving the root object
- Starting the application

Everything begins from App.xaml.cs.

---

## Q3. Does using a DI Container remove the Composition Root?

**Answer:**

No.

The DI container performs object creation, but the Composition Root is still responsible for configuring the container and requesting the application's root object.

---

## Q4. Should business logic exist inside the Composition Root?

**Answer:**

No.

The Composition Root should only configure dependencies and start the application.

Business logic belongs in services, ViewModels, or domain classes.

---

# Key Takeaways

- **Composition** means assembling objects into a complete application.
- **Root** means the starting point from which the entire object graph is built.
- `App.xaml.cs` is called the Composition Root because it initializes and starts the application.
- The Composition Root should be the only place that knows about the DI container.
- Centralizing object creation leads to a cleaner, more maintainable, and testable architecture.
- In enterprise applications, keeping a single Composition Root is considered a best practice.