---
description: C# 14 coding guidelines, naming conventions, and async patterns for .NET 10 projects.
globs: ["**/*.cs"]
alwaysApply: true
---

# C# 14 Coding Standards

## Naming Conventions

- **Classes/Interfaces**: PascalCase (`Customer`, `IOrderService`)
- **Methods**: PascalCase (`CalculateTotal`)
- **Variables/Parameters**: camelCase (`customerId`, `orderTotal`)
- **Private Fields**: `_camelCase` (`_dbContext`)
- **Constants**: PascalCase (`MaxRetryCount`)
- **Interfaces**: Prefix with 'I' (`IRepository`)

## C# 14 Features

### The `field` Keyword

Use the `field` keyword in property accessors to access compiler-synthesized backing fields for validation or notification logic:

```csharp
public string Message
{
    get;
    set => field = value ?? throw new ArgumentNullException(nameof(value));
}
```

### Extension Members

Use extension properties and static members to attach UI or Infrastructure-specific logic to Domain Entities without modifying the core types:

```csharp
public static class Enumerable
{
    extension<TSource>(IEnumerable<TSource> source)
    {
        // Extension property
        public bool IsEmpty => !source.Any();
        
        // Extension method
        public IEnumerable<TSource> Where(Func<TSource, bool> predicate) { ... }
    }
}
```

### Implicit Span Conversions

Default to `ReadOnlySpan<char>` for string-processing methods to achieve zero-allocation parsing. C# 14 provides first-class support for implicit conversions between `Span<T>`, `ReadOnlySpan<T>`, and arrays.

### Null-conditional Assignment

Use the `?.` operator on the left-hand side of assignments:

```csharp
customer?.Order = GetCurrentOrder();  // Only assigns if customer is not null
```

## General Guidelines

- Use **Implicit Usings** and **File-Scoped Namespaces** to reduce boilerplate
- Use `var` when the type is obvious from the right-hand side
- Prefer `is not null` over `!= null`
- Use `record` for DTOs and simple data structures
- Use collection expressions `[1, 2, 3]` for array initialization

## Async/Await

- All I/O bound operations (Database, File, Network) must be `async`
- Use `await` and avoid `.Result` or `.Wait()` to prevent deadlocks
- Suffix async methods with `Async` (e.g., `GetAllOrdersAsync`)

## Type Safety

- All code must be fully typed (C# 14 strictness)
- Enable nullable reference types and treat warnings as errors
- Use `required` modifier for mandatory initialization in constructors
