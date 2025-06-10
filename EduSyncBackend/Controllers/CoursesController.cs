using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EduSync.Data;
using EduSync.Models;
using EduSync.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace EduSync.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly EduSyncContext _context;

        public CoursesController(EduSyncContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCourseDTO>>> GetCourses()
        {
            var courses = await _context.Courses.ToListAsync();

            var dtoList = courses.Select(c => new GetCourseDTO
            {
                CourseId = c.CourseId,
                Title = c.Title ?? string.Empty,
                Description = c.Description ?? string.Empty,
                InstructorId = c.InstructorId ?? Guid.Empty
            }).ToList();

            return Ok(dtoList);
        }

        // GET: api/Courses/instructor/{instructorId}
        [HttpGet("instructor/{instructorId}")]
        public async Task<ActionResult<IEnumerable<GetCourseDTO>>> GetInstructorCourses(Guid instructorId)
        {
            // Verify that the current user is the instructor
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (currentUserId != instructorId.ToString())
            {
                return Forbid();
            }

            var courses = await _context.Courses
                .Where(c => c.InstructorId == instructorId)
                .ToListAsync();

            var dtoList = courses.Select(c => new GetCourseDTO
            {
                CourseId = c.CourseId,
                Title = c.Title ?? string.Empty,
                Description = c.Description ?? string.Empty,
                InstructorId = c.InstructorId ?? Guid.Empty
            }).ToList();

            return Ok(dtoList);
        }

        // GET: api/Courses/{courseId}/assessments
        [HttpGet("{courseId}/assessments")]
        public async Task<ActionResult<IEnumerable<GetAssessmentDTO>>> GetCourseAssessments(Guid courseId)
        {
            // Verify that the current user has access to this course
            var course = await _context.Courses.FindAsync(courseId);
            if (course == null)
            {
                return NotFound();
            }

            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (course.InstructorId.ToString() != currentUserId)
            {
                return Forbid();
            }

            var assessments = await _context.Assessments
                .Where(a => a.CourseId == courseId)
                .ToListAsync();

            var dtoList = assessments.Select(a => new GetAssessmentDTO
            {
                AssessmentId = a.AssessmentId,
                CourseId = a.CourseId ?? Guid.Empty,
                Title = a.Title ?? string.Empty,
                Questions = a.Questions ?? string.Empty,
                MaxScore = a.MaxScore ?? 0
            }).ToList();

            return Ok(dtoList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetCourseDTO>> GetCourse(Guid id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
                return NotFound();

            var dto = new GetCourseDTO
            {
                CourseId = course.CourseId,
                Title = course.Title ?? string.Empty,
                Description = course.Description ?? string.Empty,
                InstructorId = course.InstructorId ?? Guid.Empty
            };

            return Ok(dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(Guid id, PutCourseDTO dto)
        {
            if (id != dto.CourseId)
                return BadRequest();

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return NotFound();

            // Verify that the current user is the instructor
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (course.InstructorId.ToString() != currentUserId)
            {
                return Forbid();
            }

            course.Title = dto.Title;
            course.Description = dto.Description;
            course.InstructorId = dto.InstructorId;
            course.MediaUrl = dto.MediaUrl;
            course.Level = dto.Level;
            course.Category = dto.Category;
            course.Duration = dto.Duration;
            course.Status = dto.Status;
            course.Price = dto.Price;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<GetCourseDTO>> PostCourse(CreateCourseDto dto)
        {
            // Set the instructor ID to the current user
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized();
            }

            var newCourse = new Course
            {
                CourseId = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                InstructorId = Guid.Parse(currentUserId),
                MediaUrl = dto.MediaUrl,
                Level = dto.Level,
                Category = dto.Category,
                Duration = dto.Duration,
                Status = dto.Status,
                Price = dto.Price
            };

            _context.Courses.Add(newCourse);
            await _context.SaveChangesAsync();

            var resultDto = new GetCourseDTO
            {
                CourseId = newCourse.CourseId,
                Title = newCourse.Title ?? string.Empty,
                Description = newCourse.Description ?? string.Empty,
                InstructorId = newCourse.InstructorId ?? Guid.Empty,
                MediaUrl = newCourse.MediaUrl,
                Level = newCourse.Level,
                Category = newCourse.Category,
                Duration = newCourse.Duration,
                Status = newCourse.Status,
                Price = newCourse.Price
            };

            return CreatedAtAction(nameof(GetCourse), new { id = resultDto.CourseId }, resultDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(Guid id)
        {
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
                return NotFound();

            // Verify that the current user is the instructor
            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (course.InstructorId.ToString() != currentUserId)
            {
                return Forbid();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CourseExists(Guid id)
        {
            return _context.Courses.Any(e => e.CourseId == id);
        }
    }
}