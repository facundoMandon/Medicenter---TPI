using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class InsuranceRepository : RepositoryBase<Insurance>, IInsuranceRepository
    {
        public InsuranceRepository(ApplicationDbContext context) : base(context) { }

        // cambiarCobertura
        public async Task ChangePatientCoverageAsync(int patientId, MedicalCoverageType newCoverage)
        {
            var patient = await _context.Set<Patients>()
                .Include(p => p.Insurance)
                .FirstOrDefaultAsync(p => p.Id == patientId);

            if (patient == null)
                throw new KeyNotFoundException($"Patient with ID {patientId} not found.");

            if (patient.Insurance != null)
            {
                patient.Insurance.TipoCobertura = newCoverage;
                await _context.SaveChangesAsync();
            }
        }
    }
}
