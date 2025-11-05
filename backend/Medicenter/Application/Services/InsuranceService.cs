using Application.Interfaces;
using Application.Models;
using Application.Models.Request;
using Domain.Entities;
using Domain.Enums;
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
    public class InsuranceService : IInsuranceService
    {
        private readonly IInsuranceRepository _insuranceRepository;
        private readonly IPatientRepository _patientsRepository;

        public InsuranceService(IInsuranceRepository insuranceRepository, IPatientRepository patientsRepository)
        {
            _insuranceRepository = insuranceRepository;
            _patientsRepository = patientsRepository;
        }

        public async Task<InsuranceDTO> GetByIdAsync(int id)
        {
            var insurance = await _insuranceRepository.GetByIdAsync(id);
            if (insurance == null)
                throw new NotFoundException($"Obra Social con ID {id} no encontrada.");

            return InsuranceDTO.FromEntity(insurance);
        }

        public async Task<IEnumerable<InsuranceDTO>> GetAllAsync()
        {
            var insurances = await _insuranceRepository.GetAllAsync();
            return insurances.Select(InsuranceDTO.FromEntity);
        }

        public async Task<InsuranceDTO> CreateInsuranceAsync(CreationInsuranceDTO dto)
        {
            // Validaciones de datos de entrada
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ValidationException("El nombre de la obra social es requerido.");

            if (string.IsNullOrWhiteSpace(dto.Description))
                throw new ValidationException("La descripción de la obra social es requerida.");

            // Crear la obra social
            var insurance = new Insurance
            {
                Name = dto.Name,
                MedicalCoverageType = dto.MedicalCoverageType,
                Description = dto.Description
            };

            var created = await _insuranceRepository.CreateAsync(insurance);
            return InsuranceDTO.FromEntity(created);
        }

        public async Task<InsuranceDTO> UpdateInsuranceAsync(int id, CreationInsuranceDTO dto)
        {
            // Validaciones de datos de entrada
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ValidationException("El nombre de la obra social es requerido.");

            if (string.IsNullOrWhiteSpace(dto.Description))
                throw new ValidationException("La descripción de la obra social es requerida.");

            // Verificar que la obra social existe
            var insurance = await _insuranceRepository.GetByIdAsync(id);
            if (insurance == null)
                throw new NotFoundException($"Obra Social con ID {id} no encontrada.");

            // Actualizar propiedades
            insurance.Name = dto.Name;
            insurance.MedicalCoverageType = dto.MedicalCoverageType;
            insurance.Description = dto.Description;

            await _insuranceRepository.UpdateAsync(insurance);
            return InsuranceDTO.FromEntity(insurance);
        }

        public async Task DeleteInsuranceAsync(int id)
        {
            // Verificar que la obra social existe antes de eliminar
            var insurance = await _insuranceRepository.GetByIdAsync(id);
            if (insurance == null)
                throw new NotFoundException($"Obra Social con ID {id} no encontrada.");

            await _insuranceRepository.DeleteAsync(insurance);
        }

        // cambiarCobertura()
        public async Task ChangeCoverageAsync(int patientId, MedicalCoverageType newCoverage)
        {
            // Verificar que el paciente existe
            var patient = await _patientsRepository.GetByIdAsync(patientId);
            if (patient == null)
                throw new NotFoundException($"Paciente con ID {patientId} no encontrado.");

            // Cambiar la cobertura del paciente
            await _insuranceRepository.ChangePatientCoverageAsync(patientId, newCoverage);
        }
    }
}