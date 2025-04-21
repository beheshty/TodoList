# TodoList API

A RESTful API for managing todo items, built with .NET Core following Domain-Driven Design (DDD) principles.

## Project Structure

The solution is organized into the following projects:

- **TodoList.API**: Web API project that handles HTTP requests
- **TodoList.Application**: Contains application services, interfaces, and DTOs
- **TodoList.Domain**: Contains domain entities, value objects, and domain logic
- **TodoList.Infrastructure**: Contains implementations of repositories and external services

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- Visual Studio 2022 or VS Code

### Running the Project

1. Clone the repository
2. Navigate to the solution directory
3. Run the following commands:

```bash
dotnet restore
dotnet build
dotnet run --project TodoList.API
```

## Architecture

This project follows Domain-Driven Design principles with a clean architecture approach:

- **Domain Layer**: Contains the core business logic and entities
- **Application Layer**: Contains use cases and application services
- **Infrastructure Layer**: Contains implementations of repositories and external services
- **API Layer**: Handles HTTP requests and responses

## License

This project is licensed under the MIT License. 