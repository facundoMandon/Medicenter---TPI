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
        private readonly IProfessionalRepository _professionalsRepository;

        public SpecialtiesService(ISpecialtiesRepository specialtiesRepository, IProfessionalRepository professionalsRepository)
        {
            _specialtiesRepository = specialtiesRepository;
            _professionalsRepository = professionalsRepository;
        }

        public async Task<SpecialtiesDTO> GetByIdAsync(int id)
        {
            var specialty = await _specialtiesRepository.GetByIdAsync(id);
            if (specialty == null)
                throw new KeyNotFoundException($"Specialty with ID {id} not found.");

            return SpecialtiesDTO.FromEntity(specialty);
        }

        public async Task<IEnumerable<SpecialtiesDTO>> GetAllAsync()
        {
            var specialties = await _specialtiesRepository.GetAllAsync();
            return specialties.Select(SpecialtiesDTO.FromEntity);
        }

        // añadirEspecialidad()
        public async Task<SpecialtiesDTO> CreateSpecialtyAsync(CreationSpecialtiesDTO dto)
        {
            // ✅ Obtener todas las especialidades existentes
            var existingSpecialties = await _specialtiesRepository.GetAllAsync();

            string tipoNormalized = dto.Type.Trim().ToLower();

            // Verificar duplicado por nombre (Type)
            bool exists = existingSpecialties.Any(s => s.Type.Trim().ToLower() == tipoNormalized);

            if (exists)
                throw new ArgumentException($"Ya existe una especialidad registrada con el nombre '{dto.Type}'.");

            // Crear nueva especialidad
            var specialty = new Specialties
            {
                Type = dto.Type,
                Description = dto.Description
            };

            var created = await _specialtiesRepository.CreateAsync(specialty);
            return SpecialtiesDTO.FromEntity(created);
        }

        // modificarEspecialidad()
        public async Task<SpecialtiesDTO> UpdateSpecialtyAsync(int id, CreationSpecialtiesDTO dto)
        {
            var existing = await _specialtiesRepository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"Specialty with ID {id} not found.");

            // ✅ Verificar duplicado al modificar (excluyendo la propia especialidad)
            var allSpecialties = await _specialtiesRepository.GetAllAsync();
            string tipoNormalized = dto.Type.Trim().ToLower();

            bool exists = allSpecialties.Any(s =>
                s.Id != id && s.Type.Trim().ToLower() == tipoNormalized
            );

            if (exists)
                throw new ArgumentException($"Ya existe otra especialidad con el nombre '{dto.Type}'.");

            // Actualizar datos
            existing.Type = dto.Type;
            existing.Description = dto.Description;

            await _specialtiesRepository.UpdateAsync(existing);
            return SpecialtiesDTO.FromEntity(existing);
        }


        // eliminarEspecialidad()
        public async Task DeleteSpecialtyAsync(int id)
        {
            var existing = await _specialtiesRepository.GetByIdAsync(id);
            if (existing == null)
                throw new KeyNotFoundException($"Specialty with ID {id} not found.");

            await _specialtiesRepository.DeleteAsync(existing);
        }

        // asignarEspecialidad(profesional)
        public async Task AssignSpecialtyToProfessionalAsync(int specialtyId, int professionalId)
        {
            var specialty = await _specialtiesRepository.GetByIdAsync(specialtyId);
            if (specialty == null)
                throw new KeyNotFoundException($"Specialty with ID {specialtyId} not found.");

            var professional = await _professionalsRepository.GetByIdAsync(professionalId);
            if (professional == null)
                throw new KeyNotFoundException($"Professional with ID {professionalId} not found.");

            professional.SpecialtyId = specialtyId;
            await _professionalsRepository.UpdateAsync(professional);
        }

        // quitarEspecialidad(profesional)
        public async Task RemoveSpecialtyFromProfessionalAsync(int specialtyId, int professionalId)
        {
            var professional = await _professionalsRepository.GetByIdAsync(professionalId);
            if (professional == null)
                throw new KeyNotFoundException($"Professional with ID {professionalId} not found.");

            if (professional.SpecialtyId == specialtyId)
            {
                professional.SpecialtyId = 0; // O null si es nullable
                await _professionalsRepository.UpdateAsync(professional);
            }
        }
    }
}
