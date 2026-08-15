# Inventory Management System

A RESTful Inventory Management System API built with **ASP.NET Core 8 Web API**, following a layered architecture with **Domain, Application, Infrastructure, and Presentation** projects.

The API provides inventory, item, stock-operation, people, authentication, reporting, email, and PayPal subscription/payment functionality.

## Features

- User registration and authentication
- JWT access tokens and refresh tokens
- ASP.NET Core Identity with roles
- Email confirmation
- Forgot/reset password
- Rate limiting for authentication endpoints
- Inventory management
- Item management
- People/supplier/customer management
- Stock-in / stock-out operations
- Inventory-to-inventory transfer operations
- Inventory and item reports
- Stored and expired item reports
- Item movement reports
- PayPal subscription/payment integration
- Email sending through SMTP
- Entity Framework Core with SQL Server
- Repository and Unit of Work patterns
- CQRS using MediatR
- FluentValidation
- AutoMapper
- Swagger / OpenAPI
- Audit logging middleware
- Security headers middleware
- Database seeding
- Pagination for list endpoints

## Architecture

The solution is divided into four projects:

```text
InventoryManagementSystem/
├── Domain/
│   ├── Entities/
│   ├── Filters/
│   ├── Interfaces/
│   └── Models/
│
├── Application/
│   ├── Common/
│   ├── DTOs/
│   ├── Features/
│   ├── Interfaces/
│   └── DependencyInjection.cs
│
├── Infrastructure/
│   ├── Data/
│   ├── Identity/
│   ├── Migrations/
│   ├── Repositories/
│   ├── Services/
│   ├── UnitOfWork/
│   └── DependencyInjection.cs
│
└── Presentation/
    ├── Controllers/
    ├── Middlewares/
    ├── Program.cs
    ├── appsettings.json
    └── Properties/
```

### Domain

Contains the core business entities and abstractions:

- `Inventory`
- `InventoryItem`
- `Item`
- `Operation`
- `Person`
- `Payment`
- Repository interfaces
- Unit of Work interface
- Operation filters and domain models

### Application

Contains application use cases and business workflows.

The project uses **CQRS with MediatR**, separating commands from queries.

Examples:

- Create/update/delete inventory
- Create/update/delete item
- Create/update/delete person
- Create/update/delete operation
- Transfer items between inventories
- Generate inventory reports
- Generate item reports
- Generate stored/expired item reports
- Generate item movement reports
- Send emails
- Create/capture/check payment orders

FluentValidation validators are used to validate commands and queries before processing.

### Infrastructure

Contains implementations for external and persistence concerns:

- Entity Framework Core
- SQL Server
- ASP.NET Core Identity
- JWT token service
- Repository implementations
- Unit of Work
- Email service
- PayPal service
- Database migrations
- Database seeding

### Presentation

Contains the ASP.NET Core Web API layer and HTTP endpoints.

Main controllers:

- `UsersController`
- `InventoriesController`
- `ItemsController`
- `OperationsController`
- `PeopleController`

## Technology Stack

| Technology | Purpose |
|---|---|
| .NET 8 | Application framework |
| ASP.NET Core Web API | REST API |
| C# | Programming language |
| Entity Framework Core 8 | ORM |
| SQL Server | Database |
| ASP.NET Core Identity | User and role management |
| JWT Bearer | API authentication |
| MediatR | CQRS / request handling |
| FluentValidation | Request validation |
| AutoMapper | DTO mapping |
| MailKit | SMTP email |
| PayPal API | Subscription/payment processing |
| Swagger / Swashbuckle | API documentation |

## Requirements

Before running the project, install:

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server 2019+ or another compatible SQL Server installation
- SQL Server Management Studio or Azure Data Studio (optional)
- Visual Studio 2022 or Visual Studio Code
- A PayPal Sandbox account if payment functionality is required
- A Gmail/SMTP account if email functionality is required

## Database Configuration

The project uses SQL Server through Entity Framework Core.

The default development connection string targets SQL Server Express:

```text
Server: .\SQLEXPRESS
Database: InventoryManagementSystemDb
Authentication: Windows Authentication
```

Configure the connection string in:

```text
Presentation/appsettings.json
```

Example:

```json
{
  "ConnectionStrings": {
    "InventoryManagementSystem": "Data Source=.\\SQLEXPRESS;Initial Catalog=InventoryManagementSystemDb;Integrated Security=true;Trusted_Connection=true;TrustServerCertificate=true"
  }
}
```

