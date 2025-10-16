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
    public class PatientsRepository : RepositoryBase<Patients>, IPatientsRepository
    {
        // El constructor recibe el contexto de la base de datos (heredado de RepositoryBase)
        public PatientsRepository(ApplicationDbContext context) : base(context)
        {
        }

        // Implementación de GetAppointmentsByPatientIdAsync (verTurnos())
        public async Task<List<Appointments>> GetAppointmentsByPatientIdAsync(int patientId)
        {
            // Lógica EF Core: Busca en la tabla Appointments donde PatientId coincide.
            return await _context.Set<Appointments>()
                                 .Where(a => a.PatientId == patientId)
                                 // Opcional: Incluir datos del profesional y especialidad para mostrar en la vista de turnos.
                                 .Include(a => a.Professional)
                                 .Include(a => a.Professional.Specialty)
                                 .ToListAsync();
        }
    }
}
    
