using Application.Models;
using Application.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    // Reemplaza 'ProfessionalDTO' y 'CreationProfessionalsDTO' con tus nombres reales de DTOs
    public interface IProfessionalsService
    {
        // Creación del rol
        Task<ProfessionalsDTO> CreateProfessionalAsync(CreationProfessionalsDTO dto);

        // Métodos de negocio (del diagrama)
        Task<IEnumerable<AppointmentsDTO>> ViewAppointmentsAsync(int professionalId);
        Task<bool> AcceptAppointmentAsync(int professionalId, int appointmentId);
        Task<bool> RejectAppointmentAsync(int professionalId, int appointmentId);
        Task<IEnumerable<PatientsDTO>> ListPatientsAsync(int professionalId);
    }
}