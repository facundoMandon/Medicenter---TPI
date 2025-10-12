using Domain.Entities;
using Domain.Entities.Relations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) 
        {
        
        }

        public DbSet<Users> Users { get; set; }
        public DbSet<Appointments> Appointments { get; set; }
        public DbSet<Hospitals> Hospitals { get; set; }
        public DbSet<Insurance> Insurance { get; set; }


        public DbSet<Specialties> Specialties { get; set; }


        // DbSets para las Entidades de Unión (M:M) (relaciones)
        public DbSet<ProfessionalSpecialty> ProfessionalSpecialties { get; set; }
        public DbSet<ProfessionalHospital> ProfessionalHospitals { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //acá configuramos las relaciones, restricciones, etc

            modelBuilder.Entity<Users>()
               .ToTable("Users");

            modelBuilder.Entity<Professionals>()
                .ToTable("Professionals");

            modelBuilder.Entity<Patients>()
                .ToTable("Patients");

            modelBuilder.Entity<Administrators>()
                .ToTable("Administrators");

            // 1. Profesional <-> Especialidad (a través de ProfessionalSpecialty)
            modelBuilder.Entity<ProfessionalSpecialty>()
                .HasKey(pe => new { pe.ProfessionalId, pe.SpecialtyId }); // Clave Compuesta

            modelBuilder.Entity<ProfessionalSpecialty>()
                .HasOne(pe => pe.Professional)
                .WithMany(p => p.EspecialidadesEnlazadas)
                .HasForeignKey(pe => pe.ProfessionalId);

            modelBuilder.Entity<ProfessionalSpecialty>()
                .HasOne(pe => pe.Specialty)
                .WithMany(e => e.ProfessionalsEnlazados)
                .HasForeignKey(pe => pe.SpecialtyId);

            // 2. Profesional <-> Hospital (a través de ProfessionalHospital)
            modelBuilder.Entity<ProfessionalHospital>()
                .HasKey(ph => new { ph.ProfessionalId, ph.HospitalId }); // Clave Compuesta

            modelBuilder.Entity<ProfessionalHospital>()
                .HasOne(ph => ph.Professional)
                .WithMany(p => p.HospitalesEnlazados)
                .HasForeignKey(ph => ph.ProfessionalId);

            modelBuilder.Entity<ProfessionalHospital>()
                .HasOne(ph => ph.Hospital)
                // ASUMIDO: Debes agregar la ICollection<ProfessionalHospital> a la entidad Hospitals
                .WithMany()
                .HasForeignKey(ph => ph.HospitalId);
        }
    }
}
