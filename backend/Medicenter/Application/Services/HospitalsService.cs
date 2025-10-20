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
        private readonly ISpecialtiesRepository _specialtiesRepository;

        public HospitalsService(
            IHospitalsRepository hospitalsRepository,
            IProfessionalsRepository professionalsRepository,
            ISpecialtiesRepository specialtiesRepository)
        {
            _hospitalsRepository = hospitalsRepository;
            _professionalsRepository = professionalsRepository;
            _specialtiesRepository = specialtiesRepository;
        }

        // CRUD
        public async Task<HospitalsDTO> GetByIdAsync(int id)
        {
            var hospital = await _hospitalsRepository.GetByIdAsync(id);
            if (hospital == null)
                throw new KeyNotFoundException($"Hospital con ID {id} no encontrado.");

            return HospitalsDTO.FromEntity(hospital);
        }

        public async Task<IEnumerable<HospitalsDTO>> GetAllAsync()
        {
            var hospitals = await _hospitalsRepository.GetAllAsync();
            return hospitals.Select(HospitalsDTO.FromEntity);
        }

        public async Task<HospitalsDTO> CreateHospitalAsync(CreationHospitalsDTO dto)
        {
            var hospital = new Hospitals
            {
                Nombre = dto.Nombre,
                Direccion = dto.Direccion
            };

            var created = await _hospitalsRepository.CreateAsync(hospital);
            return HospitalsDTO.FromEntity(created);
        }

        public async Task<HospitalsDTO> UpdateHospitalAsync(int id, CreationHospitalsDTO dto)
        {
            var hospital = await _hospitalsRepository.GetByIdAsync(id);
            if (hospital == null)
                throw new KeyNotFoundException($"Hospital con ID {id} no encontrado.");

            hospital.Nombre = dto.Nombre;
            hospital.Direccion = dto.Direccion;

            await _hospitalsRepository.UpdateAsync(hospital);
            return HospitalsDTO.FromEntity(hospital);
        }

        public async Task DeleteHospitalAsync(int id)
        {
            var hospital = await _hospitalsRepository.GetByIdAsync(id);
            if (hospital == null)
                throw new KeyNotFoundException($"Hospital con ID {id} no encontrado.");

            await _hospitalsRepository.DeleteAsync(hospital);
        }

        // registrarProfesional
        public async Task RegisterProfessionalAsync(int hospitalId, int professionalId)
        {
            var hospital = await _hospitalsRepository.GetByIdAsync(hospitalId);
            var professional = await _professionalsRepository.GetByIdAsync(professionalId);

            if (hospital == null)
                throw new KeyNotFoundException($"Hospital con ID {hospitalId} no encontrado.");
            if (professional == null)
                throw new KeyNotFoundException($"Profesional con ID {professionalId} no encontrado.");

            await _hospitalsRepository.RegisterProfessionalAsync(hospitalId, professionalId);
        }

        // editarProfesional (Este método no hace nada porque la edición se hace en UsersService)
        public Task EditProfessionalAsync(int hospitalId, int professionalId)
        {
            // Este método existe solo para cumplir con el contrato del diagrama
            // La edición real del profesional se hace en UsersService o ProfessionalsService
            return Task.CompletedTask;
        }

        // listarProfesionales
        public async Task<IEnumerable<ProfessionalsDTO>> ListProfessionalsAsync(int hospitalId)
        {
            var professionals = await _hospitalsRepository.GetProfessionalsByHospitalIdAsync(hospitalId);

            var dtos = new List<ProfessionalsDTO>();

            foreach (var professional in professionals)
            {
                var specialty = await _specialtiesRepository.GetByIdAsync(professional.SpecialtyId);
                dtos.Add(ProfessionalsDTO.FromEntity(professional, specialty?.Tipo ?? ""));
            }

            return dtos;
        }

        // eliminarProfesionales
        public async Task RemoveProfessionalAsync(int hospitalId, int professionalId)
        {
            await _hospitalsRepository.RemoveProfessionalAsync(hospitalId, professionalId);
        }
    }
}
