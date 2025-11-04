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
                throw new KeyNotFoundException($"Hospital con ID {id} no encontrado.");

            return HospitalDTO.FromEntity(hospital);
        }

        public async Task<IEnumerable<HospitalDTO>> GetAllAsync()
        {
            var hospitals = await _hospitalsRepository.GetAllAsync();
            return hospitals.Select(HospitalDTO.FromEntity);
        }

        public async Task<HospitalDTO> CreateHospitalAsync(CreationHospitalDTO dto)
        {
            // Normalizar los valores (para evitar falsos negativos por mayúsculas o espacios)
            string nombreNormalizado = dto.Name.Trim().ToLower();
            string direccionNormalizada = dto.Adress.Trim().ToLower();

            // ✅ Verificar si ya existe un hospital con mismo nombre y dirección
            var existingHospital = await _hospitalsRepository.GetAllAsync();
            bool exists = existingHospital.Any(h =>
                h.Name.Trim().ToLower() == nombreNormalizado &&
                h.Adress.Trim().ToLower() == direccionNormalizada
            );

            if (exists)
                throw new ArgumentException($"Ya existe un hospital con el nombre \"{dto.Name}\" y la dirección \"{dto.Adress}\".");

            // Crear nuevo hospital si no existe duplicado
            var hospital = new Hospital
            {
                Name = dto.Name,
                Adress = dto.Adress
            };

            var created = await _hospitalsRepository.CreateAsync(hospital);
            return HospitalDTO.FromEntity(created);
        }

        public async Task<HospitalDTO> UpdateHospitalAsync(int id, CreationHospitalDTO dto)
        {
            var hospital = await _hospitalsRepository.GetByIdAsync(id);
            if (hospital == null)
                throw new KeyNotFoundException($"Hospital con ID {id} no encontrado.");

            hospital.Name = dto.Name;
            hospital.Adress = dto.Adress;

            await _hospitalsRepository.UpdateAsync(hospital);
            return HospitalDTO.FromEntity(hospital);
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

        // editarProfesional (Este método no hace nada porque la edición se hace en UserService)
        public Task EditProfessionalAsync(int hospitalId, int professionalId)
        {
            // Este método existe solo para cumplir con el contrato del diagrama
            // La edición real del profesional se hace en UserService o ProfessionalService
            return Task.CompletedTask;
        }

        // listarProfesionales
        public async Task<IEnumerable<ProfessionalDTO>> ListProfessionalAsync(int hospitalId)
        {
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
            await _hospitalsRepository.RemoveProfessionalAsync(hospitalId, professionalId);
        }
    }
}
