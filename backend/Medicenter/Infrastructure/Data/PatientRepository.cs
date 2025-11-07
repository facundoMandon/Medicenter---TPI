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
    public class PatientRepository : RepositoryBase<Patient>, IPatientRepository
    {
        public PatientRepository(ApplicationDbContext context) : base(context)
        {
        }

        // 🔹 Obtener los turnos de un paciente (verTurnos)
        public async Task<List<Appointment>> GetAppointmentByPatientIdAsync(int patientId)
        {
            return await _context.Set<Appointment>()
                                 .Where(a => a.PatientId == patientId)
                                 .Include(a => a.Professional)
                                 .Include(a => a.Professional.Specialty)
                                 .ToListAsync();
        }

        // 🔹 Buscar paciente por DNI
        public async Task<Patient?> GetByDniAsync(int dni)
        {
            return await _context.Set<Patient>()
                                 .FirstOrDefaultAsync(p => p.DNI == dni);
        }

        // 🔹 Buscar paciente por Email
        public async Task<Patient?> GetByEmailAsync(string email)
        {
            return await _context.Set<Patient>()
                                 .FirstOrDefaultAsync(p => p.Email == email);
        }
    }
}

