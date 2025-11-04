# Blog Frontend

Este repositorio contiene un frontend en React creado con Vite para interactuar con la API de blogs desarrollada en ASP.NET Core. La interfaz permite crear, listar, editar y eliminar blogs utilizando los endpoints CRUD expuestos por la API (`/api/blogs`).

## Estructura del proyecto

```
.
└── frontend          # Aplicación React + Vite
```

La API se asume existente y en ejecución por separado. El frontend únicamente consume sus endpoints mediante peticiones HTTP.

## Requisitos

- Node.js 18 o superior
- npm 9 o superior
- Una instancia operativa de la API (consultar su documentación para el despliegue)

## Puesta en marcha rápida

1. Sitúate en el directorio `frontend/`.
2. Instala las dependencias con `npm install`.
3. Define la URL de la API en un archivo `.env.local` (ver [`frontend/README.md`](frontend/README.md)).
4. Arranca el servidor de desarrollo con `npm run dev`.

Para más detalles y scripts adicionales revisa la documentación dentro de `frontend/`.
