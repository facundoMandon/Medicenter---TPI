using Application.Models;
using Application.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    // Reemplaza 'ProfessionalDTO' y 'CreationProfessionalDTO' con tus nombres reales de DTOs
    public interface IProfessionalService
    {
        // Creación del rol
        Task<ProfessionalDTO> CreateProfessionalAsync(CreationProfessionalDTO dto);

        // Métodos de negocio (del diagrama)
        Task<IEnumerable<AppointmentDTO>> ViewAppointmentAsync(int professionalId);
        Task<bool> AcceptAppointmentAsync(int professionalId, int appointmentId);
        Task<bool> RejectAppointmentAsync(int professionalId, int appointmentId);
        Task<IEnumerable<PatientDTO>> ListPatientAsync(int professionalId);
    }
}