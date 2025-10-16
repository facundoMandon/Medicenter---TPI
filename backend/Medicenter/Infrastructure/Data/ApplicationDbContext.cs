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
        public DbSet<Users> Users { get; set; }
        public DbSet<Administrators> Administrators { get; set; }
        public DbSet<Professionals> Professionals { get; set; }
        public DbSet<Patients> Patients { get; set; }
        public DbSet<Appointments> Appointments { get; set; }
        public DbSet<Hospitals> Hospitals { get; set; }
        public DbSet<Specialties> Specialties { get; set; }
        public DbSet<Insurance> Insurance { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- NOMBRES DE TABLAS Y HERENCIA (TPH) ---

            // 1. TPH Base: Users es la tabla base.
            modelBuilder.Entity<Users>().ToTable("Users");

            // 2. TPH Derivados: NO USAMOS .ToTable() para las clases derivadas
            //    (Professionals, Patients, Administrators) si queremos que compartan 
            //    la tabla "Users" con una columna discriminadora.

            modelBuilder.Entity<Appointments>().ToTable("Appointments");
            modelBuilder.Entity<Hospitals>().ToTable("Hospitals");
            modelBuilder.Entity<Insurance>().ToTable("Insurance");
            modelBuilder.Entity<Specialties>().ToTable("Specialties");


            // --- RELACIONES Y REGLAS DE ELIMINACIÓN ---

            // Appointments con Professionals (Uno a muchos)
            modelBuilder.Entity<Appointments>()
                .HasOne(a => a.Professional)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.ProfessionalId)
                // Usamos RESTRICT para evitar que al borrar un profesional se borren todas sus citas (o que cause un ciclo).
                .OnDelete(DeleteBehavior.Restrict);

            // Appointments con Patients (Uno a muchos)
            modelBuilder.Entity<Appointments>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Restrict);


            // Hospital con Professionals (Muchos a Muchos - EF Core lo maneja automáticamente con una tabla de unión)
            modelBuilder.Entity<Hospitals>()
                .HasMany(h => h.Professionals)
                .WithMany(p => p.Hospitals);


            // Professional con Insurance (Muchos a Muchos)
            modelBuilder.Entity<Professionals>()
                .HasMany(p => p.Insurances)
                .WithMany(i => i.Professionals);


            // Professional con Specialties (Muchos a Uno) 
            modelBuilder.Entity<Professionals>()
                .HasOne(p => p.Specialty)
                .WithMany(s => s.Professionals)
                .HasForeignKey(p => p.SpecialtyId)
                // Si borras una especialidad, ¿qué pasa con los profesionales?
                // Usar RESTRICT: obliga a eliminar o reasignar profesionales antes de borrar la especialidad.
                .OnDelete(DeleteBehavior.Restrict);


            // Patients con Insurance (Muchos a Uno)
            modelBuilder.Entity<Patients>()
                .HasOne(p => p.Insurance)
                .WithMany(i => i.Patients)
                .HasForeignKey(p => p.InsuranceId)
                // Si borras una Obra Social, los pacientes deben ser desvinculados (SET NULL).
                // Si usas RESTRICT, no podrás borrar una Obra Social si tiene pacientes afiliados.
                .OnDelete(DeleteBehavior.SetNull); // <-- Mejor opción aquí.
        }
    }
}