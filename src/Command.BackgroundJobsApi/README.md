# Command Pattern — Background Jobs API

## Overview

This project demonstrates the **Command Pattern** in a real-world ASP.NET Core backend scenario: a **Background Jobs API** that processes asynchronous tasks through a command queue.

The pattern decouples the object that invokes an operation from the object that performs it, enabling:
- **Request queuing and deferred execution**
- **Audit trails and job tracking**
- **Command chaining and orchestration**
- **Easy extensibility without modifying existing code**

## Problem

In backend systems, certain operations are too slow or resource-intensive to execute synchronously within an HTTP request:

- Sending emails via external SMTP providers (500-2000ms latency)
- SMS notifications through third-party gateways
- Report generation requiring heavy database queries
- Multi-step workflows (e.g., user registration → welcome email + SMS)

**Without the Command Pattern:**
- Controllers become bloated with business logic
- HTTP clients timeout waiting for slow operations
- No visibility into job status or failure recovery
- Adding new job types requires modifying existing dispatch logic
- Tight coupling between request creation and execution

## Solution

The Command Pattern introduces a **Command object** that encapsulates:
- What action to perform (data/parameters)
- Who will perform it (Handler)
- When to perform it (Queue + Worker)

**Key Roles:**
- **ICommand**: Marker interface for all command types
- **ICommandHandler<T>**: Executes a specific command type
- **ICommandQueue**: Producer/consumer buffer for commands
- **CommandDispatcher**: Resolves and invokes the correct handler
- **CommandWorker**: BackgroundService that processes queued commands
- **JobStore**: Tracks job status (Queued → Processing → Succeeded/Failed)

## Architecture Overview

```
┌─────────────┐     ┌──────────────┐     ┌─────────────────┐
│   Client    │────▶│  Controller  │────▶│   JobStore      │
│  (Swagger)  │     │              │     │  (Job Created)  │
└─────────────┘     └──────────────┘     └─────────────────┘
                            │
                            ▼
                     ┌──────────────┐
                     │   Command    │
                     │   Created    │
                     └──────────────┘
                            │
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                    ICommandQueue                            │
│                 (Channel<T> in-memory)                      │
└─────────────────────────────────────────────────────────────┘
                            │
                            │ DequeueAsync (Worker)
                            ▼
┌─────────────────────────────────────────────────────────────┐
│                  CommandWorker                              │
│              (BackgroundService)                            │
│  ┌─────────────────────────────────────────────────────┐    │
│  │ 1. MarkProcessing                                   │    │
│  │ 2. DispatchAsync → CommandDispatcher                │    │
│  │ 3. Handler.Execute()                                │    │
│  │ 4. MarkSucceeded / MarkFailed                       │    │
│  └─────────────────────────────────────────────────────┘    │
└─────────────────────────────────────────────────────────────┘
```

**Command Chaining Flow:**
```
UserRegisteredCommand (Parent)
    │
    ├──▶ SendSmsCommand (Child Job #1)
    └──▶ SendEmailCommand (Child Job #2)
```

## Core Interfaces / Concepts

### ICommand
All commands implement this marker interface. The `JobId` property enables tracking across the pipeline.

### ICommandHandler<T>
Generic handler interface with compile-time type safety. Each command has exactly one handler.

### CommandDispatcher
Uses reflection to resolve `ICommandHandler<T>` from DI based on the runtime type of the command. Eliminates the need for switch/if statements when adding new commands.

### InMemoryCommandQueue
Producer/consumer queue using .NET `Channel<T>`. Production systems would replace this with RabbitMQ, Azure Service Bus, or Kafka.

### JobStore
Thread-safe job tracking using `ConcurrentDictionary`. Tracks status transitions and completion timestamps.

## Implementation Highlights

### DI Lifetime Decisions

| Service | Lifetime | Rationale |
|---------|----------|-----------|
| `JobStore` | Singleton | In-memory state must persist across requests |
| `ICommandQueue` | Singleton | Single channel instance for all producers/consumers |
| `CommandDispatcher` | Scoped | Resolves handlers from scoped DI container |
| `ICommandHandler<T>` | Scoped | Can depend on scoped services (DbContext, etc.) |

### Why Reflection in Dispatcher?

The `CommandDispatcher` uses reflection to invoke the correct handler:
```csharp
var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
var handler = _serviceProvider.GetRequiredService(handlerType);
```

This enables **Open/Closed Principle**: new commands require only:
1. New Command class
2. New Handler class  
3. One DI registration line

No changes to Dispatcher, Worker, or Queue.

### Parent-Child Job Pattern

The `UserRegisteredCommandHandler` demonstrates **command chaining**:
- Parent job (UserRegistered) completes quickly
- Creates child jobs (SMS, Email) with new JobIds
- Child jobs are independent, trackable entities
- Worker processes each child separately

This pattern is essential for complex workflows (order processing, onboarding flows, etc.).

## How to Run

```bash
cd src/Command.BackgroundJobsApi
dotnet run
```

**Swagger URL:** `https://localhost:7001/swagger` (or check console output for exact port)

## API Usage

### Send Email
```bash
POST /api/jobs/email
Content-Type: application/json

{
  "to": "user@example.com",
  "subject": "Welcome!",
  "body": "Your account has been created."
}
```

**Response:**
```json
{
  "jobId": "550e8400-e29b-41d4-a716-446655440000",
  "statusUrl": "/api/jobs/550e8400-e29b-41d4-a716-446655440000"
}
```

### Send SMS
```bash
POST /api/jobs/sms
Content-Type: application/json

{
  "phoneNumber": "+905551112233",
  "message": "Your verification code is 123456."
}
```

### Generate Report
```bash
POST /api/jobs/report
Content-Type: application/json

{
  "reportType": "sales",
  "from": "2026-01-01T00:00:00Z",
  "to": "2026-02-01T00:00:00Z"
}
```

### User Registered (Command Chaining)
```bash
POST /api/jobs/user-registered
Content-Type: application/json

{
  "email": "user@example.com",
  "phoneNumber": "+905551112233",
  "fullName": "Ahmet Yılmaz"
}
```

**Effect:** Creates 1 parent job + 2 child jobs (SMS + Email).

### Check Job Status
```bash
GET /api/jobs/{jobId}
```

**Response:**
```json
{
  "id": "550e8400-e29b-41d4-a716-446655440000",
  "status": 2,  // 0=Queued, 1=Processing, 2=Succeeded, 3=Failed
  "commandName": "SendEmailCommand",
  "error": null,
  "createdAt": "2026-02-02T10:30:00+00:00",
  "completedAt": "2026-02-02T10:30:01+00:00"
}
```
