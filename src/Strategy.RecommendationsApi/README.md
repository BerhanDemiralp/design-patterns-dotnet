# Strategy Pattern — Recommendations API

This project demonstrates the **Strategy Design Pattern** using a real, runnable ASP.NET Core Web API that returns product recommendations based on different algorithms.

The purpose is to show how multiple algorithms can be selected **at runtime** without using large `if/else` or `switch` statements, and without modifying existing code when new strategies are added.

---

## Problem

We want to generate product recommendations using different approaches:

* Most popular products
* Newest products
* Personalized recommendations based on user interests
* Category-focused recommendations

The algorithm must be selectable at runtime (via query parameter), and the system should follow the **Open/Closed Principle**:
open for extension, closed for modification.

---

## Solution: Strategy Pattern

Each recommendation algorithm is implemented as a separate class that shares a common interface:

```csharp
public interface IRecommendationStrategy
{
    string Key { get; }
    IReadOnlyList<Product> Recommend(UserContext user, IReadOnlyList<Product> products, int limit);
}
```

Concrete strategies:

* `PopularStrategy`
* `NewStrategy`
* `PersonalizedStrategy`
* `CategoryStrategy`

A context class selects and executes the correct strategy:

* `RecommendationService`

Strategies are registered using **Dependency Injection** and selected at runtime based on the `type` query parameter.

---

## Architecture Overview

```
Client (Swagger / Frontend / Postman)
        |
        v
RecommendationsController (HTTP Layer)
        |
        v
RecommendationService (Strategy Context)
        |
        v
IRecommendationStrategy (Interface)
        |
        +--> PopularStrategy
        +--> NewStrategy
        +--> PersonalizedStrategy
        +--> CategoryStrategy
```

---

## How to Run

```bash
cd src/Strategy.RecommendationsApi
dotnet run
```

Open Swagger UI:

```
https://localhost:<port>/swagger
```

---

## API Usage

### Get Recommendations

```
GET /api/recommendations?type=popular&limit=5
```

Available types:

* `popular` – Highest popularity score
* `new` – Newest products first
* `personalized` – Boosts categories matching user interests
* `category` – Focuses on the first user interest category

Examples:

```
/api/recommendations?type=new
/api/recommendations?type=personalized
/api/recommendations?type=category
```

---

## Key Files

| File                                       | Responsibility              |
| ------------------------------------------ | --------------------------- |
| `Strategies/IRecommendationStrategy.cs`    | Strategy interface          |
| `Strategies/PopularStrategy.cs`            | Popularity-based algorithm  |
| `Strategies/NewStrategy.cs`                | Newest-first algorithm      |
| `Strategies/PersonalizedStrategy.cs`       | Interest-based scoring      |
| `Strategies/CategoryStrategy.cs`           | Category-focused algorithm  |
| `Services/RecommendationService.cs`        | Strategy selector (Context) |
| `Controllers/RecommendationsController.cs` | API endpoint                |

---

## Adding a New Strategy

1. Create a new class implementing `IRecommendationStrategy`
2. Give it a unique `Key`
3. Register it in `Program.cs`:

```csharp
builder.Services.AddScoped<IRecommendationStrategy, MyNewStrategy>();
```

4. Call it:

```
/api/recommendations?type=mynew
```
