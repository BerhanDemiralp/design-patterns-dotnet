# Decorator Pattern — Caching & Logging API

This project demonstrates the **Decorator Design Pattern** using an ASP.NET Core Web API that adds **logging** and **caching** behavior around a core service without modifying its implementation.

The example shows how additional responsibilities can be dynamically layered on top of an existing service using composition instead of inheritance.

---

## Problem

We have a core service that returns a list of products:

- `GetProductsAsync()`

Over time, we want to add **cross-cutting concerns** such as:

- Logging (execution time, request info)
- Caching (avoid repeated expensive calls)

A naive approach would be to modify the service directly, but this leads to:

- Tight coupling
- Violations of the Open/Closed Principle
- Hard-to-maintain “god services”

---

## Solution: Decorator Pattern

The **Decorator Pattern** allows behavior to be added to an object dynamically by wrapping it with another object that implements the same interface.

In this project:

- All services implement `IProductCatalogService`
- Decorators also implement `IProductCatalogService`
- Each decorator wraps another `IProductCatalogService` instance (`Inner`)

This allows multiple behaviors to be layered transparently.

---

## Core Interface

```csharp
public interface IProductCatalogService
{
    Task<IReadOnlyList<Product>> GetProductsAsync(CancellationToken ct);
}
```

---

## Core Service

`ProductCatalogService` contains the **actual business logic**:

- Simulates a slow external call
- Returns a static list of products
- Has no knowledge of logging or caching

```csharp
ProductCatalogService
```

---

## Decorators

### Base Decorator

A shared abstract base class stores the wrapped service (`Inner`):

```csharp
ProductCatalogDecoratorBase
```

This avoids duplication and ensures all decorators behave consistently.

---

### Logging Decorator

`LoggingProductCatalogDecorator`:

- Logs when the method starts
- Measures execution time
- Logs the number of returned items

This decorator **adds behavior before and after** delegating the call to `Inner`.

---

### Caching Decorator

`CachingProductCatalogDecorator`:

- Uses `IMemoryCache`
- Returns cached results when available
- Calls the inner service only on cache miss

This decorator can completely bypass the inner service when data is cached.

---

## Architecture Overview

```
Client (Swagger / Frontend / Postman)
        |
        v
ProductsController (HTTP Layer)
        |
        v
LoggingProductCatalogDecorator
        |
        v
CachingProductCatalogDecorator
        |
        v
ProductCatalogService (Core Logic)
```

Each layer implements the same interface and delegates to the next layer.

---

## How to Run

```bash
cd src/Decorator.CachingLoggingApi
dotnet run
```

Open Swagger UI:

```
https://localhost:<port>/swagger
```

---

## API Usage

### Get Products

```
GET /api/products
```

Behavior:

- First request:
  - ~400 ms response time
  - Cache miss

- Subsequent requests:
  - Very fast
  - Cache hit
  - Core service not called

Execution times can be observed in the application logs.

---

## Key Files

| File                                           | Responsibility                |
| ---------------------------------------------- | ----------------------------- |
| `Services/IProductCatalogService.cs`           | Common service interface      |
| `Services/ProductCatalogService.cs`            | Core business logic           |
| `Decorators/ProductCatalogDecoratorBase.cs`    | Base decorator                |
| `Decorators/LoggingProductCatalogDecorator.cs` | Logging behavior              |
| `Decorators/CachingProductCatalogDecorator.cs` | Caching behavior              |
| `Controllers/ProductsController.cs`            | API endpoint                  |
| `Program.cs`                                   | Decorator chain configuration |

---

## Decorator Chain Configuration

The decorator chain is configured manually in `Program.cs`:

```csharp
IProductCatalogService svc = new ProductCatalogService();
svc = new CachingProductCatalogDecorator(svc, cache);
svc = new LoggingProductCatalogDecorator(svc, logger);
```

**Order matters**:

- Logging wraps caching
- Caching wraps the core service

This determines which behaviors execute first.
