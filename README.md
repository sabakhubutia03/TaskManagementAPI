# 📝 Task Management API (.NET 9)

A **robust, scalable, and professional Backend REST API** built with **.NET 9**, strictly following **Clean Architecture principles** and **Unit Testing** best practices.

This project manages **users and tasks**, focusing on **Dependency Injection, data integrity, and high-quality error handling**.

---

## 🚀 Key Features

### 👤 User Management
- Full CRUD operations for users.
- Secure profile handling and data validation.
- Decoupled architecture using DTOs for safe data transfer.

---

### 📋 Task & Assignment System
- Complete Task management (Create, Read, Update, Delete).
- Tasks are linked to specific users (One-to-Many relationship).
- Status tracking to manage task completion effectively.

---

### 🛡 Error Handling & Logging
- **Global Exception Middleware:** Centralized error handling for consistent API responses.
- **Custom Exceptions:** Domain-specific exceptions (e.g., `ApiException`) for precise error reporting.
- **Structured Logging:** Integrated logging for monitoring service-level events and failures.

---

### 🧪 Quality Assurance (Unit Testing)
- Comprehensive test suite using **xUnit** and **Moq**.
- Tests for all core services (`TaskService`, `UserService`).
- Covers success paths, validation failures, and "Not Found" edge cases.

---

## 🛠 Tech Stack & Highlights

- **Framework:** ASP.NET Core Web API (.NET 9)
- **Database:** SQL Server (Support for InMemory for testing)
- **ORM:** Entity Framework Core
- **Testing:** xUnit & Moq
- **Design Patterns:** Repository Pattern, Dependency Injection, Middleware

---

## 🧠 Architecture & Design

The project follows **Clean Architecture**, ensuring a strict separation of concerns and long-term maintainability.

### 🏗 Layers

- **📂 Domain** Core business logic, Entities (`User`, `TaskItem`), and Custom Exceptions.

- **📂 Application** Service Interfaces (`ITaskService`, `IUserService`) and business logic abstractions.

- **📂 Infrastructure** Data Access (`ApplicationDbContext`), Repository implementations, and Service logic.

- **📂 API** REST Controllers, Global Middleware, and Configuration.

---

## 🚦 Getting Started

1. **Clone the repository:**
   ```bash
   git clone [https://github.com/sabakhubutia03/TaskManagementAPI.git](https://github.com/sabakhubutia03/TaskManagementAPI.git)
