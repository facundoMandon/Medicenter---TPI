using Application.Models;
using Application.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    // Reemplaza 'PatientDTO' y 'CreationPatientDTO' con tus nombres reales de DTOs
    public interface IPatientService
    {
        // Creación del rol
        Task<PatientDTO> CreatePatientAsync(CreationPatientDTO dto);

        // Métodos de negocio (del diagrama)
        Task<IEnumerable<AppointmentDTO>> ViewAppointmentAsync(int patientId); // verTurnos
        Task CancelAppointmentAsync(int patientId, int appointmentId); // cancelarTurno
        Task<AppointmentDTO> RequestAppointmentAsync(int patientId, AppointmentRequestDTO request); // pedirTurno
    }
}