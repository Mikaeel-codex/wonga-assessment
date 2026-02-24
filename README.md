# Wonga Assessment – Full Stack Application

## 📌 Overview
This project is a full-stack application built for the Wonga Developer Assessment.

The backend is built with:
- ASP.NET Core 8
- Entity Framework Core
- PostgreSQL
- JWT Authentication
- Docker

The frontend (in progress) will consume the backend API.

---

## 🚀 Backend Setup

### 1️⃣ Run with Docker

From project root:

```bash
docker compose up --build

The API will run at:

http://localhost:5000

2️⃣ Required Environment Variable

The backend requires:

JWT_SECRET

This is configured in docker-compose.yml.

3️⃣ Run Tests

From project root:

dotnet test
🔐 API Endpoints
Register

POST /api/auth/register

Login

POST /api/auth/login

Get Current User (Protected)

GET /api/user/me
Requires Bearer token.

🧪 Testing

Unit tests are implemented for:

User registration

Duplicate email handling

Login success

Login failure

🛠️ Tech Stack

.NET 8

EF Core

PostgreSQL

JWT

xUnit

Docker

📂 Project Structure
backend/          → ASP.NET Core API
WongaApi.Tests/   → Unit Tests
docker-compose.yml
WongaAssessment.sln