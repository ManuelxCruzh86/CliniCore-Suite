using System.ComponentModel.DataAnnotations; // Validar datos obligatorios

namespace CliniCore.API.DTOs
{
    public class CreatePatientDto
    {
        [Required] // Obligatorio
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public DateTime BirthDate { get; set; }

        public string Gender { get; set; } = string.Empty;

        //[EmailAddress] 
        //public string? Email { get; set; }

        // NO ponemos 'Id', ni 'CreatedAt', ni 'IsActive', Esos los controlamos nosotros, no el usuario.
        
    }
}