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
    public class ProfessionalsService : IProfessionalsService
    {
        private readonly IProfessionalsRepository _professionalsRepository;
        // Repositorios auxiliares inyectados (ya no están comentados)
        private readonly IAppointmentsRepository _appointmentsRepository;
        private readonly IPatientsRepository _patientsRepository;
        private readonly ISpecialtiesRepository _specialtiesRepository;

        public ProfessionalsService(
            IProfessionalsRepository professionalsRepository,
            IAppointmentsRepository appointmentsRepository,
            IPatientsRepository patientsRepository,
            ISpecialtiesRepository specialtiesRepository
        )
        {
            _professionalsRepository = professionalsRepository;
            _appointmentsRepository = appointmentsRepository;
            _patientsRepository = patientsRepository;
            _specialtiesRepository = specialtiesRepository;
        }

        // 1. Creación de Profesional
        public async Task<ProfessionalsDTO> CreateProfessionalAsync(CreationProfessionalsDTO dto)
        {
            var professional = new Professionals
            {
                Name = dto.Name,
                LastName = dto.LastName,
                DNI = dto.DNI,
                Email = dto.Email,
                Password = dto.Password,
                Rol = dto.rol,
                LicenseNumber = dto.LicenseNumber,
                SpecialtyId = dto.SpecialtyId
            };

            var created = await _professionalsRepository.CreateAsync(professional);

            // Simulación de búsqueda de nombre de especialidad para el DTO de salida
            string specialtyName = "Specialty Name (lookup simulated)";

            var professionalDto = new ProfessionalsDTO
            {
                Id = created.Id,
                Name = created.Name,
                LastName = created.LastName,
                DNI = created.DNI,
                Email = created.Email,
                Rol = created.Rol,
                LicenseNumber = created.LicenseNumber,
                SpecialtyName = specialtyName
            };
            return professionalDto;
        }

        // 2. verTurnos (ViewAppointmentsAsync)
        public Task<IEnumerable<AppointmentsDTO>> ViewAppointmentsAsync(int professionalId)
        {
            // Simulación de uso de repositorio: var appointments = _appointmentsRepository.GetByProfessionalId(professionalId);
            var appointments = new List<AppointmentsDTO>
            {
                new AppointmentsDTO { Id = 101 },
                new AppointmentsDTO { Id = 102 }
            };
            return Task.FromResult<IEnumerable<AppointmentsDTO>>(appointments);
        }

        // 3. aceptarTurno (AcceptAppointmentAsync)
        public Task<bool> AcceptAppointmentAsync(int professionalId, int appointmentId)
        {
            // Simulación de uso de repositorio: var appointment = await _appointmentsRepository.GetByIdAsync(appointmentId);
            // Simulación de éxito
            return Task.FromResult(true);
        }

        // 4. rechazarTurno (RejectAppointmentAsync)
        public Task<bool> RejectAppointmentAsync(int professionalId, int appointmentId)
        {
            // Simulación de uso de repositorio: var appointment = await _appointmentsRepository.GetByIdAsync(appointmentId);
            // Simulación de éxito
            return Task.FromResult(true);
        }

        // 5. listarPacientes (ListPatientsAsync)
        public Task<IEnumerable<PatientsDTO>> ListPatientsAsync(int professionalId)
        {
            // Simulación de uso de repositorios: var patients = _patientsRepository.GetPatientsByProfessional(professionalId);
            var patients = new List<PatientsDTO>
            {
                new PatientsDTO { Id = 1, Name = "Juan", LastName = "Perez", AffiliateNumber = 12345 },
            };
            return Task.FromResult<IEnumerable<PatientsDTO>>(patients);
        }
    }
}
