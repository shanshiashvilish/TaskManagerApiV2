# Task Manager API V2

A robust, maintainable, and testable Web API for managing tasks and users, demonstrating strong adherence to architectural best practices, design patterns, and scalability considerations. 
Built using .NET 8, this project emphasizes clean architecture, dependency injection, and structured layering.

---

## 🚀 Technologies and Patterns Used

* **ASP.NET Core 8 Web API**
* **Entity Framework Core (InMemory)** for lightweight persistence and ease of debugging
* **Repository Pattern** to abstract data access
* **Dependency Injection (DI)** for loose coupling and testability
* **Layered Architecture:**

  * API Layer (Controllers and DTOs)
  * Application Layer (Services and business logic)
  * Domain Layer (Entities and interfaces)
  * Infrastructure Layer (Repositories, Data Context)
* **Background Services (IHostedService)** for automated task reassignment
* **Swagger (OpenAPI)** for API documentation and interactive testing
* **xUnit** for comprehensive unit testing
* **Moq** for mocking dependencies during tests

---

## ⚙️ Getting Started

### Prerequisites

* [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

### Running the API

* Clone the repository and navigate to the API project directory
* Run the API:

```bash
dotnet run
```

* Access Swagger UI for interactive API testing:

```
https://localhost:{port}/swagger
```

---

## 🧪 Running Unit Tests

The solution includes comprehensive unit tests using **xUnit** and **Moq**, covering critical business logic, repository functionality, and background services.

Run tests with:

```bash
dotnet test
```

### ✅ This command will:

* Build all solution projects
* Execute all unit tests
* Provide detailed results in the terminal

---

## 📌 Key Points

* **DB Seeder:** A database seeder is configured in `Program.cs`. It can be easily toggled or commented out for manual data entry.
* **Architecture:** Clearly defined project layers ensure maintainability, scalability, and easy understanding of the project structure.
* **Background Automation:** A sophisticated background service ensures tasks are dynamically reassigned following specified rules.

---
