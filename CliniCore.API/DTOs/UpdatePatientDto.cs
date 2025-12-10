using System.ComponentModel.DataAnnotations;

namespace CliniCore.API.DTOs
{
    public class UpdatePatientDto
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        public string Gender { get; set; } = string.Empty;

        public bool IsActive { get; set; } // activar/desactivar
    }
}