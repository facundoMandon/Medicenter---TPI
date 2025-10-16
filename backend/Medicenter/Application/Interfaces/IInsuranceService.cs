using Application.Models;
using Application.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    // Reemplaza 'InsuranceDTO' y 'CreationInsuranceDTO' con tus nombres reales de DTOs
    public interface IInsuranceService
    {
        // CRUD Básico (Incluye la gestión del Administrador de "EliminarObraSocial")
        Task<InsuranceDTO> GetByIdAsync(int id);
        Task<IEnumerable<InsuranceDTO>> GetAllAsync();
        Task<InsuranceDTO> CreateInsuranceAsync(CreationInsuranceDTO dto);
        Task<InsuranceDTO> UpdateInsuranceAsync(int id, CreationInsuranceDTO dto);
        Task DeleteInsuranceAsync(int id);

        // Métodos de Afiliados
        Task AddAffiliateAsync(int insuranceId, int patientId); // añadirAfiliado
        Task RemoveAffiliateAsync(int insuranceId, int patientId); // eliminarAfiliado
        Task ChangeCoverageAsync(int patientId, string newPlan); // cambiarCobertura
    }
}
