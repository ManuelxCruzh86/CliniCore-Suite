using System.ComponentModel.DataAnnotations;

namespace CliniCore.API.DTOs
{
    public class CreateAppointmentDto
    {
        [Required]
        public int PatientId { get; set; } // ¿Quien es el paciente?

        [Required]
        public int DoctorId { get; set; } // ¿Quien lo atiende?

        [Required]
        public DateTime ScheduledDate { get; set; } // ¿Cuando?

        [Required]
        public string Reason { get; set; } = string.Empty; // Razon
    }
}