import api from './api';

const AuthService = {
  // Register a new user
  register: async (userData) => {
    try {
      // Format validation check
      if (!userData.name || !userData.email || !userData.password || !userData.role) {
        throw new Error('Missing required fields');
      }
      
      // Make the API call
      const response = await api.post('/Auth/register', userData);
      
      if (response.data.token) {
        localStorage.setItem('token', response.data.token);
        
        // Store user data
        const user = response.data.user;
        localStorage.setItem('user', JSON.stringify(user));
        return { user };
      }
      
      throw new Error('No token received from server');
    } catch (error) {
      console.error('Registration error:', error.response?.data || error.message);
      throw error;
    }
  },

  // Login user
  login: async (credentials) => {
    try {
      const response = await api.post('/Auth/login', credentials);
      
      if (response.data.token) {
        localStorage.setItem('token', response.data.token);
        
        // Store user data
        const user = response.data.user;
        localStorage.setItem('user', JSON.stringify(user));
        return { user };
      }
      
      throw new Error('No token received from server');
    } catch (error) {
      console.error('Login error:', error.response?.data || error.message);
      throw error;
    }
  },

  // Get user profile
  getProfile: async () => {
    try {
      const response = await api.get('/Auth/profile');
      return response.data;
    } catch (error) {
      console.error('Error fetching profile:', error);
      throw error;
    }
  },

  // Logout user
  logout: () => {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
  },

  // Get current user info
  getCurrentUser: () => {
    const user = localStorage.getItem('user');
    return user ? JSON.parse(user) : null;
  },

  // Check if user is authenticated
  isAuthenticated: () => {
    const token = localStorage.getItem('token');
    const user = localStorage.getItem('user');
    return !!(token && user);
  },

  // Get current auth token
  getToken: () => {
    return localStorage.getItem('token');
  }
};

export default AuthService;
