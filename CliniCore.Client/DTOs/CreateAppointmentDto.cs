using System.ComponentModel.DataAnnotations;

namespace CliniCore.Client.DTOs
{
    public class CreateAppointmentDto
    {
        [Required]
        public int PatientId { get; set; }

        [Required]
        public int DoctorId { get; set; }

        [Required]
        public DateTime ScheduledDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Debes escribir el motivo de la cita.")]
        public string Reason { get; set; } = string.Empty;
    }
}