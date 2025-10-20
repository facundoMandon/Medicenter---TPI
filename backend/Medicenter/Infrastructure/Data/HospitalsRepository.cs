using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    // Asumimos que tienes una tabla de unión (ej: ProfessionalHospital) para la relación N:M
    public class HospitalsRepository : RepositoryBase<Hospitals>, IHospitalsRepository
    {
        public HospitalsRepository(ApplicationDbContext context) : base(context) { }

        // registrarProfesional
        public async Task RegisterProfessionalAsync(int hospitalId, int professionalId)
        {
            var hospital = await _context.Set<Hospitals>()
                .Include(h => h.Professionals)
                .FirstOrDefaultAsync(h => h.Id == hospitalId);

            if (hospital == null)
                throw new KeyNotFoundException($"Hospital con ID {hospitalId} no encontrado.");

            var professional = await _context.Set<Professionals>()
                .FirstOrDefaultAsync(p => p.Id == professionalId);

            if (professional == null)
                throw new KeyNotFoundException($"Profesional con ID {professionalId} no encontrado.");

            // Verificar si ya existe la relación
            if (!hospital.Professionals.Any(p => p.Id == professionalId))
            {
                hospital.Professionals.Add(professional);
                await _context.SaveChangesAsync();
            }
        }

        // eliminarProfesionales
        public async Task RemoveProfessionalAsync(int hospitalId, int professionalId)
        {
            var hospital = await _context.Set<Hospitals>()
                .Include(h => h.Professionals)
                .FirstOrDefaultAsync(h => h.Id == hospitalId);

            if (hospital == null) return;

            var professionalToRemove = hospital.Professionals
                .FirstOrDefault(p => p.Id == professionalId);

            if (professionalToRemove != null)
            {
                hospital.Professionals.Remove(professionalToRemove);
                await _context.SaveChangesAsync();
            }
        }

        // listarProfesionales
        public async Task<List<Professionals>> GetProfessionalsByHospitalIdAsync(int hospitalId)
        {
            return await _context.Set<Hospitals>()
                .Where(h => h.Id == hospitalId)
                .SelectMany(h => h.Professionals)
                .Include(p => p.Specialty)
                .ToListAsync();
        }
    }
}
