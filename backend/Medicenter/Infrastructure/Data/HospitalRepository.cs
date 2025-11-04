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
    public class HospitalRepository : RepositoryBase<Hospital>, IHospitalRepository
    {
        public HospitalRepository(ApplicationDbContext context) : base(context) { }

        // registrarProfesional
        public async Task RegisterProfessionalAsync(int hospitalId, int professionalId)
        {
            var hospital = await _context.Set<Hospital>()
                .Include(h => h.Professional)
                .FirstOrDefaultAsync(h => h.Id == hospitalId);

            if (hospital == null)
                throw new KeyNotFoundException($"Hospital con ID {hospitalId} no encontrado.");

            var professional = await _context.Set<Professional>()
                .FirstOrDefaultAsync(p => p.Id == professionalId);

            if (professional == null)
                throw new KeyNotFoundException($"Profesional con ID {professionalId} no encontrado.");

            // Verificar si ya existe la relación
            if (!hospital.Professional.Any(p => p.Id == professionalId))
            {
                hospital.Professional.Add(professional);
                await _context.SaveChangesAsync();
            }
        }

        // eliminarProfesionales
        public async Task RemoveProfessionalAsync(int hospitalId, int professionalId)
        {
            var hospital = await _context.Set<Hospital>()
                .Include(h => h.Professional)
                .FirstOrDefaultAsync(h => h.Id == hospitalId);

            if (hospital == null) return;

            var professionalToRemove = hospital.Professional
                .FirstOrDefault(p => p.Id == professionalId);

            if (professionalToRemove != null)
            {
                hospital.Professional.Remove(professionalToRemove);
                await _context.SaveChangesAsync();
            }
        }

        // listarProfesionales
        public async Task<List<Professional>> GetProfessionalByHospitalIdAsync(int hospitalId)
        {
            return await _context.Set<Hospital>()
                .Where(h => h.Id == hospitalId)
                .SelectMany(h => h.Professional)
                .Include(p => p.Specialty)
                .ToListAsync();
        }
    }
}
