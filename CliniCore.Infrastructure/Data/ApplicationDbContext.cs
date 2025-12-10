using Microsoft.EntityFrameworkCore;
using CliniCore.Core.Entities; 

namespace CliniCore.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Se Crea una tabla Patients usando la estructura de la clase Patient
        public DbSet<Patient> Patients { get; set; }
    }
}