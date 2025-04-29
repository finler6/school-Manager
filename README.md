# School Management

A cross-platform **.NET 8** sample application for keeping track of students, subjects, classroom activities and evaluation results.  
The solution follows a clean, layered architecture (UI ⇢ BL ⇢ DAL) and ships with unit-tests plus an Azure Pipelines CI template.

---

## Main features
- **Cross-platform UI** — built with .NET MAUI (`SchoolManage.App`)  
  *Runs on Windows 10+, macOS (Catalyst) and Android/iOS when the corresponding workloads are installed.*
- **Entity-Framework Core 8** data layer with SQLite provider and demo-data seeders
- **Unit-of-Work & Repository** abstractions that keep business logic persistence-agnostic
- **Comprehensive unit tests** for DAL, BL and Common layers (xUnit)
- **CI pipeline** configured in `azure-pipelines.yml`

---

## Prerequisites
| Tool | Version | Notes |
|------|---------|-------|
| [.NET SDK](https://dotnet.microsoft.com/) | **8.0** or newer | `dotnet --list-sdks` |
| .NET MAUI workload | 8.0 | `dotnet workload install maui` |
| Visual Studio 2022 (v17.9+) *or* VS Code | — | with C# extensions |
| SQLite 3 (CLI) | optional | only if you want to inspect the DB file manually |

---

## Getting started

```bash
# 1. Restore solution
dotnet restore

# 2. Run unit tests
dotnet test

# 3. Build & launch the MAUI app (desktop)
dotnet build SchoolManage.App
dotnet run --project SchoolManage.App
```

The first run will create `school.db` (SQLite) in your local profile directory.  
To start with fresh demo data every launch, set `RecreateDatabaseEachTime = true`
in **`SchoolManage.DAL/Options/DALOptions.cs`**.

---

## Solution layout

```
school_managment/
├─ SchoolManage.App/        # .NET MAUI client (Views, ViewModels, XAML)
├─ SchoolManage.BL/         # Business logic layer, DTO mappers, services
├─ SchoolManage.DAL/        # EF Core entities, migrations, repositories
├─ SchoolManage.Common/     # Shared contracts, value objects, helpers
├─ *_Tests/                 # xUnit test projects for each layer
├─ docs/                    # Wire-frames, screenshots & specs
└─ azure-pipelines.yml      # CI definition
```

---

## Running database migrations manually

```bash
# Add a migration
dotnet ef migrations add <Name> --project SchoolManage.DAL

# Apply migrations & seed demo data
dotnet ef database update --project SchoolManage.DAL
```

*(Entity-Framework CLI is already referenced; no extra tools are needed.)*
