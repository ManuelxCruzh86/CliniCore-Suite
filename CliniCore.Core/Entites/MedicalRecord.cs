namespace CliniCore.Core.Entities
{
    public class MedicalRecord
    {
        public int Id { get; set; }

        // Relación con la Cita 
        public int AppointmentId { get; set; }
        public Appointment? Appointment { get; set; }

        // Anamnesis (Lo que cuenta el paciente)
        public string Symptoms { get; set; } = string.Empty;

        // Examen Físico (Signos vitales - vitales para FHIR "Observation")
        public decimal WeightKg { get; set; } // Peso
        public decimal HeightCm { get; set; } // Altura
        public decimal TemperatureC { get; set; } // Temperatura

        // Diagnóstico (Vital para FHIR "Condition")
        public string Diagnosis { get; set; } = string.Empty; // Como "Hipertensión Arterial"
        public string Icd10Code { get; set; } = string.Empty; // Lo que es el Codigo internacional "I10"

        // Tratamiento (Vital para FHIR "MedicationRequest")
        public string TreatmentNotes { get; set; } = string.Empty;

        // Auditoría
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}