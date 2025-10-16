using Application.Interfaces;
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

        public AdministratorsService(
            IAdministratorsRepository adminsRepository,
            IUsersRepository usersRepository,
            IProfessionalsRepository professionalsRepository
        )
        {
            _adminsRepository = adminsRepository;
            _usersRepository = usersRepository;
            _professionalsRepository = professionalsRepository;
        }

        // 1. Creación de Administrador (crearUsuario)
        public async Task<AdministratorsDTO> CreateAdministratorAsync(CreationUsersDTO dto)
        {
            var admin = new Administrators
            {
                Name = dto.Name,
                LastName = dto.LastName,
                DNI = dto.DNI,
                Email = dto.Email,
                Password = dto.Password,
                Rol = dto.rol
            };

            var created = await _adminsRepository.CreateAsync(admin);
            return (AdministratorsDTO)UsersDTO.FromEntity(created);
        }

        // 2. ModificarUsuario
        public async Task UpdateUserAsync(int userId, CreationUsersDTO dto)
        {
            var user = await _usersRepository.GetByIdAsync(userId);
            if (user == null) throw new KeyNotFoundException($"User with ID {userId} not found.");

            // Lógica para actualizar campos comunes
            user.Name = dto.Name;
            user.LastName = dto.LastName;
            user.DNI = dto.DNI;
            user.Email = dto.Email;

            await _usersRepository.UpdateAsync(user);
        }

        // 3. EliminarUsuario
        public async Task DeleteUserAsync(int userId)
        {
            var user = await _usersRepository.GetByIdAsync(userId);
            if (user != null)
            {
                await _usersRepository.DeleteAsync(user);
            }
        }

        // 4. verProfesionales
        public async Task<IEnumerable<ProfessionalsDTO>> ViewProfessionalsAsync()
        {
            var professionals = await _professionalsRepository.GetAllAsync();

            return professionals.Select(p => new ProfessionalsDTO
            {
                Id = p.Id,
                Name = p.Name,
                LastName = p.LastName,
                DNI = p.DNI,
                Email = p.Email,
                Rol = p.Rol,
                LicenseNumber = p.LicenseNumber,
            });
        }

        // 5. EliminarEspecialidad
        public Task DeleteSpecialtyAsync(int specialtyId)
        {
            // Requiere ISpecialtiesRepository
            throw new NotImplementedException("Delete Specialty logic requires ISpecialtiesRepository.");
        }

        // 6. EliminarTurnos
        public Task DeleteAppointmentAsync(int appointmentId)
        {
            // Requiere IAppointmentsRepository
            throw new NotImplementedException("Delete Appointment logic requires IAppointmentsRepository.");
        }

        // 7. EliminarObraSocial
        public Task DeleteInsuranceAsync(int insuranceId)
        {
            // Requiere IInsuranceRepository
            throw new NotImplementedException("Delete Insurance logic requires IInsuranceRepository.");
        }

        // 8. verEspecialidades
        public Task<IEnumerable<SpecialtiesDTO>> ViewSpecialtiesAsync()
        {
            // Requiere ISpecialtiesRepository
            throw new NotImplementedException("View Specialties logic requires ISpecialtiesRepository.");
        }
    }
}
