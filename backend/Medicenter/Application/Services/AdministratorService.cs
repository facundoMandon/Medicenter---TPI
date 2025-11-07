using Application.Interfaces;
using Application.Models;
using Application.Models.Request;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

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
            // 🔹 Validaciones de datos de entrada
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

            if (dto.Rol != Domain.Enums.Roles.Administrator)
                throw new ValidationException("El rol debe ser Administrador para crear un administrador.");

            // 🔹 Validar duplicado por email
            var existingEmail = await _usersRepository.GetByEmailAsync(dto.Email);
            if (existingEmail != null)
                throw new DuplicateException($"Ya existe un usuario registrado con el email '{dto.Email}'.");

            // 🔹 Validar duplicado por DNI (si se proporciona)
            if (dto.DNI > 0)
            {
                var existingDni = await _usersRepository.GetByDniAsync(dto.DNI);
                if (existingDni != null)
                    throw new DuplicateException($"Ya existe otro usuario con el DNI '{dto.DNI}'.");
            }


            // 🔹 Crear el administrador
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
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ValidationException("El nombre es requerido.");

            if (string.IsNullOrWhiteSpace(dto.LastName))
                throw new ValidationException("El apellido es requerido.");

            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ValidationException("El email es requerido.");

            if (!IsValidEmail(dto.Email))
                throw new ValidationException("El formato del email no es válido.");

            var user = await _usersRepository.GetByIdAsync(userId);
            if (user == null)
                throw new NotFoundException($"Usuario con ID {userId} no encontrado.");

            // 🔹 Validar duplicado de email (otro usuario con el mismo email)
            var existingEmail = await _usersRepository.GetByEmailAsync(dto.Email);
            if (existingEmail != null && existingEmail.Id != userId)
                throw new DuplicateException($"Ya existe otro usuario con el email '{dto.Email}'.");

            // 🔹 Validar duplicado de DNI (otro usuario con el mismo DNI)
            if (dto.DNI > 0)
            {
                var existingDni = await _usersRepository.GetByDniAsync(dto.DNI);
                if (existingDni != null && existingDni.Id != userId)
                    throw new DuplicateException($"Ya existe otro usuario con el DNI '{dto.DNI}'.");
            }

            user.Name = dto.Name;
            user.LastName = dto.LastName;
            user.DNI = dto.DNI;
            user.Email = dto.Email;

            if (!string.IsNullOrEmpty(dto.Password))
                user.Password = dto.Password; // (debería hashearse)

            await _usersRepository.UpdateAsync(user);
        }

        public async Task DeleteUserAsync(int userId)
        {
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
                Type = s.Type,
                Description = s.Description
            });
        }

        public async Task DeleteSpecialtyAsync(int specialtyId)
        {
            var specialty = await _specialtiesRepository.GetByIdAsync(specialtyId);
            if (specialty == null)
                throw new NotFoundException($"Especialidad con ID {specialtyId} no encontrada.");

            await _specialtiesRepository.DeleteAsync(specialty);
        }

        public async Task DeleteAppointmentAsync(int appointmentId)
        {
            var appointment = await _appointmentsRepository.GetByIdAsync(appointmentId);
            if (appointment == null)
                throw new NotFoundException($"Turno con ID {appointmentId} no encontrado.");

            await _appointmentsRepository.DeleteAsync(appointment);
        }

        public async Task DeleteInsuranceAsync(int insuranceId)
        {
            var insurance = await _insuranceRepository.GetByIdAsync(insuranceId);
            if (insurance == null)
                throw new NotFoundException($"Obra Social con ID {insuranceId} no encontrada.");

            await _insuranceRepository.DeleteAsync(insurance);
        }

        // 🔹 Método auxiliar para validar formato de email
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
