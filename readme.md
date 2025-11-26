# TakeHomeAssignment.General.AccountAPI

![.NET](https://img.shields.io/badge/.NET-6-blue)
![Status](https://img.shields.io/badge/status-active-success)

A **JWT-based authentication API** built with **.NET 6**, featuring user registration, login, and a protected endpoint with configurable token expiration.

---

## Table of Contents

- [Features](#features)
- [Environment Variables](#environment-variables)
- [Running the Project](#running-the-project)
- [API Endpoints](#api-endpoints)
- [Notes](#notes)

---

## Features

- **Register**: Create a new user (email + password).  
- **Login**: Returns a JWT token containing email.  
- **Protected Endpoint (`/Me`)**: Accessible only with a valid JWT token.  
- **Token Expiration**: Configurable via `appsettings.json`.
- **Inactivity Timeout**: User session expires after 15 minutes of inactivity (sliding expiration).

---

## Environment Variables

Set the following before running:

| Variable | Description |
|----------|-------------|
| `ASPNETCORE_ENVIRONMENT` | Environment mode (e.g., `Development`, `Qualitycontrol`, `Staging`, `Production`) |
| `THA_JWT_SIGNING_KEY` | Secret key used to sign JWT tokens |

---

## Running the Project
- *Restore dependencies:* dotnet restore
- *Build the project:* dotnet build
- *Run the API: dotnet run

Swagger UI: [https://localhost:7068/swagger](https://localhost:7068/swagger)

---

## API Endpoints

| Endpoint | Method | Auth | Description |
|----------|--------|------|-------------|
| `/Register` | POST | ❌ | Register a new user |
| `/Login` | POST | ❌ | Login and get JWT token |
| `/Me` | POST | ✅ | Protected route; shows welcome message |

---

## Example Login Response

```json
{
  "status": true,
  "message": "Login successful",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
  }
}