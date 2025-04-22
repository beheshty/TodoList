# TodoList API
A clean and modular RESTful API for managing todo items, built with .NET 8.0 using Domain-Driven Design (DDD) and Clean Architecture principles.

---

## 🔧 Project Structure
The solution is structured into the following projects:

TodoList.API: Entry point. Exposes HTTP endpoints via ASP.NET Core.

TodoList.Application: Application logic, use cases, DTOs, interfaces.

TodoList.Domain: Core business logic – aggregates, entities, value objects.

TodoList.Infrastructure: Persistence (e.g., EF Core) and external service integrations.

---

## 🚀 Getting Started
Prerequisites
.NET 8.0 SDK

Visual Studio 2022 (or later) / VS Code

Running the API
bash
Copy
Edit
git clone <repository-url>
cd TodoList
dotnet restore
dotnet build
dotnet run --project TodoList.API
The API will start on https://localhost:5001 or similar, depending on your launch profile.

---

## 🧠 Architecture
This project is built with separation of concerns in mind:

Domain Layer: Pure domain model, aggregates, and business rules.

Application Layer: Use cases, interfaces, service contracts.

Infrastructure Layer: External concerns (e.g., data access, messaging).

API Layer: Presentation layer that handles HTTP and delegates logic to application services.

---

## 📌 Roadmap / Upcoming Enhancements
The following features and improvements are in progress or planned:

🐳 Docker support – Enable containerized deployment

✏️ Update & Delete a Todo task – Extend CRUD operations

♻️ Service registration refactor – Improve DI setup for modularity and scalability

⚙️ Automatic Unit of Work – Streamline transaction management across repositories
