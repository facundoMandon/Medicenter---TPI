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
    public class InsuranceService : IInsuranceService
    {
        private readonly IInsuranceRepository _insuranceRepository;
        private readonly IPatientsRepository _patientsRepository;

        // Mapeo Manual (sin AutoMapper)
        private InsuranceDTO MapToDto(Insurance insurance)
        {
            return new InsuranceDTO
            {
                Id = insurance.Id,
                Name = insurance.Name,
                Plan = insurance.Plan,
                Description = insurance.Description
            };
        }

        public InsuranceService(IInsuranceRepository insuranceRepository, IPatientsRepository patientsRepository)
        {
            _insuranceRepository = insuranceRepository;
            _patientsRepository = patientsRepository;
        }

        // --- CRUD Básico ---
        public async Task<InsuranceDTO> GetByIdAsync(int id)
        {
            var insurance = await _insuranceRepository.GetByIdAsync(id);
            return insurance != null ? MapToDto(insurance) : throw new KeyNotFoundException($"Obra Social ID {id} no encontrada.");
        }

        public async Task<IEnumerable<InsuranceDTO>> GetAllAsync()
        {
            var insurances = await _insuranceRepository.GetAllAsync();
            return insurances.Select(MapToDto);
        }

        public async Task<InsuranceDTO> CreateInsuranceAsync(CreationInsuranceDTO dto)
        {
            var insurance = new Insurance { Name = dto.Name, Plan = dto.Plan, Description = dto.Description };
            var created = await _insuranceRepository.CreateAsync(insurance);
            return MapToDto(created);
        }

        public async Task<InsuranceDTO> UpdateInsuranceAsync(int id, CreationInsuranceDTO dto)
        {
            var insurance = await _insuranceRepository.GetByIdAsync(id);
            if (insurance == null) throw new KeyNotFoundException($"Obra Social ID {id} no encontrada.");

            insurance.Name = dto.Name;
            insurance.Plan = dto.Plan;
            insurance.Description = dto.Description;

            await _insuranceRepository.UpdateAsync(insurance);
            return MapToDto(insurance);
        }

        public async Task DeleteInsuranceAsync(int id)
        {
            var insurance = await _insuranceRepository.GetByIdAsync(id);
            if (insurance == null) throw new KeyNotFoundException($"Obra Social ID {id} no encontrada.");
            await _insuranceRepository.DeleteAsync(insurance);
        }

        // --- MÉTODOS DE GESTIÓN DE AFILIADOS ---
        public async Task AddAffiliateAsync(int insuranceId, int patientId)
        {
            var insurance = await _insuranceRepository.GetByIdAsync(insuranceId);
            var patient = await _patientsRepository.GetByIdAsync(patientId);
            if (insurance == null) throw new KeyNotFoundException($"Obra Social ID {insuranceId} no encontrada.");
            if (patient == null) throw new KeyNotFoundException($"Paciente ID {patientId} no encontrado.");

            await _insuranceRepository.AddPatientToInsuranceAsync(insuranceId, patientId);
        }

        public async Task RemoveAffiliateAsync(int insuranceId, int patientId)
        {
            var patient = await _patientsRepository.GetByIdAsync(patientId);
            if (patient == null) throw new KeyNotFoundException($"Paciente ID {patientId} no encontrado.");
            await _insuranceRepository.RemovePatientFromInsuranceAsync(insuranceId, patientId);
        }

        public async Task ChangeCoverageAsync(int patientId, string newPlan)
        {
            var patient = await _patientsRepository.GetByIdAsync(patientId);
            if (patient == null) throw new KeyNotFoundException($"Paciente ID {patientId} no encontrado.");
            await _insuranceRepository.ChangePatientCoverageAsync(patientId, newPlan);
        }
    }
}
