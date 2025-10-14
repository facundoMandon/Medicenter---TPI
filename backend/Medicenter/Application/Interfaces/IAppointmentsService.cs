using Application.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    // Reemplaza 'AppointmentsDTO' y 'CreationAppointmentsDTO' con tus nombres reales de DTOs
    public interface IAppointmentsService
    {
        Task<AppointmentsDTO> CreateAsync(CreationAppointmentsDTO dto);
        Task<AppointmentsDTO> GetByIdAsync(int id);
        Task<IEnumerable<AppointmentsDTO>> GetAllAsync();
        Task<AppointmentsDTO> UpdateAsync(int id, CreationAppointmentsDTO dto);
        Task DeleteAsync(int id);
    }
}
