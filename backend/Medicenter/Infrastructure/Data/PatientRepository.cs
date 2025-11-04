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
        // El constructor recibe el contexto de la base de datos (heredado de RepositoryBase)
        public PatientRepository(ApplicationDbContext context) : base(context)
        {
        }

        // Implementación de GetAppointmentByPatientIdAsync (verTurnos())
        public async Task<List<Appointment>> GetAppointmentByPatientIdAsync(int patientId)
        {
            // Lógica EF Core: Busca en la tabla Appointment donde PatientId coincide.
            return await _context.Set<Appointment>()
                                 .Where(a => a.PatientId == patientId)
                                 // Opcional: Incluir datos del profesional y especialidad para mostrar en la vista de turnos.
                                 .Include(a => a.Professional)
                                 .Include(a => a.Professional.Specialty)
                                 .ToListAsync();
        }
    }
}
    
