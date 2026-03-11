# 🚀 TaskManagementAPI

**TaskManagementAPI** is a modern .NET  Web API designed for efficient management of users and their associated tasks. This project emphasizes clean code principles, robust architecture, and professional error-handling standards.

---

## 🏗 Architecture & Design Patterns

● Clean Architecture
Implements a clear separation of concerns by decoupling the API layer from business logic and data access.

● Dependency Injection (DI)
Leverages ASP.NET Core’s built-in DI container to manage service lifetimes, promoting loose coupling.

● Relational Data Modeling
Features a structured **One-to-Many** relationship between Users and Tasks via Entity Framework Core.

---

## 🌟 Advanced Features

● Global Exception Handling
Implements a centralized middleware system following the **Problem Details (RFC 7807)** standard for structured error responses.



● Structured Logging
Utilizes `ILogger` to track application flow and operations, significantly reducing debugging time.

● Model Validation
Employs Data Annotations to enforce strict validation rules on incoming requests.

---

## 🛠 Tech Stack

Backend: .NET 8 / ASP.NET Core Web API
Database: SQL Server
ORM: Entity Framework Core
API Documentation: Swagger / OpenAPI

---

## 🚀 Getting Started

### 1. Clone the Repository
Clone the Repository:** Clone this project to your local machine using the `git clone` command.
* **Configure Database:** Update the connection string in `appsettings.json` to match your local SQL Server instance.
* **Apply Migrations:** Run the database update command to sync the schema and create the necessary tables.
* **Run Application:** Launch the API and explore the documentation and endpoints via Swagger UI.
