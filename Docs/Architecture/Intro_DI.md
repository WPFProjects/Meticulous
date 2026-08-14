# Dependency Injection (DI) -

## What is Dependency Injection?

Dependency Injection (DI) is a design pattern used to achieve **Inversion of Control (IoC)** by providing the required dependencies of a class from an external source instead of creating them inside the class.

In simple words:

> **A class should receive the objects it needs rather than creating them itself.**

---

# Why do we need Dependency Injection?

Without DI, classes become tightly coupled because they create their own dependencies.

Example:

```csharp
public class EmployeeService
{
    private readonly DatabaseService _databaseService;

    public EmployeeService()
    {
        _databaseService = new DatabaseService();
    }
}
```

Problems:

- Tight coupling
- Difficult to test
- Difficult to replace implementations
- Violates Single Responsibility Principle
- Violates Dependency Inversion Principle

---

# With Dependency Injection

```csharp
public class EmployeeService
{
    private readonly DatabaseService _databaseService;

    public EmployeeService(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }
}
```

Now:

- EmployeeService doesn't create DatabaseService.
- Someone else provides it.

Benefits:

- Loose coupling
- Easy testing
- Easy maintenance
- Better scalability

---

# What is a Dependency?

A dependency is any object that another object requires to perform its work.

Example:

```csharp
public class OrderService
{
    private readonly EmailService _emailService;

    public OrderService(EmailService emailService)
    {
        _emailService = emailService;
    }
}
```

Here,

OrderService depends on EmailService.

Therefore,

EmailService is the dependency.

---

# What is Injection?

Injection means supplying the required dependency from outside the class.

Example:

```csharp
EmailService emailService = new EmailService();

OrderService service = new OrderService(emailService);
```

Here,

EmailService is injected into OrderService.

---

# Before Dependency Injection

```text
OrderService
     │
     ▼
new EmailService()
```

OrderService is responsible for creating EmailService.

---

# After Dependency Injection

```text
Application

      │

      ▼

EmailService

      │

      ▼

OrderService
```

The application creates EmailService and passes it to OrderService.

---

# What is Inversion of Control (IoC)?

Normally,

A class controls the creation of its own dependencies.

With IoC,

The control is inverted.

Instead of the class creating dependencies,

an external component creates them.

DI is one way to implement IoC.

---

# Difference between IoC and DI

IoC

- Principle
- Means giving control to another component

Dependency Injection

- Design Pattern
- One implementation of IoC

Interview Answer:

> Dependency Injection is a technique used to implement the Inversion of Control principle.

---

# Types of Dependency Injection

## 1. Constructor Injection (Most Recommended)

```csharp
public class EmployeeService
{
    private readonly DatabaseService _databaseService;

    public EmployeeService(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }
}
```

Advantages:

- Dependencies are mandatory.
- Object is always valid.
- Immutable dependencies.
- Most widely used.

Industry Standard:
✅ Yes

---

## 2. Property Injection

```csharp
public class EmployeeService
{
    public DatabaseService DatabaseService { get; set; }
}
```

Advantages

Optional dependencies.

Disadvantages

Object may be in an invalid state.

Less preferred.

---

## 3. Method Injection

```csharp
public class EmployeeService
{
    public void Save(DatabaseService databaseService)
    {

    }
}
```

Used when dependency is needed only for one method.

---

# Which Injection is Preferred?

Constructor Injection.

Reason:

- Required dependencies
- Better readability
- Better testing
- Immutable objects

Most enterprise applications use Constructor Injection.

---

# What is a DI Container?

A DI Container automatically creates objects and injects their dependencies.

Examples

- Microsoft.Extensions.DependencyInjection
- Autofac
- Ninject
- Unity
- Castle Windsor

---

# Manual Dependency Injection

Without a DI container:

```csharp
DatabaseService database = new DatabaseService();

EmployeeService employee =
    new EmployeeService(database);
```

This is called Manual Dependency Injection.

It is still Dependency Injection.

---

# Dependency Injection Container

Instead of

```csharp
new EmployeeService(database);
```

You write

```csharp
serviceProvider.GetRequiredService<EmployeeService>();
```

The container automatically

- creates DatabaseService
- passes it
- creates EmployeeService

---

# Service Registration

Example

```csharp
services.AddSingleton<DatabaseService>();

services.AddSingleton<EmployeeService>();
```

Nothing is created here.

The container only stores instructions.

---

# Service Resolution

Objects are created only when requested.

Example

