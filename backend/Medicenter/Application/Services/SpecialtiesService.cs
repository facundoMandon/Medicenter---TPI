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
    public class SpecialtiesService : ISpecialtiesService
    {
        private readonly ISpecialtiesRepository _specialtiesRepository;
        // Se asume IProfessionalsRepository para las validaciones
        private readonly IProfessionalsRepository _professionalsRepository;

        public SpecialtiesService(ISpecialtiesRepository specialtiesRepository, IProfessionalsRepository professionalsRepository)
        {
            _specialtiesRepository = specialtiesRepository;
            _professionalsRepository = professionalsRepository;
        }

        // --- Mapeo Interno ---
        private SpecialtiesDTO MapToDto(Specialties specialty)
        {
            return new SpecialtiesDTO
            {
                Id = specialty.Id,
                Name = specialty.Name,
                Description = specialty.Description
            };
        }

        // --- CRUD Básico ---
        public async Task<SpecialtiesDTO> GetByIdAsync(int id)
        {
            var specialty = await _specialtiesRepository.GetByIdAsync(id);
            return specialty != null ? MapToDto(specialty) : throw new KeyNotFoundException($"Specialty with ID {id} not found.");
        }

        public async Task<IEnumerable<SpecialtiesDTO>> GetAllAsync()
        {
            var specialties = await _specialtiesRepository.GetAllAsync();
            return specialties.Select(MapToDto);
        }

        // añadirEspecialidad
        public async Task<SpecialtiesDTO> CreateSpecialtyAsync(CreationSpecialtiesDTO dto)
        {
            var specialty = new Specialties
            {
                Name = dto.Name,
                Description = dto.Description
            };
            var created = await _specialtiesRepository.CreateAsync(specialty);
            return MapToDto(created);
        }

        // modificarEspecialidad
        public async Task<SpecialtiesDTO> UpdateSpecialtyAsync(int id, CreationSpecialtiesDTO dto)
        {
            var existing = await _specialtiesRepository.GetByIdAsync(id);
            if (existing == null) throw new KeyNotFoundException($"Specialty with ID {id} not found.");

            existing.Name = dto.Name;
            existing.Description = dto.Description;

            await _specialtiesRepository.UpdateAsync(existing);
            return MapToDto(existing);
        }

        // eliminarEspecialidad
        public async Task DeleteSpecialtyAsync(int id)
        {
            var existing = await _specialtiesRepository.GetByIdAsync(id);
            if (existing == null) throw new KeyNotFoundException($"Specialty with ID {id} not found.");

            await _specialtiesRepository.DeleteAsync(existing);
        }

        // --- Métodos de Relación ---

        // asignarEspecialidad
        public async Task AssignSpecialtyToProfessionalAsync(int specialtyId, int professionalId)
        {
            // Lógica de negocio: 
            // 1. Verificar si la especialidad y el profesional existen.
            // 2. Usar el repositorio para manejar la relación N:M o 1:N (según tu modelo).

            // Suponiendo una relación 1:N (un profesional tiene una SpecialtyId FK)
            var professional = await _professionalsRepository.GetByIdAsync(professionalId);
            if (professional == null) throw new KeyNotFoundException($"Professional ID {professionalId} not found.");

            professional.SpecialtyId = specialtyId; // Asignación de FK
            await _professionalsRepository.UpdateAsync(professional);
        }

        // quitarEspecialidad
        public async Task RemoveSpecialtyFromProfessionalAsync(int specialtyId, int professionalId)
        {
            var professional = await _professionalsRepository.GetByIdAsync(professionalId);
            if (professional == null) throw new KeyNotFoundException($"Professional ID {professionalId} not found.");

            // Asumiendo que 0 o null representa "sin especialidad"
            if (professional.SpecialtyId == specialtyId)
            {
                professional.SpecialtyId = 0;
                await _professionalsRepository.UpdateAsync(professional);
            }
        }
    }
}
