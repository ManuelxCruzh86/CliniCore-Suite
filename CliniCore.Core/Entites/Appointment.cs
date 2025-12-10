namespace CliniCore.Core.Entities
{
    public class Appointment
    {
        public int Id { get; set; }

        //  RELACIÓN CON PACIENTE 
        public int PatientId { get; set; } // llave foránea
        public Patient? Patient { get; set; } // el objeto completo

        // RELACIÓN CON DOCTOR 
        public int DoctorId { get; set; } // llave foránea
        public Doctor? Doctor { get; set; } // La navegación

        // DATOS DE LA CITA 
        public DateTime ScheduledDate { get; set; } // Fecha es la cita
        public string Reason { get; set; } = string.Empty; // Motivo 
        public string Status { get; set; } = "Scheduled"; // Scheduled, Completed, Cancelled

        // Auditoría
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}