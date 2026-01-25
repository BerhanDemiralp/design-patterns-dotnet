# Factory Method Pattern — Payments API

This project demonstrates the **Factory Method Design Pattern** using a simple ASP.NET Core Web API that selects a payment provider (Stripe, PayPal, Iyzico) at runtime.

The goal is to show how object creation logic can be centralized and abstracted so that the rest of the system does not depend on concrete implementations.

---

## Problem

We want to process payments using different providers:

* Stripe
* PayPal
* Iyzico

The provider must be selected dynamically at runtime based on a request parameter.

Without a factory, the code would look like:

```csharp
if (provider == "stripe") new StripePaymentProvider();
else if (provider == "paypal") new PayPalPaymentProvider();
else if (provider == "iyzico") new IyzicoPaymentProvider();
```

This leads to:

* Tight coupling
* Violations of the Open/Closed Principle
* Difficult maintenance when adding new providers

---

## Solution: Factory Method Pattern

A factory class encapsulates the logic of selecting and creating the correct provider.

Common interface:

```csharp
public interface IPaymentProvider
{
    string Name { get; }
    PaymentResult Charge(PaymentRequest request);
}
```

Concrete implementations:

* `StripePaymentProvider`
* `PayPalPaymentProvider`
* `IyzicoPaymentProvider`

Factory:

* `PaymentProviderFactory`

The factory receives all provider implementations via Dependency Injection and selects the correct one based on the provider name.

---

## Architecture Overview

```
Client (Swagger / Frontend / Postman)
        |
        v
PaymentsController (HTTP Layer)
        |
        v
PaymentService (Business Layer)
        |
        v
PaymentProviderFactory (Creation Logic)
        |
        v
IPaymentProvider
   |        |         |
Stripe   PayPal    Iyzico
```

---

## How to Run

```bash
cd src/Factory.PaymentsApi
dotnet run
```

Open Swagger UI:

```
https://localhost:<port>/swagger
```

---

## API Usage

### Process Payment

```
POST /api/payments
```

Request body:

```json
{
  "provider": "stripe",
  "amount": 100,
  "currency": "USD"
}
```

Response:

```json
{
  "provider": "stripe",
  "transactionId": "a1b2c3d4...",
  "status": "success"
}
```

Error example:

```json
{
  "error": "Unknown provider: 'foo'. Valid: stripe, paypal, iyzico",
  "availableProviders": ["stripe", "paypal", "iyzico"]
}
```

---

## Key Files

| File                                 | Responsibility                |
| ------------------------------------ | ----------------------------- |
| `Providers/IPaymentProvider.cs`      | Provider interface            |
| `Providers/StripePaymentProvider.cs` | Stripe implementation         |
| `Providers/PayPalPaymentProvider.cs` | PayPal implementation         |
| `Providers/IyzicoPaymentProvider.cs` | Iyzico implementation         |
| `Factory/PaymentProviderFactory.cs`  | Factory Method implementation |
| `Services/PaymentService.cs`         | Business flow                 |
| `Controllers/PaymentsController.cs`  | HTTP endpoint                 |

---

## Adding a New Provider

1. Create a new class implementing `IPaymentProvider`
2. Give it a unique `Name`
3. Register it in `Program.cs`:

```csharp
builder.Services.AddScoped<IPaymentProvider, NewProvider>();
```

4. Call the API:

```
POST /api/payments
{
  "provider": "newprovider",
  "amount": 50,
  "currency": "USD"
}
```
