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
    public class HospitalsService : IHospitalsService
    {
        private readonly IHospitalsRepository _hospitalsRepository;
        private readonly IProfessionalsRepository _professionalsRepository;

        // Asumo que tienes un mapeo simple entre entidades y DTOs
        private HospitalsDTO MapToDto(Hospitals hospital)
        {
            return new HospitalsDTO
            {
                Id = hospital.Id,
                Name = hospital.Name,
                Address = hospital.Address
            };
        }

        private ProfessionalsDTO MapToProfessionalDto(Professionals professional)
        {
            // Mapeo básico necesario para ListProfessionalsAsync
            return new ProfessionalsDTO
            {
                Id = professional.Id,
                Name = professional.Name,
                LastName = professional.LastName,
                LicenseNumber = professional.LicenseNumber
                // Añadir SpecialtyName si se puede cargar desde el repositorio
            };
        }

        public HospitalsService(IHospitalsRepository hospitalsRepository, IProfessionalsRepository professionalsRepository)
        {
            _hospitalsRepository = hospitalsRepository;
            _professionalsRepository = professionalsRepository;
        }

        // --- CRUD Básico ---

        public async Task<HospitalsDTO> GetByIdAsync(int id)
        {
            var hospital = await _hospitalsRepository.GetByIdAsync(id);
            return hospital != null ? MapToDto(hospital) : throw new KeyNotFoundException($"Hospital con ID {id} no encontrado.");
        }

        public async Task<IEnumerable<HospitalsDTO>> GetAllAsync()
        {
            var hospitals = await _hospitalsRepository.GetAllAsync();
            return hospitals.Select(MapToDto);
        }

        public async Task<HospitalsDTO> CreateHospitalAsync(CreationHospitalsDTO dto)
        {
            var hospital = new Hospitals { Name = dto.Name, Address = dto.Address };
            var created = await _hospitalsRepository.CreateAsync(hospital);
            return MapToDto(created);
        }

        public async Task<HospitalsDTO> UpdateHospitalAsync(int id, CreationHospitalsDTO dto)
        {
            var hospital = await _hospitalsRepository.GetByIdAsync(id);
            if (hospital == null) throw new KeyNotFoundException($"Hospital con ID {id} no encontrado.");

            hospital.Name = dto.Name;
            hospital.Address = dto.Address;

            await _hospitalsRepository.UpdateAsync(hospital);
            return MapToDto(hospital);
        }

        public async Task DeleteHospitalAsync(int id)
        {
            var hospital = await _hospitalsRepository.GetByIdAsync(id);
            if (hospital == null) throw new KeyNotFoundException($"Hospital con ID {id} no encontrado.");
            await _hospitalsRepository.DeleteAsync(hospital);
        }

        // --- MÉTODOS DE GESTIÓN DE PROFESIONALES ---

        // registrarProfesional
        public async Task RegisterProfessionalAsync(int hospitalId, int professionalId)
        {
            // Lógica de validación: asegurar que ambos IDs existen
            var hospital = await _hospitalsRepository.GetByIdAsync(hospitalId);
            var professional = await _professionalsRepository.GetByIdAsync(professionalId);

            if (hospital == null) throw new KeyNotFoundException($"Hospital con ID {hospitalId} no encontrado.");
            if (professional == null) throw new KeyNotFoundException($"Profesional con ID {professionalId} no encontrado.");

            await _hospitalsRepository.RegisterProfessionalAsync(hospitalId, professionalId);
        }

        // editarProfesional (El servicio se encarga de la lógica de negocio)
        // Nota: La edición de los datos del profesional (nombre, matrícula, etc.) la realiza el UsersService o ProfessionalsService.
        // Este método en IHospitalsService es redundante, pero se implementa para satisfacer el contrato.
        public Task EditProfessionalAsync(int hospitalId, int professionalId)
        {
            // En un sistema real, aquí se llamaría a IProfessionalsService.UpdateProfessionalAsync(dto)
            // Asumiendo que esta operación es sólo una bandera o un trigger. Lo dejaremos vacío por ahora.
            return Task.CompletedTask;
        }

        // listarProfesionales
        public async Task<IEnumerable<ProfessionalsDTO>> ListProfessionalsAsync(int hospitalId)
        {
            var professionals = await _hospitalsRepository.GetProfessionalsByHospitalIdAsync(hospitalId);
            return professionals.Select(MapToProfessionalDto);
        }

        // eliminarProfesionales
        public async Task RemoveProfessionalAsync(int hospitalId, int professionalId)
        {
            await _hospitalsRepository.RemoveProfessionalAsync(hospitalId, professionalId);
        }
    }
}
