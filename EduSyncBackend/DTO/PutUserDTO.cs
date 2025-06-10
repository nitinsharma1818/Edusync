namespace EduSync.DTO
{
    public class PutUserDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        // Password update should be done separately via password reset/change API for security.
    }
}
