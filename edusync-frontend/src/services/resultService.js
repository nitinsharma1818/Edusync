import api from './api';

const ResultService = {
  // Get all results (for instructors or for a student's own results)
  getAllResults: async () => {
    try {
      const response = await api.get('/Results');
      return response.data;
    } catch (error) {
      console.error('Error fetching results:', error);
      throw error;
    }
  },

  // Get result by ID
  getResultById: async (id) => {
    try {
      const response = await api.get(`/Results/${id}`);
      return response.data;
    } catch (error) {
      console.error(`Error fetching result ${id}:`, error);
      throw error;
    }
  },

  // Get results by user ID (for instructors to view student results)
  getResultsByUser: async (userId) => {
    try {
      const response = await api.get(`/Results/user/${userId}`);
      return response.data;
    } catch (error) {
      console.error(`Error fetching results for user ${userId}:`, error);
      throw error;
    }
  },

  // Get results by assessment ID
  getResultsByAssessment: async (assessmentId) => {
    try {
      const response = await api.get(`/Results/assessment/${assessmentId}`);
      return response.data;
    } catch (error) {
      console.error(`Error fetching results for assessment ${assessmentId}:`, error);
      throw error;
    }
  },

  // Create new result (submit assessment)
  createResult: async (resultData) => {
    try {
      const response = await api.post('/Results', resultData);
      return response.data;
    } catch (error) {
      console.error('Error creating result:', error);
      throw error;
    }
  },

  // Update result
  updateResult: async (id, resultData) => {
    try {
      const response = await api.put(`/Results/${id}`, resultData);
      return response.data;
    } catch (error) {
      console.error(`Error updating result ${id}:`, error);
      throw error;
    }
  },

  // Delete result
  deleteResult: async (id) => {
    try {
      const response = await api.delete(`/Results/${id}`);
      return response.data;
    } catch (error) {
      console.error(`Error deleting result ${id}:`, error);
      throw error;
    }
  }
};

export default ResultService;
