import { useEffect, useState } from 'react';

const defaultValues = {
  url: '',
  author: ''
};

export default function BlogForm({
  onSubmit,
  loading = false,
  initialValues,
  onCancel,
  submitLabel = 'Crear blog'
}) {
  const [values, setValues] = useState(() => ({
    ...(initialValues ?? defaultValues)
  }));
  const [errors, setErrors] = useState({});

  useEffect(() => {
    setValues({
      ...(initialValues ?? defaultValues)
    });
    setErrors({});
  }, [initialValues]);

  const handleChange = (event) => {
    const { name, value } = event.target;
    setValues((current) => ({
      ...current,
      [name]: value
    }));
  };

  const validate = () => {
    const nextErrors = {};
    if (!values.url.trim()) {
      nextErrors.url = 'La URL es obligatoria';
    }
    if (!values.author.trim()) {
      nextErrors.author = 'El autor es obligatorio';
    }
    setErrors(nextErrors);
    return Object.keys(nextErrors).length === 0;
  };

  const handleSubmit = async (event) => {
    event.preventDefault();
    if (!validate()) {
      return;
    }
    await onSubmit({
      ...values
    });
    if (!initialValues) {
      setValues({ ...defaultValues });
    }
  };

  return (
    <form className="card" onSubmit={handleSubmit} noValidate>
      <div className="field">
        <label htmlFor="url">URL del blog</label>
        <input
          id="url"
          name="url"
          type="url"
          placeholder="https://mi-blog.com"
          value={values.url}
          onChange={handleChange}
          disabled={loading}
          required
        />
        {errors.url ? <p className="error">{errors.url}</p> : null}
      </div>

      <div className="field">
        <label htmlFor="author">Autor</label>
        <input
          id="author"
          name="author"
          type="text"
          placeholder="Nombre del autor"
          value={values.author}
          onChange={handleChange}
          disabled={loading}
          required
        />
        {errors.author ? <p className="error">{errors.author}</p> : null}
      </div>

      <div className="form-actions">
        {onCancel ? (
          <button type="button" className="secondary" onClick={onCancel} disabled={loading}>
            Cancelar
          </button>
        ) : null}
        <button type="submit" disabled={loading}>
          {loading ? 'Guardando…' : submitLabel}
        </button>
      </div>
    </form>
  );
}
