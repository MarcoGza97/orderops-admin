# OrderOps

OrderOps is a sample Order Management System built with ASP.NET Core 8 Razor Pages following Clean Architecture principles.

The project demonstrates:

* ASP.NET Core Razor Pages
* Entity Framework Core (Code First)
* SQL Server
* Dapper
* CQRS-inspired separation of read and write concerns
* FluentValidation
* Dependency Injection
* Repository Pattern
* NUnit Unit Testing
* EngineQuery integration for strongly typed SQL generation

## Architecture

```text
OrderOps.Admin
    ↓
Application
    ↓
Infrastructure
    ↓
SQL Server
```

### Layers

#### OrderOps.Admin

Presentation layer using Razor Pages.

Responsibilities:

* User interface
* Input binding
* Navigation
* Validation feedback

#### OrderOps.Application

Application layer containing:

* Services
* DTOs
* Requests
* Validators
* Contracts

Responsibilities:

* Business rules
* Use case orchestration
* Validation

#### OrderOps.Domain

Core business entities.

Examples:

* Product
* Customer
* Order
* OrderItem

#### OrderOps.Infrastructure

Data access and external concerns.

Technologies:

* Entity Framework Core
* Dapper
* EngineQuery

Responsibilities:

* Persistence
* Query execution
* Database transactions

---

## Features

### Products

* Create product
* Edit product
* Delete product
* Product listing
* Product details

### Customers

* Create customer
* Edit customer
* Delete customer
* Customer listing
* Customer details

### Orders

* Create order
* View order details
* Cancel order
* Automatic stock deduction
* Stock restoration on cancellation

### Reports

* Top selling products
* Sales by customer
* Low stock products

---

## Data Access Strategy

### Entity Framework Core

Used for:

* Writes
* Transactions
* Aggregate persistence

### Dapper

Used for:

* Read models
* Reporting queries
* Optimized projections

### EngineQuery

Used for:

* Strongly typed SQL generation
* EF Core metadata reuse
* Eliminating magic strings in simple read queries

---

## Testing

Frameworks:

* NUnit
* Moq

Coverage includes:

* ProductService
* CustomerService
* OrderService
* ReportService

---

## Running the Application

### Database

Update connection string in:

```json
appsettings.json
```

Run migrations:

```bash
dotnet ef database update
```

### Start application

```bash
dotnet run --project OrderOps.Admin
```

Open:

```text
https://localhost:xxxx
```

---

## Design Decisions

### Why Razor Pages?

The freelance opportunity requires Razor Pages experience.

The project intentionally uses Razor Pages instead of MVC or Blazor to demonstrate page-centric development.

### Why EF Core + Dapper?

EF Core provides:

* Change tracking
* Transactions
* Migrations

Dapper provides:

* Faster read operations
* Explicit SQL control
* Lightweight projections

### Why EngineQuery?

EngineQuery generates deterministic SQL using strongly typed expressions while reusing EF Core metadata.

---

## Future Improvements

* Docker support
* Health checks
* Structured logging
* Authentication and authorization
* Integration tests
* CI/CD pipeline
* EngineQuery aggregate expression support

---

## Author

Marco Garza

Backend .NET Developer

Technologies:

* ASP.NET Core
* SQL Server
* Azure
* Dapper
* Entity Framework Core
