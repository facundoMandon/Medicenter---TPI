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
                throw new NotFoundException($"Especialidad con ID {id} no encontrada.");

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
            // Validaciones de datos de entrada
            if (string.IsNullOrWhiteSpace(dto.Type))
                throw new ValidationException("El tipo de especialidad es requerido.");

            if (string.IsNullOrWhiteSpace(dto.Description))
                throw new ValidationException("La descripción de la especialidad es requerida.");

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
            // Validaciones de datos de entrada
            if (string.IsNullOrWhiteSpace(dto.Type))
                throw new ValidationException("El tipo de especialidad es requerido.");

            if (string.IsNullOrWhiteSpace(dto.Description))
                throw new ValidationException("La descripción de la especialidad es requerida.");

            // Verificar que la especialidad existe
            var existing = await _specialtiesRepository.GetByIdAsync(id);
            if (existing == null)
                throw new NotFoundException($"Especialidad con ID {id} no encontrada.");

            // Actualizar propiedades
            existing.Type = dto.Type;
            existing.Description = dto.Description;

            await _specialtiesRepository.UpdateAsync(existing);
            return SpecialtiesDTO.FromEntity(existing);
        }

        // eliminarEspecialidad()
        public async Task DeleteSpecialtyAsync(int id)
        {
            // Verificar que la especialidad existe antes de eliminar
            var existing = await _specialtiesRepository.GetByIdAsync(id);
            if (existing == null)
                throw new NotFoundException($"Especialidad con ID {id} no encontrada.");

            await _specialtiesRepository.DeleteAsync(existing);
        }

        // asignarEspecialidad(profesional)
        public async Task AssignSpecialtyToProfessionalAsync(int specialtyId, int professionalId)
        {
            // Verificar que la especialidad existe
            var specialty = await _specialtiesRepository.GetByIdAsync(specialtyId);
            if (specialty == null)
                throw new NotFoundException($"Especialidad con ID {specialtyId} no encontrada.");

            // Verificar que el profesional existe
            var professional = await _professionalsRepository.GetByIdAsync(professionalId);
            if (professional == null)
                throw new NotFoundException($"Profesional con ID {professionalId} no encontrado.");

            // Asignar la especialidad al profesional
            professional.SpecialtyId = specialtyId;
            await _professionalsRepository.UpdateAsync(professional);
        }

        // quitarEspecialidad(profesional)
        public async Task RemoveSpecialtyFromProfessionalAsync(int specialtyId, int professionalId)
        {
            // Verificar que el profesional existe
            var professional = await _professionalsRepository.GetByIdAsync(professionalId);
            if (professional == null)
                throw new NotFoundException($"Profesional con ID {professionalId} no encontrado.");

            // Validar que el profesional tiene asignada esta especialidad
            if (professional.SpecialtyId != specialtyId)
                throw new ValidationException($"El profesional con ID {professionalId} no tiene asignada la especialidad con ID {specialtyId}.");

            // Quitar la especialidad del profesional
            professional.SpecialtyId = 0; // O null si es nullable
            await _professionalsRepository.UpdateAsync(professional);
        }
    }
}