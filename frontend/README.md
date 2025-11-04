# Frontend (React)

Este frontend está construido con [Vite](https://vitejs.dev/) y React. Proporciona una interfaz mínima para consumir la API de blogs ya existente en ASP.NET Core, permitiendo crear, listar, editar y eliminar blogs.

## Requisitos previos

- Node.js 18+
- npm 9+
- Una instancia en ejecución de la API (`BlogApi`) escuchando peticiones HTTP/HTTPS

## Variables de entorno

El cliente utiliza la variable `VITE_API_BASE_URL` para conocer la URL base de la API. Por ejemplo, si la API está disponible en `https://localhost:5001`, crea un archivo `.env.local` dentro de `frontend/` con el siguiente contenido:

```env
VITE_API_BASE_URL=https://localhost:5001/api
```

Si no se especifica esta variable, el cliente asumirá por defecto `https://localhost:5001/api`.

## Instalación de dependencias

```bash
cd frontend
npm install
```

## Ejecutar el servidor de desarrollo

```bash
npm run dev
```

El servidor de Vite expone la aplicación en `http://localhost:3000`.

## Construir para producción

```bash
npm run build
```

## Previsualizar la build de producción

```bash
npm run preview
```
