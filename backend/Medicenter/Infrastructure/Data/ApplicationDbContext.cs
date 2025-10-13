using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
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
            //Nombres de tablas
            modelBuilder.Entity<Users>()
               .ToTable("Users");
            modelBuilder.Entity<Professionals>()
                .ToTable("Professionals");
            modelBuilder.Entity<Patients>()
                .ToTable("Patients");
            modelBuilder.Entity<Administrators>()
                .ToTable("Administrators");
            modelBuilder.Entity<Appointments>()
                .ToTable("Appointments");
            modelBuilder.Entity<Hospitals>()
                .ToTable("Hospitals");
            modelBuilder.Entity<Insurance>()
                .ToTable("Insurance");
            modelBuilder.Entity<Specialties>()
                .ToTable("Specialties");

         //Relaciones
         //Appointments con Professionals (Uno a muchos)
            modelBuilder.Entity<Appointments>() //para la entidad de turnos defino que
                .HasOne(a => a.Professional) //un profesional
                .WithMany(d => d.Appointments) //va a tener muchos turnos asignados
                .HasForeignKey(a => a.ProfessionalId) //y va a mandar su FK a Turnos
                .OnDelete(DeleteBehavior.Cascade); //con criterio de cascada
                                                   //(qué pasa con las entidades relacionadas cuando se elimina una entidad principal
                                                   //Si eliminás un Doctor, EF automáticamente eliminará todas las citas(Appointments) relacionadas con ese doctor.)

        //Hospital con Professionals (Muchos a Muchos)
            modelBuilder.Entity<Hospitals>()
                .HasMany(h => h.Professionals)
                .WithMany(p => p.Hospitals);

        //Professional con Insurance (Muchos a Muchos)
            modelBuilder.Entity<Professionals>()
                .HasMany(p => p.Insurances)
                .WithMany(i => i.Professionals);

            //Professional con Specialties (Muchos a Uno) 
            modelBuilder.Entity<Professionals>() //->Se parte desde la entidad que tiene la FK
                .HasOne(p => p.Specialty)
                .WithMany(s => s.Professionals)
                .HasForeignKey(p => p.SpecialtyId)
                .OnDelete(DeleteBehavior.Cascade);

            //Patients con Insurance
            modelBuilder.Entity<Patients>() //->Se parte desde la entidad que tiene la FK
                .HasOne(p => p.Insurance)
                .WithMany(i => i.Patients)
                .HasForeignKey(p => p.InsuranceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Appointments>()
                .HasOne(a => a.Patient)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PatientId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
