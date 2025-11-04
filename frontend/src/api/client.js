import axios from 'axios';

const client = axios.create({
  baseURL: '/api',
  headers: {
    'Content-Type': 'application/json'
  }
});

export const fetchPosts = async () => {
  const response = await client.get('/posts');
  return response.data;
};

export const fetchPost = async (id) => {
  const response = await client.get(`/posts/${id}`);
  return response.data;
};

export const createPost = async (payload) => {
  const response = await client.post('/posts', payload);
  return response.data;
};

export const updatePost = async (id, payload) => {
  await client.put(`/posts/${id}`, payload);
};

export const deletePost = async (id) => {
  await client.delete(`/posts/${id}`);
};

export const fetchComments = async (postId) => {
  const response = await client.get(`/posts/${postId}/comments`);
  return response.data;
};

export const createComment = async (postId, payload) => {
  const response = await client.post(`/posts/${postId}/comments`, payload);
  return response.data;
};

export const deleteComment = async (postId, commentId) => {
  await client.delete(`/posts/${postId}/comments/${commentId}`);
};

export default client;
