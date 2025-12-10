using System.ComponentModel.DataAnnotations;

namespace CliniCore.API.DTOs
{
    public class CreateDoctorDto
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string Specialty { get; set; } = string.Empty; 

        [Required]
        public string LicenseNumber { get; set; } = string.Empty; 
    }
}