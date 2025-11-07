using Application.Interfaces;
using Application.Models;
using Application.Models.Request;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

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
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ValidationException("El nombre de la obra social es requerido.");

            if (string.IsNullOrWhiteSpace(dto.Description))
                throw new ValidationException("La descripción de la obra social es requerida.");

            // 🧠 Verificar duplicado por nombre antes de crear
            var existing = await _insuranceRepository.GetByNameAsync(dto.Name);
            if (existing != null)
                throw new DuplicateException($"Ya existe una obra social con el nombre '{dto.Name}'.");

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
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ValidationException("El nombre de la obra social es requerido.");

            if (string.IsNullOrWhiteSpace(dto.Description))
                throw new ValidationException("La descripción de la obra social es requerida.");

            var insurance = await _insuranceRepository.GetByIdAsync(id);
            if (insurance == null)
                throw new NotFoundException($"Obra Social con ID {id} no encontrada.");

            // 🧠 Verificar si ya existe otra obra social con el mismo nombre
            var duplicate = await _insuranceRepository.GetByNameAsync(dto.Name);
            if (duplicate != null && duplicate.Id != id)
                throw new DuplicateException($"Ya existe otra obra social con el nombre '{dto.Name}'.");

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
                throw new NotFoundException($"Obra Social con ID {id} no encontrada.");

            await _insuranceRepository.DeleteAsync(insurance);
        }

        public async Task ChangeCoverageAsync(int patientId, MedicalCoverageType newCoverage)
        {
            var patient = await _patientsRepository.GetByIdAsync(patientId);
            if (patient == null)
                throw new NotFoundException($"Paciente con ID {patientId} no encontrado.");

            await _insuranceRepository.ChangePatientCoverageAsync(patientId, newCoverage);
        }
    }
}
