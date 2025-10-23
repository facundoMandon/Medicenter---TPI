using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IInsuranceRepository : IRepositoryBase<Insurance>
    {
        // Métodos según diagrama
        Task AddPatientToInsuranceAsync(int insuranceId, int patientId); // añadirAfiliado
        Task RemovePatientFromInsuranceAsync(int insuranceId, int patientId); // eliminarAfiliado
        Task ChangePatientCoverageAsync(int patientId, MedicalCoverageType newCoverage); // cambiarCobertura
    }
}
