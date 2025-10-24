# 🚀 Mars Logistics API

A .NET 8 Web API MVP for tracking parcel deliveries from Earth to Mars. Built for rapid prototyping, this solution supports parcel registration, status updates, and delivery history retrieval — with launch scheduling and audit trails.

---

## 🛠️ Setup Instructions

### Prerequisites
- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- Visual Studio 2022+ or VS Code

### Run the API
```bash
dotnet run
```
Once running, open:
```
https://localhost:{port}/swagger
```
to access the Swagger UI for testing endpoints.

### Run Unit Tests
```bash
dotnet test MarsLogisticsTests
```
Ensure you're in the correct folder or pass the full `.csproj` path if needed.

---

## 🧠 Design Choices & Trade-offs

### ✅ InMemory Database
- **Trade-off**: Fast and simple for prototyping, but not persistent.
- **Reason**: Avoided external dependencies to focus on core logic and testability.

### ✅ Async/Await Everywhere
- **Trade-off**: Slight overhead for simple operations.
- **Reason**: Prepares the codebase for scalable, I/O-bound workloads.

### ✅ Status Transition Validation
- **Trade-off**: Hardcoded rules for now.
- **Reason**: Used a dictionary-based rule engine for clarity and extensibility.

### ✅ `[Owned]` Entity for ParcelHistory
- **Trade-off**: Embedded structure limits querying flexibility.
- **Reason**: Keeps audit trail tightly coupled with Parcel, ideal for value-object semantics.

---

## 🧭 Solution Walkthrough

### 📦 Parcel Registration
- Validates barcode format (`RMARS` + 19 digits + 1 capital letter).
- Assigns launch date based on delivery service:
  - **Express**: 1st Wednesday of each month, ETA 90 days
  - **Standard**: Next launch 2025-10-01, ETA 180 days
- Initializes status as `Created` and logs history.

### 🔄 Status Updates
- Validates transitions using a rule dictionary.
- Appends to `ParcelHistory` with timestamp.

### 🔍 Parcel Retrieval
- Returns full parcel details including audit trail.
- Uses EF Core InMemory for fast access.

### 🧪 Testing
- Unit tests cover:
  - Barcode validation
  - Status transitions
  - Launch date logic
- Test project: `MarsLogisticsTests`

---

## 🔮 Enterprise-Scale Improvements

| Area              | Suggested Upgrade                          |
|-------------------|--------------------------------------------|
| Persistence       | Switch to SQL Server or PostgreSQL         |
| Time Control      | Inject `IClock` for testable time logic    |
| Validation        | Use FluentValidation for model rules       |
| API Security      | Add JWT authentication and role-based access |
| Observability     | Add Serilog, OpenTelemetry, and health checks |
| Filtering         | Add query parameters to `GET /parcels`     |
| CI/CD             | Add GitHub Actions or Azure Pipelines      |

---

## ⚙️ Assumptions & Shortcuts

- **Launch dates** are hardcoded for simplicity.
- **Status transitions** are manually defined in a dictionary.
- **No authentication** — assumed internal API for now.
- **No pagination or filtering** — not needed for MVP scale.
- **ParcelHistory** is embedded (`[Owned]`) for simplicity.
- **No integration tests yet** — focus was on core logic and unit tests.

---

## 🧱 Middleware & Exception Handling
- The solution includes a custom middleware for centralized exception handling. 
- This ensures that unexpected errors are caught and returned as consistent, structured responses — improving API reliability 
- and client-side debugging. It replaces default error pages with JSON error objects and logs exceptions for future diagnostics. 
- This approach simplifies controller logic and prepares the codebase for scalable observability and monitoring.

---

## 🤖 AI Usage Disclosure

I used Microsoft Copilot to assist with:
- Structuring this README
- Suggesting unit test outlines
- Writing automated test cases

All core logic — including status transitions, barcode validation, launch scheduling, and async refactoring — was written and validated manually.
