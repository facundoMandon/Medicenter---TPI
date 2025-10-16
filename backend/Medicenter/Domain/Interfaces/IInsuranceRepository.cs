using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IInsuranceRepository : IRepositoryBase<Insurance>
    {
        // Métodos de gestión de Afiliados (Patients)
        Task AddPatientToInsuranceAsync(int insuranceId, int patientId); // añadirAfiliado
        Task RemovePatientFromInsuranceAsync(int insuranceId, int patientId); // eliminarAfiliado
        Task ChangePatientCoverageAsync(int patientId, string newPlan); // cambiarCobertura

        // Métodos de gestión de Profesionales (quiénes aceptan esta Obra Social)
        Task AssignProfessionalToInsuranceAsync(int insuranceId, int professionalId);
        Task RemoveProfessionalFromInsuranceAsync(int insuranceId, int professionalId);
    }
}
