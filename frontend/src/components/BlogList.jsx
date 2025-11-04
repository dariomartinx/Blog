import { useCallback, useEffect, useMemo, useState } from 'react';
import { deleteBlog, fetchBlogs, updateBlog } from '../api/client.js';
import BlogForm from './BlogForm.jsx';

const formatDate = (isoString) => {
  if (!isoString) {
    return 'Sin fecha';
  }
  const date = new Date(isoString);
  return new Intl.DateTimeFormat('es-ES', {
    dateStyle: 'medium',
    timeStyle: 'short'
  }).format(date);
};

export default function BlogList({ refreshToken }) {
  const [blogs, setBlogs] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [editingId, setEditingId] = useState(null);
  const [saving, setSaving] = useState(false);

  const loadBlogs = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const data = await fetchBlogs();
      setBlogs(Array.isArray(data) ? data : []);
    } catch (err) {
      setError('No se pudieron cargar los blogs.');
      console.error(err);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    loadBlogs();
  }, [loadBlogs, refreshToken]);

  const handleDelete = useCallback(
    async (id) => {
      if (!window.confirm('¿Seguro que quieres eliminar este blog?')) {
        return;
      }
      try {
        await deleteBlog(id);
        setBlogs((current) => current.filter((blog) => blog.blogId !== id));
      } catch (err) {
        alert('No se pudo eliminar el blog. Inténtalo de nuevo más tarde.');
        console.error(err);
      }
    },
    []
  );

  const editingBlog = useMemo(
    () => blogs.find((blog) => blog.blogId === editingId) ?? null,
    [blogs, editingId]
  );

  const handleUpdate = useCallback(
    async (values) => {
      if (!editingBlog) {
        return;
      }
      try {
        setSaving(true);
        const payload = {
          blogId: editingBlog.blogId,
          url: values.url,
          author: values.author,
          createdAt: editingBlog.createdAt,
          posts: editingBlog.posts ?? []
        };
        await updateBlog(editingBlog.blogId, payload);
        setBlogs((current) =>
          current.map((blog) =>
            blog.blogId === editingBlog.blogId
              ? { ...blog, url: values.url, author: values.author }
              : blog
          )
        );
        setEditingId(null);
      } catch (err) {
        alert('No se pudo actualizar el blog.');
        console.error(err);
      } finally {
        setSaving(false);
      }
    },
    [editingBlog]
  );

  if (loading) {
    return <p>Cargando blogs…</p>;
  }

  if (error) {
    return (
      <div className="card">
        <p className="error">{error}</p>
        <button type="button" onClick={loadBlogs}>
          Reintentar
        </button>
      </div>
    );
  }

  if (blogs.length === 0) {
    return (
      <div className="card empty">
        <p>Aún no hay blogs. ¡Crea el primero usando el formulario superior!</p>
      </div>
    );
  }

  return (
    <ul className="blog-list">
      {blogs.map((blog) => (
        <li key={blog.blogId} className="card">
          {editingId === blog.blogId ? (
            <BlogForm
              initialValues={{ url: blog.url ?? '', author: blog.author ?? '' }}
              onSubmit={handleUpdate}
              onCancel={() => setEditingId(null)}
              loading={saving}
              submitLabel="Guardar cambios"
            />
          ) : (
            <article>
              <header className="blog-header">
                <h2>{blog.author ?? 'Autor desconocido'}</h2>
                <time dateTime={blog.createdAt}>{formatDate(blog.createdAt)}</time>
              </header>
              <p className="blog-url">
                <a href={blog.url} target="_blank" rel="noreferrer">
                  {blog.url}
                </a>
              </p>
              <footer className="blog-actions">
                <button type="button" className="secondary" onClick={() => setEditingId(blog.blogId)}>
                  Editar
                </button>
                <button type="button" className="danger" onClick={() => handleDelete(blog.blogId)}>
                  Eliminar
                </button>
              </footer>
            </article>
          )}
        </li>
      ))}
    </ul>
  );
}
