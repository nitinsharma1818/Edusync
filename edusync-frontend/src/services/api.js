import axios from 'axios';

// Get API URL from environment variables or use default
const API_URL = process.env.REACT_APP_API_URL || 'http://localhost:5122/api';

console.log('API Base URL:', API_URL);

// Create axios instance with default config
const api = axios.create({
    baseURL: API_URL,
    headers: {
        'Content-Type': 'application/json',
    },
    // Ensure our API calls don't timeout too quickly
    timeout: 10000
});

// Add request interceptor to include auth token in headers
api.interceptors.request.use(
    (config) => {
        // Get token from localStorage
        const token = localStorage.getItem('token');
        if (token) {
            // Add authorization header with Bearer scheme
            config.headers.Authorization = `Bearer ${token}`;
        }

        // Log outgoing requests (helpful for debugging)
        console.log(`API Request: ${config.method.toUpperCase()} ${config.url}`, {
            ...config,
            headers: {
                ...config.headers,
                Authorization: config.headers.Authorization ? '[HIDDEN]' : undefined
            }
        });

        return config;
    },
    (error) => {
        console.error('API Request Error:', error);
        return Promise.reject(error);
    }
);

// Add response interceptor for consistent error handling
api.interceptors.response.use(
    (response) => {
        // Log successful responses (helpful for debugging)
        console.log(`API Response: ${response.status}`, response.data);
        return response;
    },
    (error) => {
        // Log error responses
        console.error('API Response Error:', error.response || error);

        // Handle authentication errors
        if (error.response?.status === 401) {
            // Clear invalid token
            localStorage.removeItem('token');
            localStorage.removeItem('user');
            
            // Redirect to login if not already there
            if (!window.location.pathname.includes('/login')) {
                window.location.href = '/login';
            }
        }

        // Extract the most useful error information
        const errorData = {
            status: error.response?.status,
            statusText: error.response?.statusText,
            data: error.response?.data,
            message: error.response?.data?.message || error.response?.data || error.message
        };

        console.log('Formatted Error:', errorData);

        return Promise.reject(error);
    }
);

export default api;