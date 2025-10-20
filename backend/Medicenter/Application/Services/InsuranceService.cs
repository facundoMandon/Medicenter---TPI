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
        private readonly IPatientsRepository _patientsRepository;

        public InsuranceService(IInsuranceRepository insuranceRepository, IPatientsRepository patientsRepository)
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
            var insurance = new Insurance
            {
                Nombre = dto.Nombre,
                TipoCobertura = dto.TipoCobertura,
                Descripcion = dto.Descripcion
            };

            var created = await _insuranceRepository.CreateAsync(insurance);
            return InsuranceDTO.FromEntity(created);
        }

        public async Task<InsuranceDTO> UpdateInsuranceAsync(int id, CreationInsuranceDTO dto)
        {
            var insurance = await _insuranceRepository.GetByIdAsync(id);
            if (insurance == null)
                throw new KeyNotFoundException($"Obra Social ID {id} no encontrada.");

            insurance.Nombre = dto.Nombre;
            insurance.TipoCobertura = dto.TipoCobertura;
            insurance.Descripcion = dto.Descripcion;

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

        // añadirAfiliado()
        public async Task AddAffiliateAsync(int insuranceId, int patientId)
        {
            var insurance = await _insuranceRepository.GetByIdAsync(insuranceId);
            var patient = await _patientsRepository.GetByIdAsync(patientId);

            if (insurance == null)
                throw new KeyNotFoundException($"Obra Social ID {insuranceId} no encontrada.");
            if (patient == null)
                throw new KeyNotFoundException($"Paciente ID {patientId} no encontrado.");

            await _insuranceRepository.AddPatientToInsuranceAsync(insuranceId, patientId);
        }

        // eliminarAfiliado()
        public async Task RemoveAffiliateAsync(int insuranceId, int patientId)
        {
            var patient = await _patientsRepository.GetByIdAsync(patientId);
            if (patient == null)
                throw new KeyNotFoundException($"Paciente ID {patientId} no encontrado.");

            await _insuranceRepository.RemovePatientFromInsuranceAsync(insuranceId, patientId);
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
