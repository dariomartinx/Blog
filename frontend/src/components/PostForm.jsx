import { useState } from 'react';

const createInitialForm = () => ({
  title: '',
  authorName: '',
  content: '',
  publishedAt: new Date().toISOString().slice(0, 16)
});

export default function PostForm({ onSubmit, loading }) {
  const [form, setForm] = useState(createInitialForm);

  const handleChange = (event) => {
    const { name, value } = event.target;
    setForm((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    const payload = {
      title: form.title,
      authorName: form.authorName,
      content: form.content,
      publishedAt: form.publishedAt ? new Date(form.publishedAt).toISOString() : null
    };
    try {
      await onSubmit(payload);
      setForm(createInitialForm());
    } catch (error) {
      // eslint-disable-next-line no-alert
      alert('No se ha podido crear el post. Inténtalo de nuevo.');
    }
  };

  return (
    <section>
      <h2>Crear nuevo post</h2>
      <form onSubmit={handleSubmit}>
        <label>
          Título
          <input
            name="title"
            value={form.title}
            onChange={handleChange}
            required
            maxLength={200}
          />
        </label>
        <label>
          Autor
          <input
            name="authorName"
            value={form.authorName}
            onChange={handleChange}
            required
            maxLength={100}
          />
        </label>
        <label>
          Contenido
          <textarea
            name="content"
            value={form.content}
            onChange={handleChange}
            required
            rows={6}
          />
        </label>
        <label>
          Fecha de publicación
          <input
            type="datetime-local"
            name="publishedAt"
            value={form.publishedAt}
            onChange={handleChange}
          />
        </label>
        <button type="submit" disabled={loading}>
          {loading ? 'Guardando...' : 'Publicar'}
        </button>
      </form>
    </section>
  );
}
