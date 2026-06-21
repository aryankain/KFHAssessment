# KFH Loan Management System

Technical Assessment — KFH Bank

## Tech Stack
- .NET 8, ASP.NET Core Web API
- Blazor WebAssembly (Hosted)
- Entity Framework Core (Code First)
- SQL Server
- JWT Authentication

## Projects
| Project | Purpose |
|---|---|
| KFHAssessment.Server | API + EF Core + JWT + hosts client |
| KFHAssessment.Client | Blazor WASM frontend |
| KFHAssessment.Shared | DTOs and Enums shared between both |

## Setup

### Prerequisites
- .NET 8 SDK
- SQL Server (any edition)
- Visual Studio 2022

### Steps
1. Clone the repo
2. Update connection string in `KFHAssessment.Server/appsettings.json`
3. In Package Manager Console (KFHAssessment.Server selected):
4. Set KFHAssessment.Server as startup project
5. Press F5

### Default Login
- Username: `admin`
- Password: `Admin@1234`

## Business Rules
| Condition | Decision |
|---|---|
| Score >= 700 AND Amount <= 50,000 | Approved |
| Score 650–699 AND Amount <= 20,000 | Approved (mid-tier) |
| Everything else | Rejected |

## Assumptions
- JWT stored in localStorage for session persistence
- Users table added beyond base requirements to support auth
- AuditLogs table tracks all loan create/evaluate actions
- SQL schema is in `schema.sql`
- SQL queries for last is in `queries.sql`

## Architecture