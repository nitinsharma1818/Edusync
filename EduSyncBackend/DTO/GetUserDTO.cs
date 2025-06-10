// DTO/GetUserDTO.cs
using System;

namespace EduSync.DTO
{
    public class GetUserDTO
    {
        public Guid UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }
    }
}
