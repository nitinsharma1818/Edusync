using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EduSync.Data;
using EduSync.Models;
using EduSync.DTO;
using Microsoft.AspNetCore.Authorization;

namespace EduSync.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly EduSyncContext _context;

        public UsersController(EduSyncContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetUserDTO>>> GetUsers()
        {
            return await _context.Users
                .Select(user => new GetUserDTO
                {
                    UserId = user.UserId,
                    Name = user.Name!,
                    Email = user.Email!,
                    Role = user.Role!
                })
                .ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GetUserDTO>> GetUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var getUserDTO = new GetUserDTO
            {
                UserId = user.UserId,
                Name = user.Name!,
                Email = user.Email!,
                Role = user.Role!
            };

            return getUserDTO;
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid id, PutUserDTO putUserDTO)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            user.Name = putUserDTO.Name;
            user.Email = putUserDTO.Email;
            user.Role = putUserDTO.Role;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<GetUserDTO>> PostUser([FromBody] CreateUserDTO createUserDTO)
        {
            var user = new User
            {
                UserId = Guid.NewGuid(),
                Name = createUserDTO.Name,
                Email = createUserDTO.Email,
                Role = createUserDTO.Role,
                PasswordHash = createUserDTO.PasswordHash
            };

            _context.Users.Add(user);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (UserExists(user.UserId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            var getUserDTO = new GetUserDTO
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            };

            return CreatedAtAction("GetUser", new { id = user.UserId }, getUserDTO);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<DeleteUserDTO>> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var deletedUserDto = new DeleteUserDTO
            {
                UserId = user.UserId,
                Name = user.Name!,
                Email = user.Email!,
                Role = user.Role!
            };

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return deletedUserDto;
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
