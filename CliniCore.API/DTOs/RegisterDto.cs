using System.ComponentModel.DataAnnotations;

namespace CliniCore.API.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = "Doctor"; // "Admin", "Doctor", "Secretary"
    }
}