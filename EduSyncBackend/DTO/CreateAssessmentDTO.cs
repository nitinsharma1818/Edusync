using System;
using System.ComponentModel.DataAnnotations;

namespace EduSync.DTOs
{
    public class CreateAssessmentDto
    {
        [Required]
        public Guid CourseId { get; set; }

        [Required]
        public string? Title { get; set; }

        [Required]
        public string? Questions { get; set; } // JSON string of quiz questions

        [Required]
        public int MaxScore { get; set; }
    }
}
