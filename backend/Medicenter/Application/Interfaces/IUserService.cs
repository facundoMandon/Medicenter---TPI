using Application.Models;
using Application.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    // Reemplaza 'UserDTO' y 'CreationUserDTO' con tus nombres reales de DTOs
    public interface IUserService
    {
        Task<UserDTO?> GetByIdAsync(int id);
        Task<IEnumerable<UserDTO>> GetAllAsync();
        Task<UserDTO?> UpdateAsync(int id, CreationUserDTO dto);
        Task DeleteAsync(int id);

        // Métodos de cuenta (del diagrama)
        Task DeleteAccountAsync(int userId); // eliminarCuenta()
    }
}