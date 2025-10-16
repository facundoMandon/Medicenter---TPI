using Application.Models;
using Application.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    // Reemplaza 'UserDTO' y 'CreationUsersDTO' con tus nombres reales de DTOs
    public interface IUsersService
    {
        Task<UsersDTO?> GetByIdAsync(int id);
        Task<IEnumerable<UsersDTO>> GetAllAsync();
        Task<UsersDTO?> UpdateAsync(int id, CreationUsersDTO dto);
        Task DeleteAsync(int id);

        // Métodos de cuenta (del diagrama)
        Task RecoverPasswordAsync(string email); // recuperarContraseña()
        Task DeleteAccountAsync(int userId); // eliminarCuenta()
    }
}