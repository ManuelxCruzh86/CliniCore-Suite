using System.ComponentModel.DataAnnotations;

namespace CliniCore.Client.DTOs
{
    public class CreateConsultationDto
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; } // El doctor que atiende

        // Signos Vitales
        public decimal WeightKg { get; set; }
        public decimal HeightCm { get; set; }
        public decimal TemperatureC { get; set; }

        // La Consulta Médica
        [Required(ErrorMessage = "Debes escribir los síntomas.")]
        public string Symptoms { get; set; } = string.Empty;

        [Required(ErrorMessage = "El diagnóstico es obligatorio.")]
        public string Diagnosis { get; set; } = string.Empty;

        [Required(ErrorMessage = "La receta/tratamiento es obligatoria.")]
        public string TreatmentNotes { get; set; } = string.Empty;
    }
}