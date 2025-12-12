using CliniCore.Core.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace CliniCore.API.Services
{
	public static class PrescriptionService
	{
		// Constructor estático para la licencia
		static PrescriptionService()
		{
			QuestPDF.Settings.License = LicenseType.Community;
		}

		public static byte[] GeneratePdf(MedicalRecord record)
		{
			return Document.Create(container =>
			{
				container.Page(page =>
				{
					page.Size(PageSizes.A4);
					page.Margin(2, Unit.Centimetre);
					page.PageColor(Colors.White);
					page.DefaultTextStyle(x => x.FontSize(11));

					// 1. ENCABEZADO 
					page.Header().Row(row =>
					{
						// Lado Izquierdo: Título y Folio
						row.RelativeItem().Column(col =>
						{
							col.Item().Text("CLINICORE SUITE").Bold().FontSize(20).FontColor(Colors.Blue.Medium);
							col.Item().Text("Sistema de Gestión Médica").FontSize(10).FontColor(Colors.Grey.Medium);
							col.Item().Text($"Receta Médica #{record.Id}").FontSize(10).FontColor(Colors.Red.Medium);
						});

						// Lado Derecho: Datos del Doctor 
						row.RelativeItem().AlignRight().Column(col =>
						{
							var doc = record.Appointment?.Doctor;
							col.Item().Text($"Dr. {doc?.FirstName} {doc?.LastName}").Bold();
							col.Item().Text($"Esp: {doc?.Specialty}");
							col.Item().Text($"Cédula: {doc?.LicenseNumber}");
						});
					});

					// CONTENIDO PRINCIPAL
					page.Content().PaddingVertical(1, Unit.Centimetre).Column(col =>
					{
						// Datos del Paciente 
						col.Item().Background(Colors.Grey.Lighten4).Padding(10).Column(c =>
						{
							var pat = record.Appointment?.Patient;
							c.Item().Text($"PACIENTE: {pat?.FirstName} {pat?.LastName}").Bold();
							c.Item().Text($"Edad: {DateTime.Now.Year - pat?.BirthDate.Year} años | Género: {pat?.Gender}");
							c.Item().Text($"Fecha de Consulta: {record.CreatedAt.ToShortDateString()}");
						});

						col.Item().PaddingVertical(10); // Espacio

						// Tabla de Signos Vitales 
						col.Item().Table(table =>
						{
							// Definimos 3 columnas
							table.ColumnsDefinition(columns =>
							{
								columns.RelativeColumn();
								columns.RelativeColumn();
								columns.RelativeColumn();
							});

							// Encabezados
							table.Header(header =>
							{
								header.Cell().Text("PESO (Kg)").Bold();
								header.Cell().Text("ALTURA (cm)").Bold();
								header.Cell().Text("TEMP (°C)").Bold();
							});

							// Datos
							table.Cell().Text($"{record.WeightKg} kg");
							table.Cell().Text($"{record.HeightCm} cm");
							table.Cell().Text($"{record.TemperatureC} °C");
						});

						col.Item().LineHorizontal(1).LineColor(Colors.Grey.Lighten2);
						col.Item().PaddingVertical(10);

						// Historia Clínica
						col.Item().Text("SÍNTOMAS / MOTIVO:").Bold().FontColor(Colors.Blue.Darken2);
						col.Item().Text(record.Symptoms);

						col.Item().PaddingVertical(5);

						col.Item().Text("DIAGNÓSTICO MÉDICO:").Bold().FontColor(Colors.Blue.Darken2);
						col.Item().Text($"{record.Diagnosis} (CIE-10: {record.Icd10Code})").FontSize(12);

						col.Item().PaddingVertical(10);
						col.Item().LineHorizontal(1).LineColor(Colors.Black); // Línea divisoria 
						col.Item().PaddingVertical(10);

						// Tratamiento
						col.Item().Text("PLAN DE TRATAMIENTO (Rx):").Bold().FontSize(14).Underline();
						col.Item().PaddingTop(5).Text(record.TreatmentNotes).FontSize(12).Italic();
					});

					// PIE DE PÁGINA (Firma)
					page.Footer().Column(col =>
					{
						col.Item().PaddingTop(30).AlignCenter().Text("_________________________");
						col.Item().AlignCenter().Text("Firma del Médico");

						col.Item().PaddingTop(10).AlignCenter().Text(x =>
						{
							x.Span("Documento generado electrónicamente en ").FontSize(8);
							x.Span($"{System.Environment.OSVersion}").FontSize(8).Bold(); 
						});
					});
				});
			})
			.GeneratePdf();
		}
	}
}