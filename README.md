# Design Patterns in C# (.NET)

This repository contains a collection of classic design patterns implemented in C# using small, runnable, backend-focused projects built with ASP.NET Core.

Each pattern is implemented as an isolated mini-project and documented with:

- A short theoretical explanation
- A real-world use case
- Clean architecture and DI usage
- A runnable demo (Swagger / API / console)

---

## Implemented Patterns

### 1. Strategy Pattern

📁 Project: `src/Strategy.RecommendationsApi`  
📖 Documentation:  
➡️ [Strategy Pattern – Recommendations API](src/Strategy.RecommendationsApi/README.md)
Demonstrates runtime selection of different recommendation algorithms using the Strategy design pattern and Dependency Injection.

---

### 2. Factory Pattern

📁 Project: `src/Factory.PaymentsApi`  
📖 Documentation:  
➡️ [Factory Method – Payments API](src/Factory.PaymentsApi/README.md)
The factory receives all provider implementations via Dependency Injection and selects the correct one based on the provider name.

---

### 3. Decorator Pattern

📁 Project: `src/Decorator.CachingLoggingApi`  
📖 Documentation:  
➡️ [Decorator Pattern – Caching Logging API](src/Decorator.CachingLoggingApi/README.md)
The example shows how additional responsibilities can be dynamically layered on top of an existing service using composition instead of inheritance.

---

### 4. Command Pattern

📁 Project: `src/Command.BackgroundJobsApi`  
📖 Documentation:  
➡️ [Command Pattern – Background Jobs API](src/Command.BackgroundJobsApi/README.md)
This project demonstrates the **Command Pattern** in a real-world ASP.NET Core backend scenario: a **Background Jobs API** that processes asynchronous tasks through a command queue.

## Planned Patterns

- State Pattern
- Observer Pattern

Each will be added as a separate mini-project under `src/` with its own README and demo.
