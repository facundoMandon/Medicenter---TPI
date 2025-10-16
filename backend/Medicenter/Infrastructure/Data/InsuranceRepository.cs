using Domain.Entities;
using Domain.Interfaces;
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

        // --- Gestión de Afiliados ---

        public Task AddPatientToInsuranceAsync(int insuranceId, int patientId) // añadirAfiliado
        {
            // Lógica EF Core: Buscar Patient y asignar InsuranceId.
            return Task.CompletedTask;
        }

        public Task RemovePatientFromInsuranceAsync(int insuranceId, int patientId) // eliminarAfiliado
        {
            // Lógica EF Core: Buscar Patient y establecer InsuranceId a null/0.
            return Task.CompletedTask;
        }

        public Task ChangePatientCoverageAsync(int patientId, string newPlan) // cambiarCobertura
        {
            // Lógica EF Core: Buscar Patient y actualizar su Plan (o el campo que vincule la cobertura).
            return Task.CompletedTask;
        }

        // --- Gestión de Profesionales ---

        public Task AssignProfessionalToInsuranceAsync(int insuranceId, int professionalId)
        {
            // Lógica EF Core: Añadir la entrada en la tabla intermedia N:M.
            return Task.CompletedTask;
        }

        public Task RemoveProfessionalFromInsuranceAsync(int insuranceId, int professionalId)
        {
            // Lógica EF Core: Eliminar la entrada en la tabla intermedia N:M.
            return Task.CompletedTask;
        }
    }
}
