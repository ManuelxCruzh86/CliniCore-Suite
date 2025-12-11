using CliniCore.Core.Entities;
using Hl7.Fhir.Model;
using Patient = CliniCore.Core.Entities.Patient; // Alias para no confundir con FHIR Patient

namespace CliniCore.API.Fhir
{
    public static class FhirPatientMapper
    {
        public static Hl7.Fhir.Model.Patient ToFhir(Patient internalPatient)
        {
            var fhirPatient = new Hl7.Fhir.Model.Patient();

            // ID: En FHIR el ID es string
            fhirPatient.Id = internalPatient.Id.ToString();

            // Nombre: FHIR permite múltiples nombres. Agregamos el Oficial.
            fhirPatient.Name.Add(new HumanName
            {
                Use = HumanName.NameUse.Official,
                Family = internalPatient.LastName,
                Given = new[] { internalPatient.FirstName }
            });

            // Género: Hay que convertir tu string a un Enum de FHIR
            fhirPatient.Gender = internalPatient.Gender.ToLower() switch
            {
                "masculino" => AdministrativeGender.Male,
                "male" => AdministrativeGender.Male,
                "femenino" => AdministrativeGender.Female,
                "female" => AdministrativeGender.Female,
                _ => AdministrativeGender.Unknown
            };

            // Fecha de Nacimiento: Formato YYYY-MM-DD string
            fhirPatient.BirthDate = internalPatient.BirthDate.ToString("yyyy-MM-dd");

            // Metadatos 
            fhirPatient.Meta = new Meta
            {
                LastUpdated = internalPatient.CreatedAt
            };

            return fhirPatient;
        }
    }
}