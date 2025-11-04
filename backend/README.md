# Backend (ASP.NET Core Web API)

This project exposes a REST API for managing blog posts and their comments. It is implemented with ASP.NET Core 8 and Entity Framework Core backed by SQLite.

## Prerequisites

- .NET 8 SDK or later
- SQLite (optional locally – the provider creates the database file automatically)

## Restore dependencies

```bash
cd backend
 dotnet restore
```

## Apply migrations

Update the database using Entity Framework Core tools. The connection string is configured to use a local `blog.db` SQLite database by default.

```bash
cd backend/Blog.Api
 dotnet ef database update
```

## Run the API

```bash
cd backend/Blog.Api
 dotnet run
```

The API listens on `http://localhost:5072` by default. Swagger UI is available at `http://localhost:5072/swagger` when running in Development mode.

## Run tests

```bash
cd backend
 dotnet test
```

The test project exercises the `PostService` using the Entity Framework Core in-memory provider to verify basic CRUD behavior.
