using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    // Reemplaza 'HospitalDTO' y 'CreationHospitalDTO' con tus nombres reales de DTOs
    public interface IHospitalsService
    {
        Task<HospitalDTO> CreateAsync(CreationHospitalDTO dto);
        Task<HospitalDTO> GetByIdAsync(int id);
        Task<IEnumerable<HospitalDTO>> GetAllAsync();
        Task<HospitalDTO> UpdateAsync(int id, CreationHospitalDTO dto);
        Task DeleteAsync(int id);
    }
}
