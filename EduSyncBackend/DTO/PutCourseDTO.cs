using System;

namespace EduSync.DTOs
{
    public class PutCourseDTO
    {
        public Guid CourseId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Guid? InstructorId { get; set; }
        public string? MediaUrl { get; set; }
        public string? Level { get; set; }
        public string? Category { get; set; }
        public string? Duration { get; set; }
        public string? Status { get; set; }
        public decimal Price { get; set; }
    }
}
