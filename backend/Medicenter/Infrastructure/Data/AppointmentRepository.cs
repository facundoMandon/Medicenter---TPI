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
    public class AppointmentRepository : RepositoryBase<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(ApplicationDbContext context) : base(context) { }

        public async Task<List<Appointment>> GetByPatientIdAsync(int patientId)
        {
            return await _context.Set<Appointment>()
                .Include(a => a.Professional)
                    .ThenInclude(p => p.Specialty)
                .Include(a => a.Patient)
                .Where(a => a.PatientId == patientId)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetByProfessionalIdAsync(int professionalId)
        {
            return await _context.Set<Appointment>()
                .Include(a => a.Patient)
                    .ThenInclude(p => p.Insurance)
                .Include(a => a.Professional)
                .Where(a => a.ProfessionalId == professionalId)
                .ToListAsync();
        }

        public async Task<Appointment> AddAsync(Appointment appointment)
        {
            _context.Set<Appointment>().Add(appointment);
            await _context.SaveChangesAsync();

            // Recargar el turno con los datos del profesional y la especialidad
            await _context.Entry(appointment)
                .Reference(a => a.Professional)
                .Query()
                .Include(p => p.Specialty)
                .LoadAsync();

            return appointment;
        }
    }
}
