using System;
using System.ComponentModel.DataAnnotations;

namespace EduSync.DTOs;

public class CreateCourseDto
{
    [Required]
    public required string Title { get; set; }

    [Required]
    public required string Description { get; set; }

    [Required]
    public required Guid InstructorId { get; set; }

    public string? MediaUrl { get; set; }

    public string? Level { get; set; }

    public string? Category { get; set; }

    public string? Duration { get; set; }

    public string? Status { get; set; }

    public decimal Price { get; set; } = 0;
}
