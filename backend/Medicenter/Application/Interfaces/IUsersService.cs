using Application.Models;
using Application.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    // Reemplaza 'UserDTO' y 'CreationUsersDTO' con tus nombres reales de DTOs
    public interface IUsersService
    {
        Task<UserDTO> CreateAsync(CreationUsersDTO dto);
        Task<UserDTO> GetByIdAsync(int id);
        Task<IEnumerable<UserDTO>> GetAllAsync();
        Task<UserDTO> UpdateAsync(int id, CreationUsersDTO dto);
        Task DeleteAsync(int id);
    }
}