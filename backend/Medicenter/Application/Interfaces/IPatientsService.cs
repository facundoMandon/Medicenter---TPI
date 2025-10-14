using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    // Reemplaza 'PatientDTO' y 'CreationPatientDTO' con tus nombres reales de DTOs
    public interface IPatientsService
    {
        Task<PatientDTO> CreateAsync(CreationPatientDTO dto);
        Task<PatientDTO> GetByIdAsync(int id);
        Task<IEnumerable<PatientDTO>> GetAllAsync();
        Task<PatientDTO> UpdateAsync(int id, CreationPatientDTO dto);
        Task DeleteAsync(int id);
    }
}