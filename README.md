# ClinicBookingSystem

A backend Clinic Booking System API; Doctors create timeslots for availability, Patients book Appointments, and Admins oversee the system.
Built to demonstrate clean architecture, DDD principles, and role-based access control in .NET 10.

> The UI in `wwwroot` is a minimal page for easy demonstration, the project also runs Scalar; however the core focus is the backend.

---

## Highlights

- **Layered architecture respecting dependency flow**: `Presentation` → `Features` → `Domain` → `Infrastructure`
- **JWT authentication and role-based authorization** (Patient / Doctor / Admin)
- **Database Transactions for booking** with optimistic concurrency
- **Rich Domain model** with business logic methods in domain (ex: `Timeslot.MarkAsBooked`)
- **Repository + Unit of Work patterns**
- **Global exception handler** to return `ProblemDetails`
- **FluentValidation** for input validation
- **AutoMapper** for DTO mapping
- **Pagination support** for timeslot browsing

---
## Core Features

### Authentication
- Register and login endpoints
- JWT generation with claims

### Doctor Management
- Admin creates doctor profiles
- Doctors create timeslots
- View doctors by specialty
- View doctor availability

### Appointments
- Patients book appointments
- Patients cancel appointments
- Doctors view their appointments
- Patients view their appointments

---

## Architecture Overview
Presentation (Controllers, Middleware) 
Features (Services, DTOs, Validators) 
Domain (Entities, Exceptions, Interfaces) 
Infrastructure (EF Core, Repositories, DbContext)

---

## Tech Stack

- **.NET 10** (C# 14)
- **ASP.NET Core Web API**
- **Entity Framework Core** (SQL Server)
- **AutoMapper**
- **FluentValidation**
- **Scalar** 

---

## Prerequisites

- **.NET 10 SDK**
- **SQL Server** (local or Docker)
- **EF Core tools**

---

## Configuration

Add the following to `appsettings.json` or user-secrets:
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ClinicBookingSystem;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "Jwt": {
    "Issuer": "ClinicBookingSystem",
    "Audience": "ClinicBookingSystem",
    "Key": "your-dev-secret-key"
  },
  "Cors": {
    "AllowedOrigins": ["https://localhost:7003"]
  }
}

---

## Run Locally
```
# 1. Clone the repo
git clone https://github.com/your-username/ClinicBookingSystem.git
cd ClinicBookingSystem

# 2. Configure secrets for JWT tokenization (edit appsettings.json)
dotnet user-secrets set "Jwt:Key" "your-dev-secret-key"

# 3. Apply migrations
dotnet ef database update

# 4. Run
dotnet run --project ClinicBookingSystem.API
```
> Scalar UI will be available at https://localhost:7003/scalar 
> the minimal demo page will be at https://localhost:7003/index.html

---

## Example Endpoints

- `POST /api/Auth/register`
- `POST /api/Auth/login`
- `POST /api/Doctor/Create-Timeslot`
- `GET /api/Doctor/View-Doctors`
- `POST /api/Appointment/Book-Appointment`
- `PUT /api/Appointment/Cancel/{appointmentId}`

---

## Notes for Reviewers

This project is mainly a demonstration of backend architecture, domain design, and API design patterns. 
Automated tests are not included in this version. Focus was on architecture and design patterns, Unit Testing will be added soon.
