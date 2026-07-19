# ProductManagement

## What it is
ProductManagement is a .NET Web API built using **Clean Architecture**. The solution is split into 4 projects, each with one clear responsibility:

- **API** — entry point, handles HTTP
- **Application** — use cases / business workflows
- **Domain** — core business rules
- **Infrastructure** — database and external tech details

## Why Clean Architecture
In a typical, non-layered app, everything is mixed together: business rules, database code, and HTTP handling all live in the same class. This works at first, but becomes a mess as the app grows — a change to the database can break business logic, and you can't test a business rule without a real database running.

Clean Architecture solves this by giving each concern its own layer, with a strict rule: **dependencies only point inward, toward Domain.**

```
API  →  Infrastructure  →  Application  →  Domain
```

Domain depends on nothing else in the system. Every other layer depends on the layer(s) closer to Domain, never the other way around.

## The 4 layers

**Domain**
Pure C# — no frameworks, no database, no HTTP. Contains:
- Entities (e.g. `Product`) — self-validating, so an invalid `Product` can never exist in memory
- Value Objects (e.g. `Money(Amount, Currency)`) — no identity of their own, compared by value
- Domain Exceptions (e.g. `InsufficientStockException`) — named, specific business rule violations

**Application**
Defines *what* the system can do, without knowing *how* it's actually done:
- Interfaces like `IProductRepository`, `IUnitOfWork` — contracts Infrastructure must fulfill
- Commands (write) and Queries (read) — handled via MediatR (CQRS)
- DTOs (e.g. `ProductDto`) — the shape returned to the outside world, not the raw entity
- `Result<T>` — makes expected failures (like bad input) explicit, instead of throwing exceptions

**Infrastructure**
Implements the contracts Application defined — the real EF Core + database code lives here. Application never needs to know which database engine is behind `IUnitOfWork`.

**API**
Thin controllers only: parse the HTTP request → build a MediatR command/query → send it → map the result to an HTTP response (e.g. `201 Created`, `404 Not Found`). No business logic lives here.

## How a request flows (example: creating a product)
```
POST /api/v1/products
  → Controller builds CreateProductCommand
  → MediatR routes it to CreateProductCommandHandler
  → Handler validates input, builds a Product (Domain), saves via IUnitOfWork
  → Returns Result<Guid> back up to the Controller
  → Controller returns 201 Created (or 400 with the error)
```

The Handler never knows it was called from HTTP — it would run identically from a console app or a test.

## How to run

```bash
dotnet build
dotnet run --project ProductManagement.Api
dotnet test
```

Swagger UI: `https://localhost:{port}/swagger`

## Architecture diagram

```
┌─────────────┐
│     API     │  (Controllers — thin, HTTP only)
└──────┬──────┘
       │ depends on
       ▼
┌─────────────────┐
│ Infrastructure   │  (EF Core, DB — implements Application's interfaces)
└──────┬───────────┘
       │ depends on
       ▼
┌─────────────────┐
│  Application     │  (Commands, Queries, Interfaces, DTOs, Result<T>)
└──────┬───────────┘
       │ depends on
       ▼
┌─────────────────┐
│    Domain        │  (Entities, Value Objects, Domain Exceptions)
│ (depends on NOTHING) │
└─────────────────┘
```