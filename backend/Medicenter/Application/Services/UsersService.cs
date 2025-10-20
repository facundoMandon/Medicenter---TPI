﻿using Application.Models;
using Application.Models.Request;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;

        public UsersService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        public async Task<UsersDTO?> GetByIdAsync(int id)
        {
            var result = await _usersRepository.GetByIdAsync(id);
            return result != null ? UsersDTO.FromEntity(result) : null;
        }

        public async Task<IEnumerable<UsersDTO>> GetAllAsync()
        {
            var results = await _usersRepository.GetAllAsync();
            return results.Select(UsersDTO.FromEntity);
        }

        public async Task<UsersDTO?> UpdateAsync(int id, CreationUsersDTO dto)
        {
            var user = await _usersRepository.GetByIdAsync(id);
            if (user == null) return null;

            user.Name = dto.Name;
            user.LastName = dto.LastName;
            user.DNI = dto.DNI;
            user.Email = dto.Email;
            if (!string.IsNullOrEmpty(dto.Password))
                user.Password = dto.Password; // Debería hashearse

            await _usersRepository.UpdateAsync(user);
            return UsersDTO.FromEntity(user);
        }

        public async Task DeleteAccountAsync(int userId)
        {
            var user = await _usersRepository.GetByIdAsync(userId);
            if (user != null)
                await _usersRepository.DeleteAsync(user);
        }

        public async Task DeleteAsync(int id) => await DeleteAccountAsync(id);

        public Task RecoverPasswordAsync(string email)
        {
            // TODO: Implementar lógica de recuperación
            throw new System.NotImplementedException("Password recovery logic not implemented.");
        }
    }
}
