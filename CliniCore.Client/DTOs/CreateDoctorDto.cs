using System.ComponentModel.DataAnnotations;

namespace CliniCore.Client.DTOs
{
    public class CreateDoctorDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ\s]+$", ErrorMessage = "El nombre solo puede contener letras.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ\s]+$", ErrorMessage = "El apellido solo puede contener letras.")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^[a-zA-ZñÑáéíóúÁÉÍÓÚ\s]+$", ErrorMessage = "La especialidad no puede contener números.")]
        public string Specialty { get; set; } = "General";

        [Required(ErrorMessage = "La cédula profesional es obligatoria.")]
        [RegularExpression(@"^\d{7,8}$", ErrorMessage = "La cédula debe ser solo números (7 u 8 dígitos).")]
        public string LicenseNumber { get; set; } = string.Empty;
    }
}