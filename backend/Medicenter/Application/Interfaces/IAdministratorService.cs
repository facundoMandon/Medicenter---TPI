using Application.Models;
using Application.Models.Request;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    // Reemplaza 'AdministratorDTO' y 'CreationAdministratorDTO' con tus nombres reales de DTOs
    public interface IAdministratorService
    {
        Task<AdministratorDTO> CreateAdministratorAsync(CreationUserDTO dto);

        // Métodos de Administración (Mapeo del diagrama: EliminarUsuario, ModificarUsuario, etc.)
        Task DeleteUserAsync(int userId);
        Task UpdateUserAsync(int userId, CreationUserDTO dto);
        Task<IEnumerable<ProfessionalDTO>> ViewProfessionalAsync();
        Task<IEnumerable<SpecialtiesDTO>> ViewSpecialtiesAsync();

        // Administración de Entidades Secundarias
        Task DeleteSpecialtyAsync(int specialtyId);
        Task DeleteAppointmentAsync(int appointmentId);
        Task DeleteInsuranceAsync(int insuranceId);
    }
}