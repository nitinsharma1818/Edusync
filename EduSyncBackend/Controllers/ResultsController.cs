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
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ResultsController : ControllerBase
    {
        private readonly EduSyncContext _context;

        public ResultsController(EduSyncContext context)
        {
            _context = context;
        }

        // GET: api/Results
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetResultDto>>> GetResults()
        {
            return await _context.Results
                .Select(r => new GetResultDto
                {
                    ResultId = r.ResultId,
                    AssessmentId = r.AssessmentId!.Value,
                    UserId = r.UserId!.Value,
                    Score = r.Score!.Value,
                    AttemptDate = r.AttemptDate!.Value
                })
                .ToListAsync();
        }

        // GET: api/Results/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetResultDto>> GetResult(Guid id)
        {
            var result = await _context.Results.FindAsync(id);

            if (result == null)
                return NotFound();

            var dto = new GetResultDto
            {
                ResultId = result.ResultId,
                AssessmentId = result.AssessmentId!.Value,
                UserId = result.UserId!.Value,
                Score = result.Score!.Value,
                AttemptDate = result.AttemptDate!.Value
            };

            return dto;
        }

        // PUT: api/Results/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutResult(Guid id, PutResultDto dto)
        {
            if (id != dto.ResultId)
                return BadRequest("ID in URL does not match ID in body.");

            var result = await _context.Results.FindAsync(id);
            if (result == null)
                return NotFound();

            result.AssessmentId = dto.AssessmentId;
            result.UserId = dto.UserId;
            result.Score = dto.Score;
            result.AttemptDate = dto.AttemptDate;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ResultExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/Results
        [HttpPost]
        public async Task<ActionResult<GetResultDto>> PostResult(CreateResultDto dto)
        {
            var newResult = new Result
            {
                ResultId = Guid.NewGuid(),
                AssessmentId = dto.AssessmentId,
                UserId = dto.UserId,
                Score = dto.Score,
                AttemptDate = dto.AttemptDate
            };

            _context.Results.Add(newResult);
            await _context.SaveChangesAsync();

            var resultDto = new GetResultDto
            {
                ResultId = newResult.ResultId,
                AssessmentId = newResult.AssessmentId.Value,
                UserId = newResult.UserId.Value,
                Score = newResult.Score.Value,
                AttemptDate = newResult.AttemptDate.Value
            };

            return CreatedAtAction(nameof(GetResult), new { id = newResult.ResultId }, resultDto);
        }

        // DELETE: api/Results/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResult(Guid id)
        {
            var result = await _context.Results.FindAsync(id);
            if (result == null)
                return NotFound();

            _context.Results.Remove(result);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE via body: api/Results
        [HttpDelete]
        public async Task<IActionResult> DeleteResultByBody([FromBody] DeleteResultDTO dto)
        {
            var result = await _context.Results.FindAsync(dto.ResultId);
            if (result == null)
                return NotFound();

            _context.Results.Remove(result);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ResultExists(Guid id)
        {
            return _context.Results.Any(e => e.ResultId == id);
        }
    }
}
