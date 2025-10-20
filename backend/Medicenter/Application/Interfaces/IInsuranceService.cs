using Application.Models;
using Application.Models.Request;
using Domain.Enums;
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
        // CRUD Básico
        Task<InsuranceDTO> GetByIdAsync(int id);
        Task<IEnumerable<InsuranceDTO>> GetAllAsync();
        Task<InsuranceDTO> CreateInsuranceAsync(CreationInsuranceDTO dto);
        Task<InsuranceDTO> UpdateInsuranceAsync(int id, CreationInsuranceDTO dto);
        Task DeleteInsuranceAsync(int id);

        // Métodos según diagrama
        Task AddAffiliateAsync(int insuranceId, int patientId); // añadirAfiliado()
        Task RemoveAffiliateAsync(int insuranceId, int patientId); // eliminarAfiliado()
        Task ChangeCoverageAsync(int patientId, MedicalCoverageType newCoverage); // cambiarCobertura()
    }
}
