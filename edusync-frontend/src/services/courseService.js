import api from './api';

const CourseService = {
  // Get all courses
  getAllCourses: async () => {
    try {
      const response = await api.get('/Courses');
      return response.data;
    } catch (error) {
      console.error('Error fetching courses:', error);
      throw error;
    }
  },

  // Get course by ID
  getCourseById: async (id) => {
    try {
      const response = await api.get(`/Courses/${id}`);
      return response.data;
    } catch (error) {
      console.error(`Error fetching course ${id}:`, error);
      throw error;
    }
  },

  // Create new course (Instructor only)
  createCourse: async (courseData) => {
    try {
      const response = await api.post('/Courses', courseData);
      return response.data;
    } catch (error) {
      console.error('Error creating course:', error);
      throw error;
    }
  },

  // Update course (Instructor only)
  updateCourse: async (id, courseData) => {
    try {
      const response = await api.put(`/Courses/${id}`, courseData);
      return response.data;
    } catch (error) {
      console.error(`Error updating course ${id}:`, error);
      throw error;
    }
  },

  // Delete course (Instructor only)
  deleteCourse: async (id) => {
    try {
      const response = await api.delete(`/Courses/${id}`);
      return response.data;
    } catch (error) {
      console.error(`Error deleting course ${id}:`, error);
      throw error;
    }
  },

  // Get instructor's courses
  getInstructorCourses: async (instructorId) => {
    try {
      const response = await api.get(`/Courses/instructor/${instructorId}`);
      return response.data;
    } catch (error) {
      console.error(`Error fetching courses for instructor ${instructorId}:`, error);
      throw error;
    }
  },

  // Get course assessments
  getCourseAssessments: async (courseId) => {
    try {
      const response = await api.get(`/Courses/${courseId}/assessments`);
      return response.data;
    } catch (error) {
      console.error(`Error fetching assessments for course ${courseId}:`, error);
      throw error;
    }
  }
};

export default CourseService;
