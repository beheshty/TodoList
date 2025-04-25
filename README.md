# TodoList API

A robust, modular, and production-ready RESTful API for managing todo items, built with **.NET 8.0**, Domain-Driven Design (DDD), and Clean Architecture.

---

## 📁 Project Structure

```
src/
  TodoList.API/           # ASP.NET Core Web API (Controllers, Middleware, Models, Extensions, Mappers)
    Controllers/          # API endpoints (TodoItemsController)
    Middleware/           # Global exception handler
    Models/               # Request/response models (Create, Update, Get)
    Extensions/           # Service registration (OpenTelemetry, etc.)
    Mappers/              # Mapperly mapping profiles
  TodoList.Application/   # Application logic (CQRS: Commands, Queries, Handlers, DTOs, Interfaces)
    Commands/             # Command handlers for create, update, delete, status
    Queries/              # Query handlers for get operations
  TodoList.Domain/        # Core business logic (Entities, Value Objects, Repositories, Domain Events)
    Entities/             # TodoItem, TodoItemStatus
    Repositories/         # ITodoItemRepository
  TodoList.Infrastructure # Persistence (EF Core), Unit of Work, Migrations, Repositories, Mediator
    Data/                 # DbContext, database initialization
    Migrations/           # EF Core migrations
    Repositories/         # Repository implementations
    UnitOfWork/           # Unit of Work pattern
    Mediator/             # Mediator pattern implementation
tests/
  TodoList.Application.Tests/ # Unit tests for Application layer
```

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- Visual Studio 2022+ or VS Code
- Docker (optional, for containerized deployment)

### Running the API

```bash
git clone <repository-url>
cd TodoList
dotnet restore
dotnet build
dotnet run --project src/TodoList.API
```

The API will start on `https://localhost:5001` (or as configured).

### Running with Docker

```bash
docker-compose up --build
```

---

## 🧠 Architecture Overview

- **API Layer**: HTTP endpoints, request/response models, middleware, and mapping.
- **Application Layer**: Use cases, CQRS (Commands/Queries), interfaces, and DTOs.
- **Domain Layer**: Aggregates, entities, value objects, domain events, and business rules.
- **Infrastructure Layer**: EF Core persistence, repositories, migrations, Unit of Work, Mediator, and external services.

---

## ⚙️ Features

- Modular Clean Architecture with DDD
- CRUD operations for Todo items
- Global exception handling middleware
- Automatic Unit of Work for transaction management
- Mapperly for fast, type-safe object mapping
- Docker support for containerized deployment
- API documentation with Swagger (OpenAPI)
- OpenTelemetry for distributed tracing, metrics, and structured logging
- Unit tests (xUnit, Moq)
- Extensible for future enhancements

---

## 🛠️ Configuration

- **App settings**: `src/TodoList.API/appsettings.json`
- **Database**: Configured via EF Core in `src/TodoList.Infrastructure/Data/TodoListDbContext.cs`
- **Dependency Injection**: All services registered in `Program.cs`
- **Swagger**: Enabled by default for API documentation at `/swagger`
- **OpenTelemetry**: Configurable via environment variables or `appsettings.json`
- **CORS**: Configurable in `Program.cs`
- **JWT Authentication**: Ready for stateless APIs (add your configuration)

---

## 🧪 Testing

- Run all tests:
  ```bash
  dotnet test
  ```
- Tests are organized by layer in the `tests/` directory.

---

## 🛠️ Deployment

- **Docker**: Use the provided `Dockerfile` and `docker-compose.yml` for containerized deployment.
- **Migrations**: EF Core migrations are located in `src/TodoList.Infrastructure/Migrations`.

---

## 🌐 Observability

- **OpenTelemetry**: Tracing, metrics, and logging are integrated via `OpenTelemetryExtensions`.
  - Configure OTLP endpoint via `OTEL_EXPORTER_OTLP_ENDPOINT` or `appsettings.json`.
  - Console and OTLP exporters supported.

---

## 🤝 Contributing

Contributions are welcome! Please open issues or submit pull requests.

---

**Note:**  
- All code follows .NET best practices, Clean Architecture, and DDD principles.
- For more details, see the source code and XML comments.