```csharp
var employee =
serviceProvider.GetRequiredService<EmployeeService>();
```

This process is called Resolution.

---

# Object Graph

Suppose

```
MainWindowViewModel
        │
        ├── NavigationViewModel
        │          │
        │          └── NavigationService
        │
        └── ContentRegionViewModel
                   │
                   └── NavigationService
```

This complete hierarchy is called the Object Graph.

The DI container builds the object graph automatically.

---

# Composition Root

The Composition Root is the place where the complete object graph is assembled.

Usually

- Program.cs
- App.xaml.cs
- Bootstrapper

Only the Composition Root should know about the DI Container.

---

# Service Lifetime

## Singleton

One instance for the entire application.

```csharp
services.AddSingleton<ILogger, Logger>();
```

Suitable for

- Logging
- Navigation
- Configuration
- Theme Service

---

## Scoped

One instance per scope.

Mostly used in ASP.NET Core.

Rarely used in desktop applications.

```csharp
services.AddScoped<IEmployeeService, EmployeeService>();
```

---

## Transient

New object every time.

```csharp
services.AddTransient<IReportService, ReportService>();
```

Suitable for

- Short-lived services
- Temporary objects

---

# Constructor Selection

The DI container automatically selects the constructor with the most resolvable parameters.

Example

```csharp
public EmployeeService(DatabaseService db)
{

}
```

The container sees

EmployeeService requires DatabaseService

It creates DatabaseService first

Then EmployeeService.

---

# Dependency Chain

```
EmployeeController

        │

        ▼

EmployeeService

        │

        ▼

EmployeeRepository

        │

        ▼

DatabaseConnection
```

The DI container recursively creates every dependency.

---

# Benefits of Dependency Injection

- Loose Coupling
- Better Maintainability
- Better Testability
- Easier Mocking
- Reusable Components
- Follows SOLID Principles
- Cleaner Architecture
- Easier Refactoring
- Better Separation of Concerns
- Centralized Object Creation

---

# Drawbacks

- Initial learning curve
- More abstraction
- Slight startup overhead
- Can be overused in small projects

---

# DI and SOLID Principles

## Single Responsibility Principle

Classes don't create dependencies.

Only perform business logic.

---

## Open/Closed Principle

Replace implementations without modifying consumers.

---

## Liskov Substitution Principle

Different implementations can replace each other.

---

## Interface Segregation Principle

Classes depend only on required interfaces.

---

## Dependency Inversion Principle

Depend upon abstractions.

Example

```csharp
public class EmployeeService
{
    public EmployeeService(IRepository repository)
    {

    }
}
```

Instead of

```csharp
EmployeeService(DatabaseRepository repository)
```

---

# Common Interview Questions

## Q1. What is Dependency Injection?

Dependency Injection is a design pattern where dependencies are provided to a class from outside rather than being created inside the class.

---

## Q2. What problem does DI solve?

- Tight coupling
- Difficult testing
- Difficult maintenance
- Difficult replacement of implementations

---

## Q3. What is a Dependency?

An object required by another object to perform its work.

---

## Q4. Difference between IoC and DI?

IoC is a principle.

DI is a design pattern implementing IoC.

---

## Q5. What is Constructor Injection?

Dependencies are supplied through the constructor.

It is the preferred approach.

---

## Q6. What is a DI Container?

A framework that automatically creates objects and injects dependencies.

---

## Q7. What is Service Lifetime?

Defines how long an object should live.

- Singleton
- Scoped
- Transient

---

## Q8. What is the Composition Root?

The place where the application builds the dependency graph and configures the DI container.

---

## Q9. Does DI improve performance?

No.

DI improves architecture and maintainability.

Performance improvement is not its goal.

---

## Q10. Can DI exist without a DI Container?

Yes.

Example:

```csharp
DatabaseService db = new DatabaseService();

EmployeeService employee =
    new EmployeeService(db);
```

This is called Manual Dependency Injection.

---

# Interview Summary

Remember these keywords:

- Dependency
- Injection
- Inversion of Control
- Loose Coupling
- Constructor Injection
- DI Container
- Service Registration
- Service Resolution
- Object Graph
- Composition Root
- Singleton
- Scoped
- Transient
- Dependency Inversion Principle
- Microsoft.Extensions.DependencyInjection

---

# One-Line Interview Definition

> Dependency Injection is a design pattern that implements Inversion of Control by supplying a class's required dependencies from an external source instead of allowing the class to create them itself. This results in loose coupling, better maintainability, improved testability, and adherence to SOLID principles.