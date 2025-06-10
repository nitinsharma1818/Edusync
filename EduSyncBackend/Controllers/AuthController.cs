using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using EduSync.Data;
using EduSync.DTO;
using EduSync.Models;
using EduSync.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BCrypt.Net;

namespace EduSync.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly EduSyncContext _context;
        private readonly JwtService _jwtService;

        public AuthController(EduSyncContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { Error = "Invalid token." });
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == Guid.Parse(userId));
            if (user == null)
            {
                return NotFound(new { Error = "User not found." });
            }

            return Ok(new
            {
                Id = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);
                return BadRequest(new { Errors = errors });
            }

            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                return BadRequest(new { Error = "Email already registered." });

            if (!new[] { "Student", "Instructor" }.Contains(dto.Role))
                return BadRequest(new { Error = "Role must be either 'Student' or 'Instructor'." });

            var user = new User
            {
                UserId = Guid.NewGuid(),
                Name = dto.Name,
                Email = dto.Email,
                Role = dto.Role,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = _jwtService.GenerateToken(user);

            return Ok(new { 
                Token = token,
                User = new {
                    Id = user.UserId,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role
                }
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage);
                return BadRequest(new { Errors = errors });
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            
            // If user is null or password hash is null, return unauthorized
            if (user?.PasswordHash == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            // Verify password using BCrypt
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
            {
                return Unauthorized("Invalid email or password.");
            }

            var token = _jwtService.GenerateToken(user);

            return Ok(new { 
                Token = token,
                User = new {
                    Id = user.UserId,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role
                }
            });
        }

        // Simple SHA256 hash (for demo). Use stronger hashing (e.g., BCrypt) in production.
        private string HashPassword(string password)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            var hashOfInput = HashPassword(password);
            return hashOfInput == storedHash;
        }
    }
}
