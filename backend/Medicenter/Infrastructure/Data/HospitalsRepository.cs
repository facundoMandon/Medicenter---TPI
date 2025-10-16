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
        // El constructor recibe el contexto de la base de datos (heredado de RepositoryBase)
        public HospitalsRepository(ApplicationDbContext context) : base(context) { }

        public async Task RegisterProfessionalAsync(int hospitalId, int professionalId)
        {
            // La lógica es la misma que tenías antes:
            var hospital = await _context.Set<Hospitals>()
                                         .Include(h => h.Professionals)
                                         .FirstOrDefaultAsync(h => h.Id == hospitalId);

            if (hospital == null) throw new KeyNotFoundException($"Hospital con ID {hospitalId} no encontrado.");

            var professional = await _context.Set<Professionals>()
                                             .FirstOrDefaultAsync(p => p.Id == professionalId);

            if (professional == null) throw new KeyNotFoundException($"Profesional con ID {professionalId} no encontrado.");

            if (!hospital.Professionals.Any(p => p.Id == professionalId))
            {
                _context.Entry(professional).State = EntityState.Unchanged;
                hospital.Professionals.Add(professional);
                await _context.SaveChangesAsync();
            }
        }

        // 1. AssignProfessionalToHospitalAsync (Corresponde a registrarProfesional)
        public async Task AssignProfessionalToHospitalAsync(int hospitalId, int professionalId)
        {
            // Lógica EF Core para manejar la relación N:M:

            // Buscar la entidad Hospital con su colección de Profesionales
            var hospital = await _context.Set<Hospitals>()
                                         .Include(h => h.Professionals) // Asegura que la colección se cargue
                                         .FirstOrDefaultAsync(h => h.Id == hospitalId);

            if (hospital == null) throw new KeyNotFoundException($"Hospital con ID {hospitalId} no encontrado.");

            // Buscar la entidad Professional (solo para asegurarnos de que exista)
            var professional = await _context.Set<Professionals>()
                                             .FirstOrDefaultAsync(p => p.Id == professionalId);

            if (professional == null) throw new KeyNotFoundException($"Profesional con ID {professionalId} no encontrado.");

            // Verificar si la relación ya existe antes de añadir
            if (!hospital.Professionals.Any(p => p.Id == professionalId))
            {
                // Attach si es necesario, aunque Find o FirstOrDefault ya lo haría
                _context.Entry(professional).State = EntityState.Unchanged;
                hospital.Professionals.Add(professional);
                await _context.SaveChangesAsync();
            }
        }

        // 2. RemoveProfessionalFromHospitalAsync (Corresponde a eliminarProfesional)
        public async Task RemoveProfessionalAsync(int hospitalId, int professionalId)
        {
            // Lógica EF Core para eliminar la relación N:M:

            var hospital = await _context.Set<Hospitals>()
                                         .Include(h => h.Professionals)
                                         .FirstOrDefaultAsync(h => h.Id == hospitalId);

            if (hospital == null) return; // Si no existe el hospital, se considera completada

            // Encontrar el profesional dentro de la colección del hospital
            var professionalToRemove = hospital.Professionals
                                               .FirstOrDefault(p => p.Id == professionalId);

            if (professionalToRemove != null)
            {
                hospital.Professionals.Remove(professionalToRemove);
                await _context.SaveChangesAsync();
            }
        }

        // 3. GetProfessionalsByHospitalIdAsync (Corresponde a listarProfesionales)
        public async Task<List<Professionals>> GetProfessionalsByHospitalIdAsync(int hospitalId)
        {
            // Lógica EF Core para obtener profesionales a través de la relación N:M:

            return await _context.Set<Hospitals>()
                                 // Filtra por el Hospital solicitado
                                 .Where(h => h.Id == hospitalId)
                                 // Proyecta la colección de Profesionales (hace el join implícito)
                                 .SelectMany(h => h.Professionals)
                                 // Incluye datos adicionales del profesional si es necesario (ej: Especialidad)
                                 // .Include(p => p.Specialty) 
                                 .ToListAsync();
        }
    }
}
