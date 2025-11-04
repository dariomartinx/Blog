import axios from 'axios';

const normalizeBaseUrl = (value) => {
  if (!value) {
    return undefined;
  }
  return value.endsWith('/') ? value.slice(0, -1) : value;
};

const API_BASE_URL =
  normalizeBaseUrl(import.meta.env.VITE_API_BASE_URL) ?? 'https://localhost:5001/api';

const client = axios.create({
  baseURL: `${API_BASE_URL}/blogs`,
  headers: {
    'Content-Type': 'application/json'
  }
});

export const fetchBlogs = async () => {
  const response = await client.get('/');
  return response.data;
};

export const fetchBlog = async (id) => {
  const response = await client.get(`/${id}`);
  return response.data;
};

export const createBlog = async (payload) => {
  const response = await client.post('/', payload);
  return response.data;
};

export const updateBlog = async (id, payload) => {
  await client.put(`/${id}`, payload);
};

export const deleteBlog = async (id) => {
  await client.delete(`/${id}`);
};

export default client;
