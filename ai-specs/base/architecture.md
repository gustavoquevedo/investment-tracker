---
description: Vertical Slice Architecture (VSA), Clean Architecture, and Domain-Driven Design (DDD) patterns for .NET applications.
globs: ["**/*.cs"]
alwaysApply: true
---

# Architecture Patterns

## Overview

This document describes the preferred architectural approach: a hybrid of **Vertical Slice Architecture (VSA)** for Web APIs and **Clean Architecture** for complex domain logic, using **DDD building blocks** throughout.

## Vertical Slice Architecture (VSA)

### When to Use

- **Preferred for Web APIs**: Group code by feature (e.g., `PlaceOrder` folder containing DTOs, Logic, and Endpoint)
- **Benefits**: Reduces file hopping, aligns with Minimal APIs, improves developer velocity

### Sharing Strategy

| What | Strategy |
|------|----------|
| **Entities** | ALWAYS SHARE. Core business truths belong in the Domain layer. |
| **DTOs** | AGGRESSIVELY DUPLICATE. Do not share DTOs between features to avoid tight coupling. "Duplication is cheaper than the wrong abstraction." |
| **Data Logic** | Use **Extension Methods** on `IQueryable` for complex queries instead of Repositories |
| **Shared Biz Logic** | Use **Domain Services** for universal logic (e.g., Tax calculation) |

### The "Rule of Three" for Refactoring

1. Write the code in Slice A (no sharing)
2. Copy it to Slice B (accept duplication)
3. Copy it to Slice C → **STOP**. Only then extract to a shared component.

## Clean Architecture Layers

When domain logic becomes complex, organize into layers with strict dependency rules:

```
┌─────────────────────────────────────────────────────────────┐
│                         UI/API Layer                         │
│            (Controllers, Endpoints, Console App)             │
├─────────────────────────────────────────────────────────────┤
│                     Infrastructure Layer                     │
│         (EF Core, Repositories, External Services)          │
├─────────────────────────────────────────────────────────────┤
│                        Domain Layer                          │
│        (Entities, Value Objects, Domain Services)           │
└─────────────────────────────────────────────────────────────┘

        Dependencies flow INWARD (UI → Infra → Domain)
        Domain has NO external dependencies (Pure C#)
```

### Layer Responsibilities

1. **Domain Layer**
   - Core business logic, entities, value objects, repository interfaces
   - **Dependencies**: None (Pure C#)
   - *Rule*: No reference to external libraries or infrastructure concerns

2. **Infrastructure Layer**
   - Implements interfaces defined in Domain (e.g., Repositories)
   - Handles database access (EF Core), file systems, external API calls
   - **Dependencies**: Domain, EF Core, External SDKs

3. **UI/Presentation Layer**
   - Entry point of the application (Console App or Web API)
   - Orchestrates application flow
   - **Dependencies**: Domain, Infrastructure

## Domain-Driven Design (DDD) Building Blocks

### Entities

- Have identity that persists across state changes
- Private constructor for EF Core, public constructor/factory for creation
- Properties should be `private set` to enforce encapsulation
- Modification only through methods

```csharp
public class Customer
{
    public int Id { get; private set; }
    public string Name { get; private set; }
    
    private Customer() { } // EF Core
    
    public static Customer Create(string name) => new Customer { Name = name };
    
    public void Rename(string newName) => Name = newName;
}
```

### Value Objects

- Immutable objects defined by their attributes, not identity
- Use `record` types for simple value objects

```csharp
public record Money(decimal Amount, string Currency);
```

### Repositories

- Define interfaces in the **Domain** layer
- Implement in the **Infrastructure** layer
- Return Domain Entities, not Database Models

```csharp
// Domain
public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(int id);
    Task<IEnumerable<Customer>> GetAllAsync();
}

// Infrastructure
public class CustomerRepository : ICustomerRepository { ... }
```

### Domain Services

- Contain business logic that doesn't naturally fit in an entity
- Stateless
- Named after the operation they perform

```csharp
public class DiscountService
{
    public decimal CalculateDiscount(Customer customer, decimal orderTotal) { ... }
}
```
