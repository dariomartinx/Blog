# Blog Full Stack Application

This repository contains a minimal end-to-end blog application composed of an ASP.NET Core Web API backend backed by Entity Framework Core and a React front-end. The goal is to illustrate how to structure a small project that exposes CRUD endpoints for blog posts and their comments while providing a simple interface to interact with the API.

## Project structure

```
.
├── backend           # ASP.NET Core Web API + EF Core
└── frontend          # React application powered by Vite
```

Each section of the repository includes a dedicated README with detailed instructions on how to restore dependencies, run migrations, start the application, and execute automated tests.

## Requirements

* .NET 8 SDK or later
* Node.js 18 or later
* SQLite (optional – created automatically by EF Core migrations)

## Getting started

1. Follow the instructions in [`backend/README.md`](backend/README.md) to configure the API, apply migrations, and run the integration tests.
2. Follow the instructions in [`frontend/README.md`](frontend/README.md) to install dependencies and run the React development server.

Both applications are configured to work together out of the box. The React client expects the API to be running on `http://localhost:5072` and proxies API calls accordingly.
