# OrderOps

OrderOps is a sample Order Management System built with ASP.NET Core 8 Razor Pages following Clean Architecture principles.

The project demonstrates a practical implementation of modern .NET development practices using Entity Framework Core, Dapper, FluentValidation, NUnit, and EngineQuery.

---

## Technical Highlights

- ASP.NET Core 8 Razor Pages
- SQL Server
- Entity Framework Core Code First
- EF Core Migrations
- Dapper Read Models
- EngineQuery Integration
- FluentValidation
- Dependency Injection
- NUnit
- Moq
- Clean Architecture
- Repository Pattern
- Lightweight CQRS Approach

---

## Architecture

```text
┌─────────────────────────────┐
│      OrderOps.Admin         │
│      (Presentation)         │
└─────────────┬───────────────┘
              │
              ▼
┌─────────────────────────────┐
│    OrderOps.Application     │
│   (Use Cases & Services)    │
└─────────────┬───────────────┘
              │
              ▼
┌─────────────────────────────┐
│      OrderOps.Domain        │
│   (Entities & Contracts)    │
└─────────────┬───────────────┘
              │
              ▼
┌─────────────────────────────┐
│  OrderOps.Infrastructure    │
│ (EF Core, Dapper, SQL)      │
└─────────────┬───────────────┘
              │
              ▼
┌─────────────────────────────┐
│         SQL Server          │
└─────────────────────────────┘
```

### Dependency Flow

Dependencies always point inward:

```text
Presentation → Application → Domain
Infrastructure → Domain
Infrastructure → Application (Contracts)
```

