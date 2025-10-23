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
    public interface IAdministratorsService
    {
        Task<AdministratorsDTO> CreateAdministratorAsync(CreationUsersDTO dto);

        // Métodos de Administración (Mapeo del diagrama: EliminarUsuario, ModificarUsuario, etc.)
        Task DeleteUserAsync(int userId);
        Task UpdateUserAsync(int userId, CreationUsersDTO dto);
        Task<IEnumerable<ProfessionalsDTO>> ViewProfessionalsAsync();
        Task<IEnumerable<SpecialtiesDTO>> ViewSpecialtiesAsync();

        // Administración de Entidades Secundarias
        Task DeleteSpecialtyAsync(int specialtyId);
        Task DeleteAppointmentAsync(int appointmentId);
        Task DeleteInsuranceAsync(int insuranceId);
    }
}