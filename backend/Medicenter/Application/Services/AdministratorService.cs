using Application.Interfaces;
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

namespace Application.Services
{
    public class AdministratorService : IAdministratorService
    {
        private readonly IAdministratorRepository _adminsRepository;
        private readonly IUserRepository _usersRepository;
        private readonly IProfessionalRepository _professionalsRepository;
        private readonly ISpecialtiesRepository _specialtiesRepository;
        private readonly IAppointmentRepository _appointmentsRepository;
        private readonly IInsuranceRepository _insuranceRepository;

        public AdministratorService(
            IAdministratorRepository adminsRepository,
            IUserRepository usersRepository,
            IProfessionalRepository professionalsRepository,
            ISpecialtiesRepository specialtiesRepository,
            IAppointmentRepository appointmentsRepository,
            IInsuranceRepository insuranceRepository)
        {
            _adminsRepository = adminsRepository;
            _usersRepository = usersRepository;
            _professionalsRepository = professionalsRepository;
            _specialtiesRepository = specialtiesRepository;
            _appointmentsRepository = appointmentsRepository;
            _insuranceRepository = insuranceRepository;
        }

        public async Task<AdministratorDTO> CreateAdministratorAsync(CreationUserDTO dto)
        {
            // Validaciones de datos de entrada
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ValidationException("El nombre es requerido.");

            if (string.IsNullOrWhiteSpace(dto.LastName))
                throw new ValidationException("El apellido es requerido.");

            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ValidationException("El email es requerido.");

            if (!IsValidEmail(dto.Email))
                throw new ValidationException("El formato del email no es válido.");

            if (string.IsNullOrWhiteSpace(dto.Password))
                throw new ValidationException("La contraseña es requerida.");

            // Validar que el rol sea Administrador
            if (dto.Rol != Domain.Enums.Roles.Administrator)
                throw new ValidationException("El rol debe ser Administrador para crear un administrador.");

            // Crear el administrador
            var admin = new Administrator
            {
                Name = dto.Name,
                LastName = dto.LastName,
                DNI = dto.DNI,
                Email = dto.Email,
                Password = dto.Password, // Debería hashearse
                Rol = dto.Rol
            };

            var created = await _adminsRepository.CreateAsync(admin);
            return AdministratorDTO.FromEntity(created);
        }

        public async Task UpdateUserAsync(int userId, CreationUserDTO dto)
        {
            // Validaciones de datos de entrada
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ValidationException("El nombre es requerido.");

            if (string.IsNullOrWhiteSpace(dto.LastName))
                throw new ValidationException("El apellido es requerido.");

            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ValidationException("El email es requerido.");

            if (!IsValidEmail(dto.Email))
                throw new ValidationException("El formato del email no es válido.");

            // Verificar que el usuario existe
            var user = await _usersRepository.GetByIdAsync(userId);
            if (user == null)
                throw new NotFoundException($"Usuario con ID {userId} no encontrado.");

            // Actualizar propiedades del usuario
            user.Name = dto.Name;
            user.LastName = dto.LastName;
            user.DNI = dto.DNI;
            user.Email = dto.Email;

            // Actualizar contraseña si se proporciona
            if (!string.IsNullOrEmpty(dto.Password))
                user.Password = dto.Password; // Debería hashearse

            await _usersRepository.UpdateAsync(user);
        }

        public async Task DeleteUserAsync(int userId)
        {
            // Verificar que el usuario existe antes de eliminar
            var user = await _usersRepository.GetByIdAsync(userId);
            if (user == null)
                throw new NotFoundException($"Usuario con ID {userId} no encontrado.");

            await _usersRepository.DeleteAsync(user);
        }

        public async Task<IEnumerable<ProfessionalDTO>> ViewProfessionalAsync()
        {
            var professionals = await _professionalsRepository.GetAllAsync();
            return professionals.Select(p => ProfessionalDTO.FromEntity(p));
        }

        public async Task<IEnumerable<SpecialtiesDTO>> ViewSpecialtiesAsync()
        {
            var specialties = await _specialtiesRepository.GetAllAsync();

            return specialties.Select(s => new SpecialtiesDTO
            {
                Id = s.Id,
                Type = s.Type, // CORREGIDO: Type en vez de Name
                Description = s.Description // CORREGIDO
            });
        }

        public async Task DeleteSpecialtyAsync(int specialtyId)
        {
            // Verificar que la especialidad existe antes de eliminar
            var specialty = await _specialtiesRepository.GetByIdAsync(specialtyId);
            if (specialty == null)
                throw new NotFoundException($"Especialidad con ID {specialtyId} no encontrada.");

            await _specialtiesRepository.DeleteAsync(specialty);
        }

        public async Task DeleteAppointmentAsync(int appointmentId)
        {
            // Verificar que el turno existe antes de eliminar
            var appointment = await _appointmentsRepository.GetByIdAsync(appointmentId);
            if (appointment == null)
                throw new NotFoundException($"Turno con ID {appointmentId} no encontrado.");

            await _appointmentsRepository.DeleteAsync(appointment);
        }

        public async Task DeleteInsuranceAsync(int insuranceId)
        {
            // Verificar que la obra social existe antes de eliminar
            var insurance = await _insuranceRepository.GetByIdAsync(insuranceId);
            if (insurance == null)
                throw new NotFoundException($"Obra Social con ID {insuranceId} no encontrada.");

            await _insuranceRepository.DeleteAsync(insurance);
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