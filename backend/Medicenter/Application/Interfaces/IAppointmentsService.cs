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
        // CRUD Base para el Administrador
        Task<AppointmentsDTO> GetByIdAsync(int id);
        Task<IEnumerable<AppointmentsDTO>> GetAllAsync();

        // Métodos de negocio (del diagrama y gestión)
        Task<AppointmentsDTO> AssignAppointmentAsync(CreationAppointmentDTO dto); // asignarTurno
        Task<AppointmentsDTO> UpdateAppointmentAsync(int appointmentId, CreationAppointmentDTO dto); // modificarTurno
        Task CancelAppointmentAsync(int appointmentId); // cancelarTurno
    }
}