The Domain layer contains the core business concepts and remains independent from external frameworks and technologies.
```

### Layers

#### OrderOps.Admin

Presentation layer built with Razor Pages.

Responsibilities:

- User interface
- Request handling
- Navigation
- User interactions

#### OrderOps.Application

Application layer containing:

- Services
- DTOs
- Requests
- Validators
- Contracts

Responsibilities:

- Business rules
- Use case orchestration
- Validation
- Application workflows

#### OrderOps.Domain

Core business entities and concepts.

Examples:

- Product
- Customer
- Order
- OrderItem

#### OrderOps.Infrastructure

Infrastructure and persistence layer.

Technologies:

- Entity Framework Core
- Dapper
- EngineQuery
- SQL Server

Responsibilities:

- Data persistence
- Query execution
- Transactions
- External integrations

---

## Features

### Products

- Create product
- Edit product
- Delete product
- Product listing
- Product details

### Customers

- Create customer
- Edit customer
- Delete customer
- Customer listing
- Customer details

### Orders

- Create orders
- View order details
- Cancel orders
- Automatic stock deduction
- Stock restoration on cancellation

### Reports

- Top selling products
- Sales by customer
- Low stock products

---

## Data Access Strategy

### Entity Framework Core

Used for:

- Writes
- Transactions
- Change tracking
- Migrations
- Aggregate persistence

### Dapper

Used for:

- Read models
- Reporting queries
- Optimized projections
- Explicit SQL execution

### EngineQuery

Used for:

- Strongly typed query generation
- EF Core metadata reuse
- Eliminating magic strings
- Compile-time query safety

---

## Architectural Decisions

### Why Razor Pages?

Razor Pages was selected because the application is primarily focused on administrative workflows, forms, and CRUD operations.

For this type of system, the page-centric model provides a simpler structure than MVC while maintaining a clear separation between presentation and business logic.

The goal was to keep the solution easy to understand, maintain, and extend.

---

### Why Layered Architecture?

The solution is divided into four layers:

- Domain
- Application
- Infrastructure
- Presentation

Each layer has a specific responsibility:

- Domain contains business entities and core concepts.
- Application contains use cases, services, contracts, DTOs, and validation.
- Infrastructure contains persistence and external integrations.
- Presentation contains Razor Pages and UI concerns.

This separation reduces coupling and allows each layer to evolve independently.

---

### Why Entity Framework Core?

Entity Framework Core is primarily used for write operations.

Responsibilities include:

- Entity persistence
- Change tracking
- Transactions
- Database migrations
- Relationship management

For write scenarios, EF Core significantly reduces boilerplate code and simplifies aggregate persistence.

---

### Why Dapper?

Dapper is used for read operations and reporting queries.

Reasons:

- Explicit control over executed SQL
- Lightweight object mapping
- Optimized projections
- Better fit for reporting scenarios

The solution follows a lightweight CQRS-inspired approach:

- EF Core handles writes
- Dapper handles reads

This allows each tool to be used where it provides the most value.

---

### Why Not Use Only Entity Framework?

Entity Framework Core could handle all persistence requirements.

However, reporting and read-heavy scenarios often benefit from explicit SQL and optimized projections.

Using Dapper for reads provides greater control over query execution while keeping write operations simple through EF Core.

---

### Why Use Repositories?

Write repositories encapsulate persistence operations and prevent the Application layer from depending directly on Entity Framework.

Read repositories expose query contracts and allow the underlying implementation to evolve independently from business logic.

The goal is not to abstract Entity Framework completely, but to isolate infrastructure concerns from the Application layer.

---

### Why Not Implement Full CQRS?

The project adopts only the CQRS concepts that provide immediate value.

The solution separates:

- Read repositories
- Write repositories

However, it intentionally avoids introducing additional complexity such as:

- MediatR
- Event Sourcing
- Message Brokers
- Separate databases

For the current scope, a complete CQRS implementation would increase complexity without providing proportional benefits.

---

### Why EngineQuery?

The project integrates a custom NuGet package called EngineQuery.

EngineQuery generates SQL from strongly typed expressions while reusing Entity Framework metadata.

Benefits include:

- Elimination of magic strings
- Compile-time safety
- Safer refactoring
- Reuse of EF Core mappings
- Consistent query generation

Instead of writing raw SQL column names, queries can be generated directly from entity definitions.

---

### Why Do Some Queries Remain as Explicit SQL?

Not all queries were migrated to EngineQuery.

Reporting scenarios that require aggregate expressions such as:

```sql
SUM(Quantity * UnitPrice)
```

remain as explicit SQL statements.

Current EngineQuery aggregate support accepts direct column selectors but does not yet support aggregate operations over computed expressions.

Keeping those queries as explicit SQL improves readability and avoids unnecessary complexity.

---

### Validation Strategy

Validation is implemented in the Application layer using FluentValidation.

Benefits:

- Centralized validation rules
- Reusable validation logic
- UI-independent validation
- Consistent business behavior

Presentation-level validation is treated as a user experience concern, while Application-level validation is considered a business concern.

---

### Testing Strategy

Unit tests focus on:

- Application services
- Business rules
- Validation behavior

Infrastructure repositories are intentionally excluded from unit testing because their primary responsibility is coordinating framework components and database access.

Repository behavior is better validated through integration testing.

---

### Trade-Offs

The project intentionally prioritizes simplicity and maintainability over architectural purity.

Examples include:

- Razor Pages instead of SPA frameworks.
- Lightweight CQRS instead of full CQRS.
- EF Core + Dapper instead of a single persistence strategy.
- Explicit SQL for complex reporting queries.

The objective was to build a realistic solution that can evolve over time without introducing unnecessary complexity.

---

## Testing

Frameworks:

- NUnit
- Moq

Current coverage includes:

- ProductService
- CustomerService
- OrderService
- ReportService

Execute tests:

```bash
dotnet test
```

---

## Database

### Migrations

Apply migrations:

```bash
dotnet ef database update
```

### Connection String

Configure SQL Server connection in:

```json
appsettings.json
```

---

## Running the Application

Build:

```bash
dotnet build
```

Run:

```bash
dotnet run --project OrderOps.Admin
```

Open:

```text
https://localhost:<port>
```

---

## Future Improvements

- Docker support
- Health Checks
- Structured Logging
- Authentication & Authorization
- Integration Tests
- CI/CD Pipeline
- EngineQuery Aggregate Expression Support

---

## Author

Marco Garza

Backend .NET Developer

Technologies:

- ASP.NET Core
- Entity Framework Core
- Dapper
- SQL Server
- Razor Pages
- FluentValidation
- NUnit
