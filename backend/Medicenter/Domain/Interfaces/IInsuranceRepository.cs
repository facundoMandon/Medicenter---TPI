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
        // Cambiar cobertura de un paciente (ya existente)
        Task ChangePatientCoverageAsync(int patientId, MedicalCoverageType newCoverage);

        // 🆕 Buscar una obra social por nombre (para validar duplicados)
        Task<Insurance?> GetByNameAsync(string name);
    }
}
