using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Application.Models;

namespace Application.Services
{
    // Reemplaza 'AdministratorDTO' y 'CreationAdministratorDTO' con tus nombres reales de DTOs
    public interface IAdministratorsService
    {
        Task<AdministratorDTO> CreateAsync(CreationAdministratorDTO dto);
        Task<AdministratorDTO> GetByIdAsync(int id);
        Task<IEnumerable<AdministratorDTO>> GetAllAsync();
        Task<AdministratorDTO> UpdateAsync(int id, CreationAdministratorDTO dto);
        Task DeleteAsync(int id);
    }
}