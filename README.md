# 📦 Inventory Tracking System

A **RESTful Web API** application for centrally managing company inventory assets, employee/intern assignments, and maintenance processes.

---

## 🛠️ Tech Stack

| Layer | Technology |
|---|---|
| Framework | .NET 9 / ASP.NET Core Web API |
| ORM | Entity Framework Core 9 |
| Database | PostgreSQL |
| Cache | Redis (StackExchange.Redis) |
| Message Broker | RabbitMQ (MassTransit 8) |
| Search Engine | Elasticsearch 9 |
| Background Jobs | Hangfire (PostgreSQL storage) |
| Authentication | JWT Bearer Token |
| Object Mapping | AutoMapper 12 |
| Validation | FluentValidation |
| API Documentation | Swagger / OpenAPI |

---

## 🏗️ Architecture

The project follows **Clean Architecture** principles and is structured in 4 layers:

```
Envanter_Takip_Projesi/
├── Domain/                  # Core business rules (Entity, Enum)
├── Application/             # Application logic (Service, DTO, Interface)
├── Infra/                   # Infrastructure (Repository, DB, external services)
└── Envanter_Takip_Projesi/  # Presentation (Controller, Middleware, Extension)
```

### Layer Details

**Domain** — Pure business objects with no external dependencies:
- `Entities/`: InventoryItem, Assignment, Employee, Intern, Department, University, Maintenance, AppUser
- `Enums/`: ItemStatus, DegreeType, InternshipRole
- `Common/`: BaseEntity (Id, CreatedAt, UpdatedAt)

**Application** — Layer that orchestrates business rules:
- `Services/` & `Managers/`: Business logic implementations
- `Interfaces/`: Repository and service contracts
- `DTOs/`: Request/response data transfer objects
- `Validators/`: FluentValidation rules
- `Consumers/`: RabbitMQ event consumers
- `Messages/`: Event message models (InventoryAssignedEvent, MaintenanceStartedEvent)
- `Mappings/`: AutoMapper profiles

**Infra** — Technical infrastructure details:
- `Data/`: ApplicationDbContext, EF Core Configurations
- `Repositories/`: GenericRepository + UnitOfWork implementation
- `Services/`: RedisCacheService, ElasticSearchService, TokenService, HRReminderService
- `Migrations/`: EF Core database migration files

**Presentation** — HTTP layer:
- `Controllers/`: RESTful endpoints
- `Middlewares/`: Global error handling (UseCustomExceptionHandler)
- `Filters/`: ValidationFilter (FluentValidation integration)
- `Extensions/`: JWT and Swagger service registration extensions

---

## ✨ Features

- **Inventory Management** — Track product code, serial number, category, brand/model, warranty date; status management (Available / Assigned / Damaged / Under Maintenance / Retired)
- **Assignment System** — Assign inventory items to employees or interns; handle returns
- **Personnel Management** — Employee and intern CRUD; department and university associations
- **Maintenance Tracking** — Fault/maintenance records with cost and service provider info
- **Full-Text Search** — Elasticsearch-powered inventory search; sync endpoint for existing data
- **Redis Cache** — Caching for frequently read data
- **Event-Driven Architecture** — Publish assignment and maintenance events over RabbitMQ
- **Background Jobs** — HR reminder service via Hangfire
- **JWT Authentication** — Register / login / token management
- **Global Error Handling** — Centralized exception middleware with custom exception types (NotFoundException, ItemNotAvailableException, AssignmentConflictException, etc.)

---

## 📡 API Endpoints

### Inventory (`/api/InventoryItems`)
| Method | Endpoint | Description |
|---|---|---|
| GET | `/` | List all inventory items |
| GET | `/{id}` | Get inventory item by ID |
| GET | `/available` | List only available items |
| GET | `/search?keyword=` | Full-text search via Elasticsearch |
| POST | `/` | Create new inventory item |
| POST | `/sync-elastic` | Sync existing data to Elasticsearch |
| PUT | `/` | Update inventory item |
| DELETE | `/{id}` | Delete inventory item |

