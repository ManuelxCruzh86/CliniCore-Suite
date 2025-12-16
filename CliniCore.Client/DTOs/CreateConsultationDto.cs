using System.ComponentModel.DataAnnotations;

namespace CliniCore.Client.DTOs
{
    public class CreateConsultationDto
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public decimal WeightKg { get; set; }
        public decimal HeightCm { get; set; }
        public decimal TemperatureC { get; set; }
        [Required] public string Symptoms { get; set; } = "";
        [Required] public string Diagnosis { get; set; } = "";
        [Required] public string TreatmentNotes { get; set; } = "";
    }
}