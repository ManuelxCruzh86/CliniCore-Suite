using System.ComponentModel.DataAnnotations;

namespace CliniCore.Client.DTOs
{
    public class CreatePatientDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public DateTime BirthDate { get; set; } = DateTime.Today;

        [Required]
        public string Gender { get; set; } = "Male";
    }
}