### Assignments (`/api/Assignments`)
| Method | Endpoint | Description |
|---|---|---|
| GET | `/` | List all assignments |
| GET | `/{id}` | Get assignment by ID |
| POST | `/` | Create new assignment |
| PUT | `/` | Update assignment |
| PUT | `/return` | Process item return |
| DELETE | `/{id}` | Delete assignment record |

### Other Resources
| Prefix | Description |
|---|---|
| `/api/Employees` | Employee CRUD |
| `/api/Interns` | Intern CRUD |
| `/api/Departments` | Department CRUD |
| `/api/Universities` | University CRUD |
| `/api/Maintenances` | Maintenance record CRUD |
| `/api/AppUsers` | Register / Login / Profile |

---

## 🚀 Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- PostgreSQL 14+
- Redis
- RabbitMQ
- Elasticsearch 9

### 1. Clone the Repository

```bash
git clone <repo-url>
cd Envanter_Takip_Projesi
```

### 2. Configure Connection Strings

Edit `Envanter_Takip_Projesi/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=envanterTakipDb;Username=postgres;Password=<PASSWORD>;",
    "RedisConnection": "localhost:6379",
    "RabbitMqConnection": "amqp://guest:guest@localhost:5672"
  },
  "JwtSettings": {
    "SecretKey": "<AT_LEAST_32_CHAR_SECRET_KEY>",
    "Issuer": "envanterTakipApi",
    "Audience": "envanterTakipClient",
    "ExpirationInMinutes": 60
  },
  "ElasticSearch": {
    "Url": "http://localhost:9200",
    "DefaultIndex": "inventory_items"
  }
}
```

> ⚠️ **Never commit** `appsettings.json` with real secrets to git history. Use `appsettings.Development.json` or [User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) for local development.

### 3. Apply Database Migrations

```bash
cd Envanter_Takip_Projesi
dotnet ef database update --project ../Infra
```

### 4. Run the Application

```bash
dotnet run --project Envanter_Takip_Projesi
```

The application starts at `https://localhost:7xxx` by default. The Swagger UI is available at `https://localhost:7xxx/swagger`.

---

## 🗄️ Database Schema

```
AppUsers ─────────────────────────────────────┐
Departments ──────────────┐                   │
Universities ──────┐      │                   │
                   │      │                   │
               Interns ───┤                   │
               Employees ─┴──── Assignments ──┤
                                              │
InventoryItems ──────────────── Assignments ──┘
      │
      └──── Maintenances
```

---

## ⚙️ Background Jobs (Hangfire)

The Hangfire dashboard is accessible at `/hangfire`.

Registered jobs:
- **HRReminderService** — HR notifications for upcoming intern deadlines and expiring assignments

---

## 🐇 Message Events (RabbitMQ)

| Event | Trigger | Consumer |
|---|---|---|
| `InventoryAssignedEvent` | When a new assignment is created | `InventoryAssignedEventConsumer` |
| `MaintenanceStartedEvent` | When a maintenance record is opened | `MaintenanceStartedEventConsumer` |

---

## 📁 Project Structure

```
Envanter_Takip_Projesi/
├── Domain/
│   ├── Common/BaseEntity.cs
│   ├── Entities/
│   └── Enums/
├── Application/
│   ├── Consumers/
│   ├── DTOs/
│   ├── Exceptions/
│   ├── Interfaces/
│   │   ├── Repositories/
│   │   └── Services/
│   ├── Managers/
│   ├── Mappings/
│   ├── Messages/
│   ├── Services/
│   └── Validators/
├── Infra/
│   ├── Data/
│   │   ├── Configurations/
│   │   └── Repositories/
│   ├── Migrations/
│   └── Services/
└── Envanter_Takip_Projesi/
    ├── Controllers/
    ├── Extensions/
    ├── Filters/
    ├── Middlewares/
    └── Properties/
```

---

