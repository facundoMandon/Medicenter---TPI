using Application.Interfaces;
using Application.Models;
using Application.Models.Request;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
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
                throw new KeyNotFoundException($"Obra Social ID {id} no encontrada.");

            return InsuranceDTO.FromEntity(insurance);
        }

        public async Task<IEnumerable<InsuranceDTO>> GetAllAsync()
        {
            var insurances = await _insuranceRepository.GetAllAsync();
            return insurances.Select(InsuranceDTO.FromEntity);
        }

        public async Task<InsuranceDTO> CreateInsuranceAsync(CreationInsuranceDTO dto)
        {
            // Normalizar valores para evitar falsos duplicados
            string nombreNormalizado = dto.Name.Trim().ToLower();
            string tipoCoberturaNormalizado = dto.MedicalCoverageType.ToString().Trim().ToLower();

            // ✅ Verificar si ya existe una obra social con mismo nombre y tipo de cobertura
            var existingInsurances = await _insuranceRepository.GetAllAsync();
            bool exists = existingInsurances.Any(i =>
                i.Name.Trim().ToLower() == nombreNormalizado &&
                i.MedicalCoverageType.ToString().Trim().ToLower() == tipoCoberturaNormalizado
            );

            if (exists)
                throw new ArgumentException($"Ya existe una obra social con el nombre \"{dto.Name}\" y tipo de cobertura \"{dto.MedicalCoverageType}\".");

            // Crear nueva obra social si no hay duplicado
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
            var insurance = await _insuranceRepository.GetByIdAsync(id);
            if (insurance == null)
                throw new KeyNotFoundException($"Obra Social ID {id} no encontrada.");

            insurance.Name = dto.Name;
            insurance.MedicalCoverageType = dto.MedicalCoverageType;
            insurance.Description = dto.Description;

            await _insuranceRepository.UpdateAsync(insurance);
            return InsuranceDTO.FromEntity(insurance);
        }

        public async Task DeleteInsuranceAsync(int id)
        {
            var insurance = await _insuranceRepository.GetByIdAsync(id);
            if (insurance == null)
                throw new KeyNotFoundException($"Obra Social ID {id} no encontrada.");

            await _insuranceRepository.DeleteAsync(insurance);
        }

        // cambiarCobertura()
        public async Task ChangeCoverageAsync(int patientId, MedicalCoverageType newCoverage)
        {
            var patient = await _patientsRepository.GetByIdAsync(patientId);
            if (patient == null)
                throw new KeyNotFoundException($"Paciente ID {patientId} no encontrado.");

            await _insuranceRepository.ChangePatientCoverageAsync(patientId, newCoverage);
        }
    }
}
