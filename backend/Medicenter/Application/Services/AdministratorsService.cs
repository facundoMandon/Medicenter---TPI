﻿using Application.Interfaces;
using Application.Models;
using Application.Models.Request;
using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class AdministratorsService : IAdministratorsService
    {
        private readonly IAdministratorsRepository _adminsRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IProfessionalsRepository _professionalsRepository;
        private readonly ISpecialtiesRepository _specialtiesRepository;
        private readonly IAppointmentsRepository _appointmentsRepository;
        private readonly IInsuranceRepository _insuranceRepository;

        public AdministratorsService(
            IAdministratorsRepository adminsRepository,
            IUsersRepository usersRepository,
            IProfessionalsRepository professionalsRepository,
            ISpecialtiesRepository specialtiesRepository,
            IAppointmentsRepository appointmentsRepository,
            IInsuranceRepository insuranceRepository)
        {
            _adminsRepository = adminsRepository;
            _usersRepository = usersRepository;
            _professionalsRepository = professionalsRepository;
            _specialtiesRepository = specialtiesRepository;
            _appointmentsRepository = appointmentsRepository;
            _insuranceRepository = insuranceRepository;
        }

        public async Task<AdministratorsDTO> CreateAdministratorAsync(CreationUsersDTO dto)
        {
            var admin = new Administrators
            {
                Name = dto.Name,
                LastName = dto.LastName,
                DNI = dto.DNI,
                Email = dto.Email,
                Password = dto.Password,
                Rol = dto.Rol
            };

            var created = await _adminsRepository.CreateAsync(admin);
            return AdministratorsDTO.FromEntity(created);
        }

        public async Task UpdateUserAsync(int userId, CreationUsersDTO dto)
        {
            var user = await _usersRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {userId} not found.");

            user.Name = dto.Name;
            user.LastName = dto.LastName;
            user.DNI = dto.DNI;
            user.Email = dto.Email;
            if (!string.IsNullOrEmpty(dto.Password))
                user.Password = dto.Password;

            await _usersRepository.UpdateAsync(user);
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _usersRepository.GetByIdAsync(userId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {userId} not found.");

            await _usersRepository.DeleteAsync(user);
        }

        public async Task<IEnumerable<ProfessionalsDTO>> ViewProfessionalsAsync()
        {
            var professionals = await _professionalsRepository.GetAllAsync();

            return professionals.Select(p => ProfessionalsDTO.FromEntity(p));
        }

        public async Task<IEnumerable<SpecialtiesDTO>> ViewSpecialtiesAsync()
        {
            var specialties = await _specialtiesRepository.GetAllAsync();

            return specialties.Select(s => new SpecialtiesDTO
            {
                Id = s.Id,
                Tipo = s.Tipo, // CORREGIDO: Tipo en vez de Name
                Descripcion = s.Descripcion // CORREGIDO
            });
        }

        public async Task DeleteSpecialtyAsync(int specialtyId)
        {
            var specialty = await _specialtiesRepository.GetByIdAsync(specialtyId);
            if (specialty == null)
                throw new KeyNotFoundException($"Specialty with ID {specialtyId} not found.");

            await _specialtiesRepository.DeleteAsync(specialty);
        }

        public async Task DeleteAppointmentAsync(int appointmentId)
        {
            var appointment = await _appointmentsRepository.GetByIdAsync(appointmentId);
            if (appointment == null)
                throw new KeyNotFoundException($"Appointment with ID {appointmentId} not found.");

            await _appointmentsRepository.DeleteAsync(appointment);
        }

        public async Task DeleteInsuranceAsync(int insuranceId)
        {
            var insurance = await _insuranceRepository.GetByIdAsync(insuranceId);
            if (insurance == null)
                throw new KeyNotFoundException($"Insurance with ID {insuranceId} not found.");

            await _insuranceRepository.DeleteAsync(insurance);
        }
    }
}
