using System.ComponentModel.DataAnnotations;

namespace CliniCore.API.DTOs
{
    public class CreateMedicalRecordDto
    {
        [Required]
        public int AppointmentId { get; set; } // Vinculación con la cita

        [Required]
        public string Symptoms { get; set; } = string.Empty;

        // Signos Vitales
        public decimal WeightKg { get; set; }
        public decimal HeightCm { get; set; }
        public decimal TemperatureC { get; set; }

        [Required]
        public string Diagnosis { get; set; } = string.Empty;

        // Campos Clave para el FHIR futuro
        [Required]
        public string Icd10Code { get; set; } = string.Empty; // Ej: "J00"

        public string TreatmentNotes { get; set; } = string.Empty;
    }
}