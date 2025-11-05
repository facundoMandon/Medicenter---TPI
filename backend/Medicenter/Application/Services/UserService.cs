using Application.Models;
using Application.Models.Request;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _usersRepository;

        public UserService(IUserRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<UserDTO?> GetByIdAsync(int id)
        {
            var result = await _usersRepository.GetByIdAsync(id);
            if (result == null)
                throw new NotFoundException($"Usuario con ID {id} no encontrado.");

            return UserDTO.FromEntity(result);
        }

        public async Task<IEnumerable<UserDTO>> GetAllAsync()
        {
            var results = await _usersRepository.GetAllAsync();
            return results.Select(UserDTO.FromEntity);
        }

        public async Task<UserDTO?> UpdateAsync(int id, CreationUserDTO dto)
        {
            // Validaciones de datos de entrada
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ValidationException("El nombre es requerido.");

            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ValidationException("El email es requerido.");

            if (!IsValidEmail(dto.Email))
                throw new ValidationException("El formato del email no es válido.");

            // Buscar usuario existente
            var user = await _usersRepository.GetByIdAsync(id);
            if (user == null)
                throw new NotFoundException($"Usuario con ID {id} no encontrado.");

            // Actualizar propiedades del usuario
            user.Name = dto.Name;
            user.LastName = dto.LastName;
            user.DNI = dto.DNI;
            user.Email = dto.Email;

            // Actualizar contraseña si se proporciona
            if (!string.IsNullOrEmpty(dto.Password))
                user.Password = dto.Password; // Debería hashearse

            await _usersRepository.UpdateAsync(user);
            return UserDTO.FromEntity(user);
        }

        public async Task DeleteAccountAsync(int userId)
        {
            // Verificar que el usuario existe antes de eliminar
            var user = await _usersRepository.GetByIdAsync(userId);
            if (user == null)
                throw new NotFoundException($"Usuario con ID {userId} no encontrado.");

            await _usersRepository.DeleteAsync(user);
        }

        public async Task DeleteAsync(int id) => await DeleteAccountAsync(id);

        public Task RecoverPasswordAsync(string email)
        {
            // Validar que el email no esté vacío
            if (string.IsNullOrWhiteSpace(email))
                throw new ValidationException("El email es requerido para recuperar la contraseña.");

            // TODO: Implementar lógica de recuperación
            throw new NotImplementedException("Lógica de recuperación de contraseña no implementada.");
        }

        // Método auxiliar para validar formato de email
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}