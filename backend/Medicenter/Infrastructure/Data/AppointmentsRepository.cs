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
            return await _context.Set<Appointments>()
                .Include(a => a.Professional)
                    .ThenInclude(p => p.Specialty)
                .Include(a => a.Patient)
                .Where(a => a.PatientId == patientId)
                .ToListAsync();
        }

        public async Task<List<Appointments>> GetByProfessionalIdAsync(int professionalId)
        {
            return await _context.Set<Appointments>()
                .Include(a => a.Patient)
                    .ThenInclude(p => p.Insurance)
                .Include(a => a.Professional)
                .Where(a => a.ProfessionalId == professionalId)
                .ToListAsync();
        }
    }
}
