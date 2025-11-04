# Frontend (React)

This folder contains a lightweight React single-page application created with Vite. It consumes the ASP.NET Core Web API located in `../backend/Blog.Api` to display posts and comments.

## Prerequisites

- Node.js 18+
- npm 9+

## Install dependencies

```bash
cd frontend
npm install
```

## Run the development server

```bash
npm run dev
```

The Vite server runs on `http://localhost:3000` and proxies API requests under `/api` to the ASP.NET Core backend running on `http://localhost:5072`.

## Build for production

```bash
npm run build
```

## Preview the production build locally

```bash
npm run preview
```
