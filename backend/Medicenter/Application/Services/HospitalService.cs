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
using System.Threading.Tasks;

namespace Application.Services
{
    public class HospitalService : IHospitalService
    {
        private readonly IHospitalRepository _hospitalsRepository;
        private readonly IProfessionalRepository _professionalsRepository;
        private readonly ISpecialtiesRepository _specialtiesRepository;

        public HospitalService(
            IHospitalRepository hospitalsRepository,
            IProfessionalRepository professionalsRepository,
            ISpecialtiesRepository specialtiesRepository)
        {
            _hospitalsRepository = hospitalsRepository;
            _professionalsRepository = professionalsRepository;
            _specialtiesRepository = specialtiesRepository;
        }

        // CRUD
        public async Task<HospitalDTO> GetByIdAsync(int id)
        {
            var hospital = await _hospitalsRepository.GetByIdAsync(id);
            if (hospital == null)
                throw new NotFoundException($"Hospital con ID {id} no encontrado.");

            return HospitalDTO.FromEntity(hospital);
        }

        public async Task<IEnumerable<HospitalDTO>> GetAllAsync()
        {
            var hospitals = await _hospitalsRepository.GetAllAsync();
            return hospitals.Select(HospitalDTO.FromEntity);
        }

        public async Task<HospitalDTO> CreateHospitalAsync(CreationHospitalDTO dto)
        {
            // Validaciones de datos de entrada
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ValidationException("El nombre del hospital es requerido.");

            if (string.IsNullOrWhiteSpace(dto.Address))
                throw new ValidationException("La dirección del hospital es requerida.");

            // ✅ Validar duplicado (nombre o dirección ya existente)
            var existingHospitals = await _hospitalsRepository.GetAllAsync();
            bool duplicate = existingHospitals.Any(h =>
                h.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase) ||
                h.Address.Equals(dto.Address, StringComparison.OrdinalIgnoreCase));

            if (duplicate)
                throw new DuplicateException($"Ya existe un hospital con el nombre '{dto.Name}' o la dirección '{dto.Address}'.");

            // Crear el hospital
            var hospital = new Hospital
            {
                Name = dto.Name,
                Address = dto.Address
            };

            var created = await _hospitalsRepository.CreateAsync(hospital);
            return HospitalDTO.FromEntity(created);
        }

        public async Task<HospitalDTO> UpdateHospitalAsync(int id, CreationHospitalDTO dto)
        {
            // Validaciones de datos de entrada
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ValidationException("El nombre del hospital es requerido.");

            if (string.IsNullOrWhiteSpace(dto.Address))
                throw new ValidationException("La dirección del hospital es requerida.");

            // Verificar que el hospital existe
            var hospital = await _hospitalsRepository.GetByIdAsync(id);
            if (hospital == null)
                throw new NotFoundException($"Hospital con ID {id} no encontrado.");

            // ✅ Verificar duplicado (si otro hospital tiene el mismo nombre o dirección)
            var allHospitals = await _hospitalsRepository.GetAllAsync();
            bool duplicate = allHospitals.Any(h =>
                h.Id != id &&
                (h.Name.Equals(dto.Name, StringComparison.OrdinalIgnoreCase) ||
                 h.Address.Equals(dto.Address, StringComparison.OrdinalIgnoreCase)));

            if (duplicate)
                throw new DuplicateException($"Ya existe otro hospital con el nombre '{dto.Name}' o la dirección '{dto.Address}'.");

            // Actualizar propiedades
            hospital.Name = dto.Name;
            hospital.Address = dto.Address;

            await _hospitalsRepository.UpdateAsync(hospital);
            return HospitalDTO.FromEntity(hospital);
        }

        public async Task DeleteHospitalAsync(int id)
        {
            // Verificar que el hospital existe antes de eliminar
            var hospital = await _hospitalsRepository.GetByIdAsync(id);
            if (hospital == null)
                throw new NotFoundException($"Hospital con ID {id} no encontrado.");

            await _hospitalsRepository.DeleteAsync(hospital);
        }

        // registrarProfesional
        public async Task RegisterProfessionalAsync(int hospitalId, int professionalId)
        {
            // Verificar que el hospital existe
            var hospital = await _hospitalsRepository.GetByIdAsync(hospitalId);
            if (hospital == null)
                throw new NotFoundException($"Hospital con ID {hospitalId} no encontrado.");

            // Verificar que el profesional existe
            var professional = await _professionalsRepository.GetByIdAsync(professionalId);
            if (professional == null)
                throw new NotFoundException($"Profesional con ID {professionalId} no encontrado.");

            // ✅ Verificar si el profesional ya está registrado en el hospital
            var currentProfessionals = await _hospitalsRepository.GetProfessionalByHospitalIdAsync(hospitalId);
            bool alreadyRegistered = currentProfessionals.Any(p => p.Id == professionalId);

            if (alreadyRegistered)
                throw new DuplicateException($"El profesional con ID {professionalId} ya está registrado en el hospital {hospital.Name}.");

            // Registrar el profesional en el hospital
            await _hospitalsRepository.RegisterProfessionalAsync(hospitalId, professionalId);
        }

        // editarProfesional (no implementa lógica)
        public Task EditProfessionalAsync(int hospitalId, int professionalId)
        {
            return Task.CompletedTask;
        }

        // listarProfesionales
        public async Task<IEnumerable<ProfessionalDTO>> ListProfessionalAsync(int hospitalId)
        {
            // Verificar que el hospital existe
            var hospital = await _hospitalsRepository.GetByIdAsync(hospitalId);
            if (hospital == null)
                throw new NotFoundException($"Hospital con ID {hospitalId} no encontrado.");

            // Obtener profesionales del hospital
            var professionals = await _hospitalsRepository.GetProfessionalByHospitalIdAsync(hospitalId);
            var dtos = new List<ProfessionalDTO>();

            foreach (var professional in professionals)
            {
                var specialty = await _specialtiesRepository.GetByIdAsync(professional.SpecialtyId);
                dtos.Add(ProfessionalDTO.FromEntity(professional, specialty?.Type ?? ""));
            }

            return dtos;
        }

        // eliminarProfesionales
        public async Task RemoveProfessionalAsync(int hospitalId, int professionalId)
        {
            // Verificar que el hospital existe
            var hospital = await _hospitalsRepository.GetByIdAsync(hospitalId);
            if (hospital == null)
                throw new NotFoundException($"Hospital con ID {hospitalId} no encontrado.");

            // Verificar que el profesional existe
            var professional = await _professionalsRepository.GetByIdAsync(professionalId);
            if (professional == null)
                throw new NotFoundException($"Profesional con ID {professionalId} no encontrado.");

            // Remover el profesional del hospital
            await _hospitalsRepository.RemoveProfessionalAsync(hospitalId, professionalId);
        }
    }
}
