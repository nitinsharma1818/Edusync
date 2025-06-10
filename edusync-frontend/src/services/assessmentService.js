import api from './api';

const AssessmentService = {
  // Get all assessments
  getAllAssessments: async () => {
    try {
      const response = await api.get('/Assessments');
      return response.data;
    } catch (error) {
      console.error('Error fetching assessments:', error);
      throw error;
    }
  },

  // Get assessment by ID
  getAssessmentById: async (id) => {
    try {
      const response = await api.get(`/Assessments/${id}`);
      return response.data;
    } catch (error) {
      console.error(`Error fetching assessment ${id}:`, error);
      throw error;
    }
  },

  // Create new assessment (Instructor only)
  createAssessment: async (assessmentData) => {
    try {
      const response = await api.post('/Assessments', assessmentData);
      return response.data;
    } catch (error) {
      console.error('Error creating assessment:', error);
      throw error;
    }
  },

  // Update assessment (Instructor only)
  updateAssessment: async (id, assessmentData) => {
    try {
      const response = await api.put(`/Assessments/${id}`, assessmentData);
      return response.data;
    } catch (error) {
      console.error(`Error updating assessment ${id}:`, error);
      throw error;
    }
  },

  // Delete assessment (Instructor only)
  deleteAssessment: async (id) => {
    try {
      const response = await api.delete(`/Assessments/${id}`);
      return response.data;
    } catch (error) {
      console.error(`Error deleting assessment ${id}:`, error);
      throw error;
    }
  },
  
  // Submit assessment answers (Student only)
  submitAssessment: async (assessmentId, answers) => {
    const response = await api.post(`/Results`, {
      assessmentId,
      answers
    });
    return response.data;
  }
};

export default AssessmentService;
