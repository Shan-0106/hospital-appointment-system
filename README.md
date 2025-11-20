# 🏥 Hospital Appointment Booking System - Backend

ASP.NET Core Web API for managing hospital appointments, doctors, and patients.

## 🚀 Features

- ✅ Doctor Management (CRUD operations)
- ✅ Patient Registration & Management
- ✅ Appointment Booking with conflict prevention
- ✅ RESTful API design
- ✅ Entity Framework Core with SQL Server
- ✅ Repository Pattern implementation
- ✅ Swagger documentation
- ✅ CORS enabled for frontend integration

## 🛠️ Tech Stack

- **Framework:** ASP.NET Core 8.0 Web API
- **Database:** SQL Server
- **ORM:** Entity Framework Core
- **Architecture:** Repository Pattern
- **API Documentation:** Swagger/OpenAPI

## 📦 Installation

### Prerequisites
- .NET 8.0 SDK
- SQL Server
- Visual Studio 2022

### Setup Steps

1. Clone the repository

git clone https://github.com/Shan-0106/hospital-appointment-system.git
cd hospital-appointment-system


2. Update connection string in `appsettings.json`

3. Run migrations

4. Run the application

API will be available at: `https://localhost:7163`

## 📚 API Endpoints

### Doctors
- `GET /api/Doctor` - Get all doctors
- `POST /api/Doctor` - Add new doctor

### Patients
- `GET /api/Patient` - Get all patients
- `POST /api/Patient` - Register new patient

### Appointments
- `GET /api/Appointment` - Get all appointments
- `POST /api/Appointment` - Book new appointment
- `DELETE /api/Appointment/{id}` - Cancel appointment

## 🔗 Frontend Repository

React.js frontend: https://github.com/Shan-0106/hospital-appointment-frontend

## 👨‍💻 Author

Shanmuganathan

