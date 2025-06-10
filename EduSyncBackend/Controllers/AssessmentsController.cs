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
    public class AssessmentsController : ControllerBase
    {
        private readonly EduSyncContext _context;

        public AssessmentsController(EduSyncContext context)
        {
            _context = context;
        }

        // GET: api/Assessments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetAssessmentDTO>>> GetAssessments()
        {
            var assessments = await _context.Assessments.ToListAsync();

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

        // GET: api/Assessments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetAssessmentDTO>> GetAssessment(Guid id)
        {
            var a = await _context.Assessments.FindAsync(id);

            if (a == null)
                return NotFound();

            var dto = new GetAssessmentDTO
            {
                AssessmentId = a.AssessmentId,
                CourseId = a.CourseId ?? Guid.Empty,
                Title = a.Title ?? string.Empty,
                Questions = a.Questions ?? string.Empty,
                MaxScore = a.MaxScore ?? 0
            };

            return Ok(dto);
        }

        // PUT: api/Assessments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAssessment(Guid id, [FromBody] PutAssessmentDTO dto)
        {
            var assessment = await _context.Assessments.FindAsync(id);
            if (assessment == null)
                return NotFound();

            // Verify that the current user is the instructor of the course
            var course = await _context.Courses.FindAsync(assessment.CourseId);
            if (course == null)
                return NotFound("Course not found");

            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (course.InstructorId.ToString() != currentUserId)
                return Forbid();

            assessment.CourseId = dto.CourseId;
            assessment.Title = dto.Title;
            assessment.Questions = dto.Questions;
            assessment.MaxScore = dto.MaxScore;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AssessmentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Assessments
        [HttpPost]
        public async Task<ActionResult<GetAssessmentDTO>> PostAssessment(CreateAssessmentDto dto)
        {
            // Verify that the current user is the instructor of the course
            var course = await _context.Courses.FindAsync(dto.CourseId);
            if (course == null)
                return NotFound("Course not found");

            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (course.InstructorId.ToString() != currentUserId)
                return Forbid();

            var newAssessment = new Assessment
            {
                AssessmentId = Guid.NewGuid(),
                CourseId = dto.CourseId,
                Title = dto.Title,
                Questions = dto.Questions,
                MaxScore = dto.MaxScore
            };

            _context.Assessments.Add(newAssessment);
            await _context.SaveChangesAsync();

            var resultDTO = new GetAssessmentDTO
            {
                AssessmentId = newAssessment.AssessmentId,
                CourseId = newAssessment.CourseId ?? Guid.Empty,
                Title = newAssessment.Title ?? string.Empty,
                Questions = newAssessment.Questions ?? string.Empty,
                MaxScore = newAssessment.MaxScore ?? 0
            };

            return CreatedAtAction(nameof(GetAssessment), new { id = resultDTO.AssessmentId }, resultDTO);
        }

        // DELETE: api/Assessments
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAssessment(Guid id)
        {
            var assessment = await _context.Assessments.FindAsync(id);
            if (assessment == null)
                return NotFound();

            // Verify that the current user is the instructor of the course
            var course = await _context.Courses.FindAsync(assessment.CourseId);
            if (course == null)
                return NotFound("Course not found");

            var currentUserId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (course.InstructorId.ToString() != currentUserId)
                return Forbid();

            _context.Assessments.Remove(assessment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AssessmentExists(Guid id)
        {
            return _context.Assessments.Any(e => e.AssessmentId == id);
        }
    }
}
