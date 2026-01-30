---
description: Testing standards using Microsoft.Testing.Platform, xUnit v3, FluentAssertions, and Moq.
globs: ["**/tests/**/*.cs", "**/*.Tests/**/*.cs", "**/Test*.cs", "**/*Tests.cs"]
alwaysApply: true
---

# Testing Standards

## Technology Stack

- **Runner**: Microsoft.Testing.Platform (MTP) - Standalone executable execution
- **Framework**: xUnit v3
- **Assertion Library**: FluentAssertions
- **Mocking**: Moq

## Test Naming Convention

Use the pattern: `MethodName_StateUnderTest_ExpectedBehavior`

```csharp
[Fact]
public void CalculateTotal_WithEmptyOrder_ReturnsZero()
{
    // ...
}

[Fact]
public async Task GetOrderAsync_WhenNotFound_ReturnsNull()
{
    // ...
}
```

## Test Structure (Arrange-Act-Assert)

```csharp
[Fact]
public void CalculateDiscount_WithLoyalCustomer_ReturnsCorrectDiscount()
{
    // Arrange
    var customer = new Customer { IsLoyal = true };
    var service = new DiscountService();
    
    // Act
    var result = service.CalculateDiscount(customer, 1000m);
    
    // Assert
    result.Should().Be(100m);
}
```

## Unit Testing

### Focus Areas

- **Domain Logic**: Entities, Value Objects, Domain Services
- **Services**: Business logic, calculations, transformations
- **Utilities**: Helper functions, extensions

### FluentAssertions Examples

```csharp
// Value assertions
result.Should().Be(expected);
result.Should().BeGreaterThan(0);
result.Should().BeNull();

// Collection assertions
items.Should().HaveCount(3);
items.Should().Contain(x => x.Name == "Test");
items.Should().BeEmpty();

// Exception assertions
action.Should().Throw<ArgumentException>()
    .WithMessage("*invalid*");

// Async assertions
await action.Should().ThrowAsync<InvalidOperationException>();
```

### Mocking with Moq

```csharp
[Fact]
public async Task Service_CallsRepository_WithCorrectId()
{
    // Arrange
    var mockRepo = new Mock<IOrderRepository>();
    mockRepo.Setup(r => r.GetByIdAsync(1))
        .ReturnsAsync(new Order { Id = 1, Total = 100m });
    
    var service = new OrderService(mockRepo.Object);
    
    // Act
    var result = await service.GetOrderAsync(1);
    
    // Assert
    result.Should().NotBeNull();
    mockRepo.Verify(r => r.GetByIdAsync(1), Times.Once);
}
```

## Integration Testing

### Database Testing

- Use an in-memory database or test SQLite file
- Verify end-to-end flows including database interactions
- Clean up test data after each test

```csharp
public class IntegrationTestBase : IDisposable
{
    protected readonly TestDbContext Context;
    
    public IntegrationTestBase()
    {
        var options = new DbContextOptionsBuilder<TestDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        Context = new TestDbContext(options);
    }
    
    public void Dispose() => Context.Dispose();
}
```

## Running Tests

```bash
# Run all tests via CLI
dotnet test

# Run with verbose output
dotnet test --logger "console;verbosity=detailed"

# Run specific test project as standalone executable (MTP)
./tests/Project.Tests/bin/Debug/net10.0/Project.Tests.exe
```

## Test Organization

```
tests/
  Project.Tests/
    Domain/           # Tests for domain logic
    Services/         # Tests for services
    Infra/            # Integration tests for repositories
    Fixtures/         # Shared test fixtures
    Helpers/          # Test utilities
```
