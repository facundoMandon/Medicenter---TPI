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
    public class ProfessionalService : IProfessionalService
    {
        private readonly IProfessionalRepository _professionalsRepository;
        private readonly IAppointmentRepository _appointmentsRepository;
        private readonly IPatientRepository _patientsRepository;
        private readonly ISpecialtiesRepository _specialtiesRepository;
        private readonly IInsuranceRepository _insuranceRepository;

        public ProfessionalService(
            IProfessionalRepository professionalsRepository,
            IAppointmentRepository appointmentsRepository,
            IPatientRepository patientsRepository,
            ISpecialtiesRepository specialtiesRepository,
            IInsuranceRepository insuranceRepository)
        {
            _professionalsRepository = professionalsRepository;
            _appointmentsRepository = appointmentsRepository;
            _patientsRepository = patientsRepository;
            _specialtiesRepository = specialtiesRepository;
            _insuranceRepository = insuranceRepository;
        }

        public async Task<ProfessionalDTO> CreateProfessionalAsync(CreationProfessionalDTO dto)
        {
            // ✅ Obtener todos los profesionales existentes
            var existingProfessional = await _professionalsRepository.GetAllAsync();

            // Normalizar datos para evitar falsos duplicados
            string emailNormalized = dto.Email.Trim().ToLower();
     

            // Verificar duplicados por DNI, Email o Número de Matrícula
            bool exists = existingProfessional.Any(p =>
                p.DNI == dto.DNI ||
                p.Email.Trim().ToLower() == emailNormalized ||
                p.LicenseNumber == dto.LicenseNumber
            );

            if (exists)
                throw new ArgumentException(
                    $"Ya existe un profesional registrado con el mismo DNI ({dto.DNI}), correo electrónico ({dto.Email}) o número de matrícula ({dto.LicenseNumber})."
                );

            // Crear nuevo profesional si no hay duplicado
            var professional = new Professional
            {
                Name = dto.Name,
                LastName = dto.LastName,
                DNI = dto.DNI,
                Email = dto.Email,
                Password = dto.Password, // ⚠️ sin hash, pendiente de encriptar
                Rol = dto.Rol,
                LicenseNumber = dto.LicenseNumber,
                SpecialtyId = dto.SpecialtyId
            };

            var created = await _professionalsRepository.CreateAsync(professional);

            var specialty = await _specialtiesRepository.GetByIdAsync(dto.SpecialtyId);
            string specialtyName = specialty?.Type ?? "";

            return ProfessionalDTO.FromEntity(created, specialtyName);
        }

        public async Task<IEnumerable<AppointmentDTO>> ViewAppointmentAsync(int professionalId)
        {
            var appointments = await _appointmentsRepository.GetByProfessionalIdAsync(professionalId);

            return appointments.Select(a => AppointmentDTO.FromEntity(
                a,
                $"{a.Patient.Name} {a.Patient.LastName}",
                $"{a.Professional.Name} {a.Professional.LastName}",
                a.Professional.Specialty?.Type ?? "" // CORREGIDO: Type en vez de Name
            ));
        }

        public async Task<bool> AcceptAppointmentAsync(int professionalId, int appointmentId)
        {
            var appointment = await _appointmentsRepository.GetByIdAsync(appointmentId);

            if (appointment == null || appointment.ProfessionalId != professionalId)
                return false;

            appointment.Status = Domain.Enums.AppointmentStatus.Accepted;
            await _appointmentsRepository.UpdateAsync(appointment);
            return true;
        }

        public async Task<bool> RejectAppointmentAsync(int professionalId, int appointmentId)
        {
            var appointment = await _appointmentsRepository.GetByIdAsync(appointmentId);

            if (appointment == null || appointment.ProfessionalId != professionalId)
                return false;

            appointment.Status = Domain.Enums.AppointmentStatus.Rejected;
            await _appointmentsRepository.UpdateAsync(appointment);
            return true;
        }

        public async Task<IEnumerable<PatientDTO>> ListPatientAsync(int professionalId)
        {
            var appointments = await _appointmentsRepository.GetByProfessionalIdAsync(professionalId);

            var patientIds = appointments.Select(a => a.PatientId).Distinct();
            var patients = new List<PatientDTO>();

            foreach (var patientId in patientIds)
            {
                var patient = await _patientsRepository.GetByIdAsync(patientId);
                if (patient != null)
                {
                    var insurance = await _insuranceRepository.GetByIdAsync(patient.InsuranceId);
                    patients.Add(PatientDTO.FromEntity(patient, insurance?.Name ?? ""));
                }
            }

            return patients;
        }
    }
}
