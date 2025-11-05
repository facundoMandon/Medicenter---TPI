// Infrastructure/Data/ApplicationDbContext.cs
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
// using System.Runtime.Intrinsics.X86; // <--- ELIMINADO

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets para todas las entidades
        public DbSet<User> User { get; set; }
        public DbSet<Administrator> Administrator { get; set; }
        public DbSet<Professional> Professional { get; set; }
        public DbSet<Patient> Patient { get; set; }
        public DbSet<Appointment> Appointment { get; set; }
        public DbSet<Hospital> Hospital { get; set; }
        public DbSet<Specialties> Specialties { get; set; }
        public DbSet<Insurance> Insurance { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- NOMBRES DE TABLAS Y HERENCIA (TPH) ---

            // 1. TPH Base: User es la tabla base.
            modelBuilder.Entity<User>().ToTable("User");

            // 2. TPH Derivados: NO USAMOS .ToTable() para las clases derivadas
            //    (Professional, Patient, Administrator) si queremos que compartan 
            //    la tabla "User" con una columna discriminadora.

            modelBuilder.Entity<Appointment>().ToTable("Appointment");
            modelBuilder.Entity<Hospital>().ToTable("Hospital");
            modelBuilder.Entity<Insurance>().ToTable("Insurance");
            modelBuilder.Entity<Specialties>().ToTable("Specialties");


            // --- RELACIONES Y REGLAS DE ELIMINACIÓN ---

            // Appointment con Professional (Uno a muchos)
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Professional)
                .WithMany(d => d.Appointment)
                .HasForeignKey(a => a.ProfessionalId)
                // Usamos RESTRICT para evitar que al borrar un profesional se borren todas sus citas (o que cause un ciclo).
                .OnDelete(DeleteBehavior.Restrict);

            // Appointment con Patient (Uno a muchos)
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointment)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);


            // Hospital con Professional (Muchos a Muchos - EF Core lo maneja automáticamente con una tabla de unión)
            modelBuilder.Entity<Hospital>()
                .HasMany(h => h.Professional)
                .WithMany(p => p.Hospital);


            // Professional con Insurance (Muchos a Muchos)
            modelBuilder.Entity<Professional>()
                .HasMany(p => p.Insurance)
                .WithMany(i => i.Professional);


            // Professional con Specialties (Muchos a Uno) 
            modelBuilder.Entity<Professional>()
                .HasOne(p => p.Specialty)
                .WithMany(s => s.Professional)
                .HasForeignKey(p => p.SpecialtyId)
                // Si borras una especialidad, ¿qué pasa con los profesionales?
                // Usar RESTRICT: obliga a eliminar o reasignar profesionales antes de borrar la especialidad.
                .OnDelete(DeleteBehavior.Restrict);


            // Patient con Insurance (Muchos a Uno)
            modelBuilder.Entity<Patient>()
                .HasOne(p => p.Insurance)
                .WithMany(i => i.Patient)
                .HasForeignKey(p => p.InsuranceId)
                // Si borras una Obra Social, los pacientes deben ser desvinculados (SET NULL).
                // Si usas RESTRICT, no podrás borrar una Obra Social si tiene pacientes afiliados.
                .OnDelete(DeleteBehavior.SetNull); // <-- Mejor opción aquí.
        }
    }
}