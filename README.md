# Wonga Developer Assessment – Full Stack Application

## Overview

A full-stack authentication application built for the Wonga Developer Assessment. The application demonstrates a complete login/registration flow with a React frontend, C# API backend, PostgreSQL database, and JWT authentication — all containerized with Docker.

---

## Tech Stack

**Frontend**
- React
- React Router DOM
- Axios
- CSS3 (custom animations, glassmorphism)

**Backend**
- ASP.NET Core 9
- Entity Framework Core
- BCrypt.Net (password hashing)
- JWT Authentication

**Database**
- PostgreSQL 15

**Infrastructure**
- Docker
- Docker Compose

---

## Features

- User registration with first name, last name, email and password
- Secure login with JWT token generation
- Protected user details page (inaccessible without authentication)
- Password hashing with BCrypt
- Auto database migrations on startup
- CORS configured for frontend communication
- Unit tests with xUnit

---

## Project Structure
```
wonga-assessment/
├── backend/
│   └── WongaApi/
│       ├── Controllers/        # AuthController, UserController
│       ├── Data/               # AppDbContext, DbContextFactory
│       ├── DTOs/               # Request/Response objects
│       ├── Models/             # User model
│       ├── Migrations/         # EF Core migrations
│       ├── Services/           # AuthService (business logic)
│       ├── Program.cs          # App entry point
│       └── Dockerfile
├── frontend/
│   └── wonga-frontend/
│       ├── src/
│       │   ├── components/     # ProtectedRoute, ShapeBlur
│       │   ├── pages/          # AuthPage, UserDetailsPage
│       │   ├── services/       # api.js (Axios config)
│       │   └── App.js
│       └── Dockerfile
├── WongaApi.Tests/             # Unit tests
├── docker-compose.yml
└── README.md
```

---

## Getting Started

### Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- Git

### Run the Application

1. Clone the repository:
```bash
git clone https://github.com/yourusername/wonga-assessment.git
cd wonga-assessment
```

2. Start all services with Docker Compose:
```bash
docker compose up --build
```

3. Access the application:
   - Frontend: http://localhost:3000
   - Backend API: http://localhost:5000

> The database migrations run automatically on startup — no manual setup required.

### Stop the Application
```bash
docker compose down
```

To also remove all database data:
```bash
docker compose down -v
```

---

## API Endpoints

### Register a User
```
POST /api/auth/register
```
**Body:**
```json
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john@example.com",
  "password": "Password123!"
}
```
**Response:**
```json
{
  "token": "eyJhbGci..."
}
```

---

### Login
```
POST /api/auth/login
```
**Body:**
```json
{
  "email": "john@example.com",
  "password": "Password123!"
}
```
**Response:**
```json
{
  "token": "eyJhbGci..."
}
```

---

### Get User Details (Protected)
```
GET /api/user/me
```
**Headers:**
```
Authorization: Bearer <token>
```
**Response:**
```json
{
  "firstName": "John",
  "lastName": "Doe",
  "email": "john@example.com"
}
```

---

## Running Tests

From the project root:
```bash
dotnet test
```

---

## Environment Variables

| Variable | Description | Default |
|----------|-------------|---------|
| `JWT_SECRET` | Secret key for JWT signing | Set in docker-compose.yml |
| `ConnectionStrings__Default` | PostgreSQL connection string | Set in docker-compose.yml |

---

## How It Works

1. User registers via the React frontend form
2. The C# API hashes the password with BCrypt and stores the user in PostgreSQL
3. On successful registration the user is redirected to login
4. User logs in — the API verifies credentials and returns a JWT token
5. The frontend stores the token in localStorage
6. The protected User Details page sends the JWT token with every request
7. The API validates the token and returns the user's details
8. If the token is missing or invalid the user is redirected to login

---

## Author

Built by Mikaeel Pathan for the Wonga Developer Assessment.