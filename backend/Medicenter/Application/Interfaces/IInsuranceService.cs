using Application.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    // Reemplaza 'InsuranceDTO' y 'CreationInsuranceDTO' con tus nombres reales de DTOs
    public interface IInsuranceService
    {
        Task<InsuranceDTO> CreateAsync(CreationInsuranceDTO dto);
        Task<InsuranceDTO> GetByIdAsync(int id);
        Task<IEnumerable<InsuranceDTO>> GetAllAsync();
        Task<InsuranceDTO> UpdateAsync(int id, CreationInsuranceDTO dto);
        Task DeleteAsync(int id);
    }
}