> **Security:** Do not commit real database passwords, JWT signing keys, SMTP passwords, or PayPal secrets to source control. Use environment variables, User Secrets, or another secure secrets-management solution.

## Configuration

The API uses the following configuration sections:

### JWT

```json
"Jwt": {
  "Key": "YOUR_SECRET_KEY",
  "Issuer": "http://localhost:5145",
  "Audience": "http://localhost:4200",
  "ExpiryInMinutes": 60
}
```

Use a long, randomly generated signing key in real environments.

### Email

```json
"EmailSettings": {
  "Server": "smtp.gmail.com",
  "Port": 587,
  "SenderEmail": "your-email@example.com",
  "SenderName": "YOUR_APP_NAME",
  "Password": "YOUR_APP_PASSWORD"
}
```

For Gmail, use an [App Passwords](https://myaccount.google.com/apppasswords) and create app to use app name and password in EmailSettings.

### PayPal

```json
"PayPal": {
  "ClientId": "YOUR_PAYPAL_CLIENT_ID",
  "ClientSecret": "YOUR_PAYPAL_CLIENT_SECRET",
  "BaseUrl": "https://api-m.sandbox.paypal.com"
}
```

The included configuration is intended for PayPal Sandbox usage.

## Installation

Clone the repository and enter the solution directory:

```bash
git clone <repository-url>
cd InventoryManagementSystem
```

Restore NuGet packages:

```bash
dotnet restore
```

Build the solution:

```bash
dotnet build
```

## Database Migration

The repository already contains an Entity Framework Core migration under:

```text
Infrastructure/Migrations/
```

If the database has not been created, apply the migrations with:

```bash
dotnet ef database update --project Infrastructure --startup-project Presentation
```

If the EF CLI is not installed:

```bash
dotnet tool install --global dotnet-ef
```

To create a new migration after changing the entities:

```bash
dotnet ef migrations add YourMigrationName --project Infrastructure --startup-project Presentation
```

Then update the database:

```bash
dotnet ef database update --project Infrastructure --startup-project Presentation
```

## Running the API

From the solution directory:

```bash
dotnet run --project Presentation
```

The development launch profiles include:

```text
HTTP:  http://localhost:5145
HTTPS: https://localhost:7261
```

Swagger is available in the Development environment at:

```text
http://localhost:5145/swagger
```

or:

```text
https://localhost:7261/swagger
```

## Authentication

Most inventory-related endpoints require a JWT access token.

After successful login, the API returns:

- Access token
- Refresh token

For protected requests, send:

```http
Authorization: Bearer YOUR_ACCESS_TOKEN
```

Swagger can be configured with the JWT token using the **Authorize** button.

## User API

Base route:

```text
/api/Users
```

| Method | Endpoint | Description |
|---|---|---|
| POST | `/register` | Register a new user |
| POST | `/login` | Authenticate a user |
| POST | `/verification` | Confirm email |
| GET | `/currentUserInfo` | Get current authenticated user |
| PUT | `/accountUpdate` | Update account information |
| DELETE | `/{id}` | Delete a user |
| POST | `/refresh-token` | Generate a new access token |
| POST | `/forgot-password` | Request password reset |
| POST | `/reset-password` | Reset password |
| POST | `/capture-payment` | Capture PayPal payment and activate subscription |

Authentication endpoints are protected with a fixed-window rate limiter. The login endpoint currently allows up to **5 requests per minute** per limiter configuration.

## Inventory API

Base route:

```text
/api/Inventories
```

The inventory API supports:

- Get inventories with pagination
- Get an inventory by ID
- Create inventory
- Update inventory
- Delete inventory
- Get inventory IDs
- Generate an inventory report

Example:

```http
GET /api/Inventories?pageNumber=1&pageSize=10
```

## Item API

Base route:

```text
/api/Items
```

The item API supports:

- Get items with pagination
- Get an item by ID
- Create item
- Update item
- Delete item
- Get items by a list of inventory IDs
- Generate item reports
- Generate stored-item reports
- Generate expired-item reports
- Generate item movement reports

Examples:

```http
GET /api/Items?pageNumber=1&pageSize=10
```

```http
GET /api/Items/storage/{inventoryId}/{daysCount}/{monthsCount}/{yearsCount}
```

```http
GET /api/Items/expired/{inventoryId}/{daysCount}/{monthsCount}/{yearsCount}
```

## Operations API

Base route:

```text
/api/Operations
```

Operations represent inventory movements and support:

- Get operations with pagination
- Get an operation by ID
- Create an operation
- Update an operation
- Delete an operation
- Create transfer operations
- Filter operations by inventory
- Filter operations by operation type

Example:

```http
POST /api/Operations/transfer
```

## People API

Base route:

```text
/api/People
```

The People API supports:

- Get people with pagination
- Get a person by ID
- Create person
- Update person
- Delete person
- Filter people by type

## Core Data Model

The main relationships are:

```text
ApplicationUser
    │
    ├── People
    ├── Inventories
    ├── Items
    ├── Operations
    └── Payments


Inventory ─────── InventoryItem ─────── Item
    │                                      │
    └────────────── Operation ─────────────┘
                         │
                       Person
```

### Inventory

Represents a physical inventory/warehouse and contains:

- Name
- Address
- Manager
- Items
- Operations
- Application user ownership

### Item

Represents a product/item managed by the system and contains:

- Name
- Code
- Size
- Inventory relationships
- Operation relationships

### InventoryItem

Represents the quantity of an item stored in an inventory.

It links:

```text
Inventory + Item + Quantity
```

### Operation

Represents an inventory movement and contains:

- Date
- Quantity
- Operation type
- Production date
- Expiry date
- Item
- Person
- Inventory

### Person

Represents a person involved in inventory operations, such as a supplier or customer, depending on the configured type.

### Payment

Stores subscription/payment information:

- Payment ID
- Amount
- Status
- Creation date
- Application user

## Design Patterns and Practices

The project uses several common architectural patterns:

### Clean/Layered Architecture

Business logic is separated from infrastructure and HTTP concerns.

### CQRS

MediatR is used to separate:

- Commands — operations that change data
- Queries — operations that retrieve data

### Repository Pattern

Repository abstractions are defined in the Domain project and implemented in Infrastructure.

### Unit of Work

`IUnitOfWork` coordinates repository operations and persistence.

### Dependency Injection

Services and repositories are registered through dependency-injection extension methods.

### DTOs

DTOs are used to control the data exposed by the API instead of returning domain entities directly.

### Validation Pipeline

FluentValidation is integrated through a MediatR validation behavior.

### Middleware

The API includes custom middleware for:

- Audit logging
- Security headers
- Exception/validation handling

## Database Seeding

On application startup, the API calls the database helper to seed initial data, including Identity roles/users and application data where configured.

The relevant implementation is:

```text
Infrastructure/Data/Helper/ApplicationDbHelper.cs
```

## CORS

The current development configuration allows:

```text
Any origin
Any header
Any HTTP method
```

For production, replace this with a restricted list of trusted frontend origins.

## Production Recommendations

Before deploying the API to production:

1. Move secrets out of `appsettings.json`.
2. Rotate any credentials that may have been committed previously.
3. Use a strong randomly generated JWT signing key.
4. Restrict CORS to trusted frontend domains.
5. Use HTTPS only.
6. Configure production PayPal credentials and endpoints.
7. Configure a production SMTP provider.
8. Disable development Swagger if it is not required.
9. Review rate-limiting policies.
10. Configure proper logging and monitoring.
11. Use a production SQL Server database.
12. Store sensitive configuration in environment variables, Azure Key Vault, AWS Secrets Manager, or another secrets-management solution.

## Project Structure Summary

```text
Domain
  ↓
Application
  ↓
Infrastructure
  ↓
Presentation
```

Dependencies are organized so that the core domain remains independent of infrastructure and presentation concerns.

## Useful Commands

Restore dependencies:

```bash
dotnet restore
```

Build:

```bash
dotnet build
```

Run:

```bash
dotnet run --project Presentation
```

Run with a specific environment:

```bash
dotnet run --project Presentation --environment Development
```

Create migration:

```bash
dotnet ef migrations add MigrationName --project Infrastructure --startup-project Presentation
```

Apply migrations:

```bash
dotnet ef database update --project Infrastructure --startup-project Presentation
```

## License

This project does not currently specify a license. Add a `LICENSE` file if you intend to distribute the project publicly.

## Author

**Muhammad Samy Saber**

.NET Developer | ASP.NET Core | C# | SQL Server
