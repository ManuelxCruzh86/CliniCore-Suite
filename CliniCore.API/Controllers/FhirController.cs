using CliniCore.Infrastructure.Data;
using CliniCore.API.Fhir;
using Hl7.Fhir.Model;
using Hl7.Fhir.Serialization; // Convertir el objeto FHIR a texto JSON
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CliniCore.API.Controllers
{
    [Route("api/fhir")]
    [ApiController]
    [Authorize]
    public class FhirController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FhirController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/fhir/Patient/1
        // Este endpoint devuelve un recurso FHIR estándar válido
        [HttpGet("Patient/{id}")]
        public async Task<IActionResult> GetFhirPatient(int id)
        {
            // Buscamos en NUESTRA base de datos
            var patient = await _context.Patients.FindAsync(id);

            if (patient == null)
            {
                // En FHIR, un 404 también debería devolver un OperationOutcome, 
                return NotFound();
            }

            // TRADUCIMOS (Mapping)
            var fhirPatient = FhirPatientMapper.ToFhir(patient);

            // SERIALIZAMOS (Convertir a JSON formato FHIR)
            var serializer = new FhirJsonSerializer();
            var json = serializer.SerializeToString(fhirPatient);

            // RETORNAMOS con el tipo de contenido correcto para salud
            return Content(json, "application/fhir+json");
        }

        // GET: api/fhir/Patient/1/Everything
        [HttpGet("Patient/{id}/Everything")]
        public async Task<IActionResult> GetPatientEverything(int id)
        {
            // Validar paciente
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null) return NotFound();

            // Buscar sus expedientes médicos
            var records = await _context.MedicalRecords
                                        .Where(r => r.Appointment.PatientId == id)
                                        .ToListAsync();

            // Crear un "Bundle" (Un paquete de documentos FHIR)
            var bundle = new Bundle
            {
                Type = Bundle.BundleType.Collection,
                Id = Guid.NewGuid().ToString()
            };

            // Agregar al Paciente mismo al paquete
            bundle.Entry.Add(new Bundle.EntryComponent { Resource = FhirPatientMapper.ToFhir(patient) });

            // Recorrer expedientes y convertirlos a FHIR
            foreach (var record in records)
            {
                // Convertir Diagnóstico
                var condition = FhirClinicalMapper.ToFhirCondition(record, id.ToString());
                bundle.Entry.Add(new Bundle.EntryComponent { Resource = condition });

                // Convertir Peso (Solo si hay peso registrado)
                if (record.WeightKg > 0)
                {
                    var weight = FhirClinicalMapper.ToFhirWeightObservation(record, id.ToString());
                    bundle.Entry.Add(new Bundle.EntryComponent { Resource = weight });
                }
            }

            // Serializar y enviar
            var serializer = new FhirJsonSerializer();
            var json = serializer.SerializeToString(bundle);

            return Content(json, "application/fhir+json");
        }
    }
}