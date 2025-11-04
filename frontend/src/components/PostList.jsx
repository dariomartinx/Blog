import { useEffect, useState } from 'react';
import { fetchPosts, deletePost, fetchPost } from '../api/client.js';
import CommentList from './CommentList.jsx';
import CommentForm from './CommentForm.jsx';

export default function PostList({ refreshToken = 0, onRefreshRequest }) {
  const [posts, setPosts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [selectedPostId, setSelectedPostId] = useState(null);
  const [commentRefresh, setCommentRefresh] = useState({});
  const [postDetails, setPostDetails] = useState({});
  const [detailError, setDetailError] = useState(null);

  useEffect(() => {
    loadPosts();
  }, [refreshToken]);

  useEffect(() => {
    if (selectedPostId) {
      loadPostDetail(selectedPostId);
    }
  }, [selectedPostId]);

  const loadPosts = async () => {
    try {
      setLoading(true);
      setDetailError(null);
      const data = await fetchPosts();
      setPosts(data);
      setCommentRefresh({});
      setPostDetails((current) => {
        const ids = new Set(data.map((post) => post.postId));
        return Object.fromEntries(
          Object.entries(current).filter(([id]) => ids.has(Number(id)))
        );
      });
      if (data.length > 0) {
        setSelectedPostId((current) => {
          if (!current || !data.some((post) => post.postId === current)) {
            return data[0].postId;
          }
          return current;
        });
      } else {
        setSelectedPostId(null);
      }
    } catch (err) {
      setError('No se han podido cargar los posts');
    } finally {
      setLoading(false);
    }
  };

  const loadPostDetail = async (postId) => {
    if (postDetails[postId]) {
      return;
    }
    try {
      setDetailError(null);
      const data = await fetchPost(postId);
      setPostDetails((prev) => ({ ...prev, [postId]: data }));
    } catch (err) {
      setDetailError('No se ha podido cargar el detalle del post.');
    }
  };

  const handleDelete = async (postId) => {
    if (!confirm('¿Seguro que deseas eliminar el post?')) {
      return;
    }
    await deletePost(postId);
    if (typeof onRefreshRequest === 'function') {
      onRefreshRequest();
    } else {
      await loadPosts();
    }
  };

  const handleCommentCreated = (postId) => {
    setCommentRefresh((prev) => ({
      ...prev,
      [postId]: (prev[postId] ?? 0) + 1
    }));
  };

  if (loading) {
    return <p>Cargando posts...</p>;
  }

  if (error) {
    return <p role="alert">{error}</p>;
  }

  return (
    <section>
      <div className="post-list">
        {posts.length === 0 ? (
          <p>Todavía no hay publicaciones.</p>
        ) : (
          posts.map((post) => (
            <article key={post.postId} className="post-card">
              <h2>{post.title}</h2>
              <p>
                <strong>{post.blogAuthor ?? 'Autor desconocido'}</strong>
                {post.blogUrl && (
                  <>
                    {' '}
                    <a href={post.blogUrl} target="_blank" rel="noreferrer">
                      {post.blogUrl}
                    </a>
                  </>
                )}
                {' '}
                – {new Date(post.publishedAt).toLocaleString()}
              </p>
              {selectedPostId === post.postId && (
                <>
                  {detailError && <p role="alert">{detailError}</p>}
                  {postDetails[post.postId]?.content && (
                    <p>{postDetails[post.postId].content}</p>
                  )}
                </>
              )}
              <div className="actions">
                <button type="button" onClick={() => setSelectedPostId(post.postId)}>
                  Ver detalles
                </button>{' '}
                <button type="button" onClick={() => handleDelete(post.postId)}>
                  Eliminar
                </button>
              </div>
              {selectedPostId === post.postId && (
                <div className="comments">
                  <CommentList
                    postId={post.postId}
                    refreshToken={commentRefresh[post.postId] ?? 0}
                  />
                  <CommentForm
                    postId={post.postId}
                    onCreated={() => handleCommentCreated(post.postId)}
                  />
                </div>
              )}
            </article>
          ))
        )}
      </div>
    </section>
  );
}
