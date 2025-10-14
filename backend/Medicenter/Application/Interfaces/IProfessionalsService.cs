using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    // Reemplaza 'ProfessionalDTO' y 'CreationProfessionalsDTO' con tus nombres reales de DTOs
    public interface IProfessionalsService
    {
        Task<ProfessionalDTO> CreateAsync(CreationProfessionalsDTO dto);
        // Nota: Asumo que GetById/Update/Delete siguen usando 'int' como ID (matrícula)
        Task<ProfessionalDTO> GetByIdAsync(int id);
        Task<IEnumerable<ProfessionalDTO>> GetAllAsync();
        Task<ProfessionalDTO> UpdateAsync(int id, CreationProfessionalsDTO dto);
        Task DeleteAsync(int id);
    }
}