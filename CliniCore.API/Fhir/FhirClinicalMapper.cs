using CliniCore.Core.Entities;
using Hl7.Fhir.Model; 
using System.Collections.Generic;

namespace CliniCore.API.Fhir
{
    public static class FhirClinicalMapper
    {
        // Método para convertir tu Diagnóstico en una "Condition" de FHIR
        public static Condition ToFhirCondition(MedicalRecord record, string patientId)
        {
            var condition = new Condition();

            // ID Único (el ID de tu record + prefijo)
            condition.Id = $"cond-{record.Id}";

            // Estado Clínico (Activo)
            condition.ClinicalStatus = new CodeableConcept
            {
                Coding = new List<Coding>
                {
                    new Coding("http://terminology.hl7.org/CodeSystem/condition-clinical", "active")
                }
            };

            // El Código del Diagnóstico (CIE-10) -> Importante
            condition.Code = new CodeableConcept
            {
                Text = record.Diagnosis,
                Coding = new List<Coding>
                {
                    new Coding("http://hl7.org/fhir/sid/icd-10", record.Icd10Code, record.Diagnosis)
                }
            };

            // A quién pertenece (Referencia al Paciente)
            condition.Subject = new ResourceReference($"Patient/{patientId}");

            // Cuándo se registró
            condition.RecordedDateElement = new FhirDateTime(record.CreatedAt);

            return condition;
        }

        // Método convertir el Peso en una "Observation" FHIR
        public static Observation ToFhirWeightObservation(MedicalRecord record, string patientId)
        {
            var observation = new Observation();

            observation.Id = $"obs-weight-{record.Id}";
            observation.Status = ObservationStatus.Final;

            // Código LOINC para "Peso corporal" (Estándar mundial)
            observation.Code = new CodeableConcept
            {
                Coding = new List<Coding>
                {
                    new Coding("http://loinc.org", "29463-7", "Body Weight")
                }
            };

            // El valor numérico y la unidad
            observation.Value = new Quantity(record.WeightKg, "kg", "http://unitsofmeasure.org");

            observation.Subject = new ResourceReference($"Patient/{patientId}");
            observation.Effective = new FhirDateTime(record.CreatedAt);

            return observation;
        }
    }
}