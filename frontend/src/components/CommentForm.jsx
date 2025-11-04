import { useState } from 'react';
import { createComment } from '../api/client.js';

const createInitialForm = () => ({
  authorName: '',
  content: ''
});

export default function CommentForm({ postId, onCreated = () => {} }) {
  const [form, setForm] = useState(createInitialForm);
  const [submitting, setSubmitting] = useState(false);

  const handleChange = (event) => {
    const { name, value } = event.target;
    setForm((prev) => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    try {
      setSubmitting(true);
      await createComment(postId, form);
      setForm(createInitialForm());
      onCreated();
    } catch (error) {
      // eslint-disable-next-line no-alert
      alert('No se ha podido crear el comentario.');
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <form onSubmit={handleSubmit}>
      <h3>Nuevo comentario</h3>
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
        Comentario
        <textarea
          name="content"
          value={form.content}
          onChange={handleChange}
          required
          rows={4}
          maxLength={500}
        />
      </label>
      <button type="submit" disabled={submitting}>
        {submitting ? 'Enviando...' : 'Comentar'}
      </button>
    </form>
  );
}
