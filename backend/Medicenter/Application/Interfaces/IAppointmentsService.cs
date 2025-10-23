using Application.Models;
using Application.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    // Reemplaza 'AppointmentsDTO' y 'CreationAppointmentsDTO' con tus nombres reales de DTOs
    public interface IAppointmentsService
    {
        // CRUD Base
        Task<AppointmentsDTO> GetByIdAsync(int id);
        Task<IEnumerable<AppointmentsDTO>> GetAllAsync();

        // Métodos de negocio (según diagrama)
        Task<AppointmentsDTO> AssignAppointmentAsync(CreationAppointmentDTO dto); // asignarTurno()
        Task<AppointmentsDTO> UpdateAppointmentAsync(int appointmentId, CreationAppointmentDTO dto); // modificarTurno()
        Task CancelAppointmentAsync(int appointmentId); // cancelarTurno()
        Task<AppointmentsDTO> ConfirmAppointmentAsync(int appointmentId); // confirmarTurno() - AGREGADO
    }
}
