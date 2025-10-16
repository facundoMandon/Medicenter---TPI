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
    public class AppointmentsRepository : RepositoryBase<Appointments>, IAppointmentsRepository
    {
        public AppointmentsRepository(ApplicationDbContext context) : base(context) { }

        public async Task<List<Appointments>> GetByPatientIdAsync(int patientId)
        {
            // Implementación real con EF Core
            return await _context.Set<Appointments>()
                                 .Where(a => a.PatientId == patientId)
                                 .ToListAsync();
        }

        public async Task<List<Appointments>> GetByProfessionalIdAsync(int professionalId)
        {
            // Implementación real con EF Core
            return await _context.Set<Appointments>()
                                 .Where(a => a.ProfessionalId == professionalId)
                                 .ToListAsync();
        }
    }
}
