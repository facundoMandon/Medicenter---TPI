using Application.Models;
using Application.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    // Reemplaza 'AppointmentDTO' y 'CreationAppointmentDTO' con tus nombres reales de DTOs
    public interface IAppointmentService
    {
        // CRUD Base
        Task<AppointmentDTO> GetByIdAsync(int id);
        Task<IEnumerable<AppointmentDTO>> GetAllAsync();

        // Métodos de negocio (según diagrama)
        Task<AppointmentDTO> AssignAppointmentAsync(CreationAppointmentDTO dto); // asignarTurno()
        Task<AppointmentDTO> UpdateAppointmentAsync(int appointmentId, CreationAppointmentDTO dto); // modificarTurno()
        Task CancelAppointmentAsync(int appointmentId); // cancelarTurno()

        //Añadir el DeleteAppointmentAsync
        Task<AppointmentDTO> ConfirmAppointmentAsync(int appointmentId); // confirmarTurno() - AGREGADO
    }
}
