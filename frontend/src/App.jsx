import { useState } from 'react';
import BlogForm from './components/BlogForm.jsx';
import BlogList from './components/BlogList.jsx';
import { createBlog } from './api/client.js';

export default function App() {
  const [creating, setCreating] = useState(false);
  const [refreshToken, setRefreshToken] = useState(0);

  const handleCreateBlog = async (payload) => {
    try {
      setCreating(true);
      await createBlog(payload);
      setRefreshToken((value) => value + 1);
    } catch (err) {
      alert('No se pudo crear el blog. Revisa los datos e inténtalo de nuevo.');
      console.error(err);
    } finally {
      setCreating(false);
    }
  };

  return (
    <main>
      <header>
        <h1>Blog Manager</h1>
        <p>Interfaz React para administrar blogs publicados con tu API de ASP.NET Core.</p>
      </header>
      <BlogForm onSubmit={handleCreateBlog} loading={creating} />
      <BlogList refreshToken={refreshToken} />
    </main>
  );
}
