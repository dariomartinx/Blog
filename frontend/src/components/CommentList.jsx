import { useEffect, useState } from 'react';
import { fetchComments, deleteComment } from '../api/client.js';

export default function CommentList({ postId, refreshToken = 0 }) {
  const [comments, setComments] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    let active = true;

    const loadComments = async () => {
      try {
        setLoading(true);
        const data = await fetchComments(postId);
        if (active) {
          setComments(data);
        }
      } catch (err) {
        if (active) {
          setError('No se han podido cargar los comentarios');
        }
      } finally {
        if (active) {
          setLoading(false);
        }
      }
    };

    loadComments();

    return () => {
      active = false;
    };
  }, [postId, refreshToken]);

  const handleDelete = async (commentId) => {
    if (!confirm('¿Eliminar este comentario?')) {
      return;
    }
    await deleteComment(postId, commentId);
    const data = await fetchComments(postId);
    setComments(data);
  };

  if (loading) {
    return <p>Cargando comentarios...</p>;
  }

  if (error) {
    return <p role="alert">{error}</p>;
  }

  if (comments.length === 0) {
    return <p>Sé el primero en comentar.</p>;
  }

  return (
    <div>
      {comments.map((comment) => (
        <div key={comment.id} className="comment">
          <p>
            <strong>{comment.authorName}</strong> –{' '}
            {new Date(comment.createdAt).toLocaleString()}
          </p>
          <p>{comment.content}</p>
          <button type="button" onClick={() => handleDelete(comment.id)}>
            Borrar
          </button>
        </div>
      ))}
    </div>
  );
}
