using System.ComponentModel.DataAnnotations;

namespace CliniCore.Client.DTOs
{
    public class CreateDoctorDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public string Specialty { get; set; } = "General";

        [Required(ErrorMessage = "La cédula profesional es obligatoria.")]
        [RegularExpression(@"^\d{7,8}$", ErrorMessage = "La cédula debe tener entre 7 y 8 dígitos numéricos.")]
        public string LicenseNumber { get; set; } = string.Empty;
    }
}