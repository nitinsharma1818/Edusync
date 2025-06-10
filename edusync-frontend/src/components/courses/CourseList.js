import React, { useState, useEffect, useContext } from 'react';
import { Link } from 'react-router-dom';
import { AuthContext } from '../../context/AuthContext';
import courseService from '../../services/courseService';

const CourseList = () => {
  const { currentUser, isInstructor } = useContext(AuthContext);
  const [courses, setCourses] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [searchTerm, setSearchTerm] = useState('');

  useEffect(() => {
    const fetchCourses = async () => {
      try {
        setLoading(true);
        const data = await courseService.getAllCourses();
        setCourses(data);
      } catch (err) {
        console.error('Error fetching courses:', err);
        setError('Failed to load courses. Please try again later.');
      } finally {
        setLoading(false);
      }
    };

    fetchCourses();
  }, []);

  const filteredCourses = courses.filter(course =>
    course.title.toLowerCase().includes(searchTerm.toLowerCase()) ||
    course.description.toLowerCase().includes(searchTerm.toLowerCase())
  );

  if (loading) {
    return (
      <div className="d-flex justify-content-center my-5">
        <div className="spinner-border text-primary" role="status">
          <span className="visually-hidden">Loading...</span>
        </div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="alert alert-danger my-5" role="alert">
        {error}
      </div>
    );
  }

  return (
    <div className="container py-5">
      <div className="row mb-4">
        <div className="col">
          <h1>Courses</h1>
          <p className="text-muted">Browse all available courses</p>
        </div>
        {isInstructor && (
          <div className="col-auto">
            <Link to="/instructor/courses/create" className="btn btn-primary">
              <i className="bi bi-plus-circle me-2"></i>
              Create New Course
            </Link>
          </div>
        )}
      </div>

      {/* Search and Filter */}
      <div className="row mb-4">
        <div className="col-md-6">
          <div className="input-group">
            <span className="input-group-text">
              <i className="bi bi-search"></i>
            </span>
            <input
              type="text"
              className="form-control"
              placeholder="Search courses..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
            {searchTerm && (
              <button
                className="btn btn-outline-secondary"
                type="button"
                onClick={() => setSearchTerm('')}
              >
                <i className="bi bi-x"></i>
              </button>
            )}
          </div>
        </div>
      </div>

      {/* Course Cards */}
      {filteredCourses.length === 0 ? (
        <div className="alert alert-info">
          {searchTerm 
            ? 'No courses match your search criteria.' 
            : 'No courses available at the moment.'}
        </div>
      ) : (
        <div className="row row-cols-1 row-cols-md-2 row-cols-lg-3 g-4">
          {filteredCourses.map(course => (
            <div className="col" key={course.courseId}>
              <div className="card h-100 shadow-sm">
                <div className="card-img-top bg-light" style={{height: '160px'}}>
                  {course.mediaUrl ? (
                    <img 
                      src={course.mediaUrl} 
                      className="img-fluid h-100 w-100 object-fit-cover" 
                      alt={course.title} 
                    />
                  ) : (
                    <div className="d-flex justify-content-center align-items-center h-100 text-muted">
                      <i className="bi bi-image fs-1"></i>
                    </div>
                  )}
                </div>
                <div className="card-body">
                  <h5 className="card-title">{course.title}</h5>
                  <p className="card-text text-truncate">{course.description}</p>
                </div>
                <div className="card-footer bg-white border-top-0">
                  <Link 
                    to={`/courses/${course.courseId}`} 
                    className="btn btn-outline-primary w-100"
                  >
                    View Course
                  </Link>
                </div>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default CourseList;
