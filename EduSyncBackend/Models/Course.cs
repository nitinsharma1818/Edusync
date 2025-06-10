using System;
using System.Collections.Generic;

namespace EduSync.Models;

public partial class Course
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

    public decimal Price { get; set; } = 0; // Default to 0 for free courses
}
