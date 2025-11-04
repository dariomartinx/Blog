import { useState } from 'react';
import PostForm from './components/PostForm.jsx';
import PostList from './components/PostList.jsx';
import { createPost } from './api/client.js';

export default function App() {
  const [creating, setCreating] = useState(false);
  const [refreshToken, setRefreshToken] = useState(0);

  const handleCreatePost = async (payload) => {
    try {
      setCreating(true);
      await createPost(payload);
      setRefreshToken((value) => value + 1);
    } finally {
      setCreating(false);
    }
  };

  const handleRefreshRequest = () => {
    setRefreshToken((value) => value + 1);
  };

  return (
    <main>
      <header>
        <h1>Blog</h1>
        <p>Ejemplo básico de blog con ASP.NET Core + React</p>
      </header>
      <PostForm onSubmit={handleCreatePost} loading={creating} />
      <PostList refreshToken={refreshToken} onRefreshRequest={handleRefreshRequest} />
    </main>
  );
}
