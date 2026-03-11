# 🚀 TaskManagementAPI
TaskManagementAPI is a modern .NET 8 Web API designed for efficient management of users and their associated tasks. This project emphasizes clean code principles, robust architecture, and professional error-handling standards.

## 🏗 Architecture & Design Patterns
Clean Architecture: Implements a clear separation of concerns by decoupling the API layer from business logic and data access to ensure long-term maintainability.

Dependency Injection (DI): Leverages ASP.NET Core’s built-in DI container to manage service lifetimes, promoting loose coupling and easier testing.

Relational Data Modeling: Features a structured One-to-Many relationship between Users and Tasks, managed efficiently via Entity Framework Core.

## 🌟 Advanced Features
Global Exception Handling: Implements a centralized middleware system following the Problem Details (RFC 7807) standard to provide consistent and structured error responses.

Structured Logging: Utilizes ILogger to track application flow and operations, significantly reducing debugging time and improving monitoring.

Model Validation: Employs Data Annotations and custom validation logic to enforce strict rules on incoming requests, ensuring data integrity.


## 🛠 Tech Stack

Backend: .NET 8 / ASP.NET Core Web API

Database: SQL Server

ORM: Entity Framework Core

API Documentation: Swagger / OpenAPI


## 🚀 Getting Started
Clone the Repository: Clone this project to your local machine using the git clone command.

Configure Database: Update the connection string in appsettings.json to match your local SQL Server instance.

Apply Migrations: Run the dotnet ef database update command to sync the schema and create the necessary tables.

Run Application: Launch the API and explore the documentation and endpoints via the interactive Swagger UI.
