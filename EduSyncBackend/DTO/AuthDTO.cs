// DTOs/AuthDTOs.cs
using System.ComponentModel.DataAnnotations;

namespace EduSync.DTO
{
    public class RegisterDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [RegularExpression("Student|Instructor", ErrorMessage = "Role must be either 'Student' or 'Instructor'.")]
        public string Role { get; set; } = string.Empty;

        [Required]
        [MinLength(6, ErrorMessage = "Password should be minimum 6 characters.")]
        public string Password { get; set; } = string.Empty;
    }

    public class LoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
