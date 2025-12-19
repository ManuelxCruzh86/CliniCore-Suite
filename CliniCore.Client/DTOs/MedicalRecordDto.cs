namespace CliniCore.Client.DTOs
{
    public class MedicalRecordDto
    {
        public int Id { get; set; }
        public string Diagnosis { get; set; } = string.Empty;
        public string TreatmentNotes { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }

        // Datos anidados (simplificados)
        public string DoctorName { get; set; } = string.Empty;
    }
}