using Microsoft.EntityFrameworkCore;
using PrescrioptionManagement.Shared;

namespace PrescrioptionManagement.Server.Models
{
    public class PrescriptionDbContext : DbContext
    {
        public PrescriptionDbContext(DbContextOptions<PrescriptionDbContext> options) : base(options)
        {

        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<MedicalTest> MedicalTests { get; set; }
        public DbSet<TestEntry> TestEntries { get; set; }
    }
}
