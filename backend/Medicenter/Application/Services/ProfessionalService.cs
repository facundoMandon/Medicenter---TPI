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
            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ValidationException("El nombre es requerido.");

            if (string.IsNullOrWhiteSpace(dto.LastName))
                throw new ValidationException("El apellido es requerido.");

            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ValidationException("El email es requerido.");

            if (!IsValidEmail(dto.Email))
                throw new ValidationException("El formato del email no es válido.");

            if (string.IsNullOrWhiteSpace(dto.Password))
                throw new ValidationException("La contraseña es requerida.");

            if (dto.LicenseNumber <= 0)
                throw new ValidationException("El número de matrícula es requerido.");

            if (dto.SpecialtyId <= 0)
                throw new ValidationException("Debe especificar un ID de especialidad válido.");

            // ✅ Verificar duplicados de Email
            var existingEmail = await _professionalsRepository.GetByEmailAsync(dto.Email);
            if (existingEmail != null)
                throw new DuplicateException($"Ya existe otro profesional con el email '{dto.Email}'.");

            // ✅ Verificar duplicados de DNI
            var existingDni = await _professionalsRepository.GetByDniAsync(dto.DNI);
            if (existingDni != null)
                throw new DuplicateException($"Ya existe otro profesional con el DNI '{dto.DNI}'.");

            // Verificar que la especialidad existe
            var specialty = await _specialtiesRepository.GetByIdAsync(dto.SpecialtyId);
            if (specialty == null)
                throw new NotFoundException($"No existe una especialidad con el ID {dto.SpecialtyId}.");

            // Crear el profesional
            var professional = new Professional
            {
                Name = dto.Name,
                LastName = dto.LastName,
                DNI = dto.DNI,
                Email = dto.Email,
                Password = dto.Password, // 🔒 Debería encriptarse
                Rol = dto.Rol,
                LicenseNumber = dto.LicenseNumber,
                SpecialtyId = dto.SpecialtyId
            };

            var created = await _professionalsRepository.CreateAsync(professional);
            string specialtyName = specialty.Type;

            return ProfessionalDTO.FromEntity(created, specialtyName);
        }

        public async Task<IEnumerable<AppointmentDTO>> ViewAppointmentAsync(int professionalId)
        {
            // Verificar que el profesional existe
            var professional = await _professionalsRepository.GetByIdAsync(professionalId);
            if (professional == null)
                throw new NotFoundException($"Profesional con ID {professionalId} no encontrado.");

            // Obtener turnos del profesional
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
            // Verificar que el profesional existe
            var professional = await _professionalsRepository.GetByIdAsync(professionalId);
            if (professional == null)
                throw new NotFoundException($"Profesional con ID {professionalId} no encontrado.");

            // Verificar que el turno existe
            var appointment = await _appointmentsRepository.GetByIdAsync(appointmentId);
            if (appointment == null)
                throw new NotFoundException($"Turno con ID {appointmentId} no encontrado.");

            // Verificar que el turno pertenece al profesional
            if (appointment.ProfessionalId != professionalId)
                throw new ValidationException("El turno no pertenece a este profesional.");

            // Aceptar el turno
            appointment.Status = Domain.Enums.AppointmentStatus.Accepted;
            await _appointmentsRepository.UpdateAsync(appointment);
            return true;
        }

        public async Task<bool> RejectAppointmentAsync(int professionalId, int appointmentId)
        {
            // Verificar que el profesional existe
            var professional = await _professionalsRepository.GetByIdAsync(professionalId);
            if (professional == null)
                throw new NotFoundException($"Profesional con ID {professionalId} no encontrado.");

            // Verificar que el turno existe
            var appointment = await _appointmentsRepository.GetByIdAsync(appointmentId);
            if (appointment == null)
                throw new NotFoundException($"Turno con ID {appointmentId} no encontrado.");

            // Verificar que el turno pertenece al profesional
            if (appointment.ProfessionalId != professionalId)
                throw new ValidationException("El turno no pertenece a este profesional.");

            // Rechazar el turno
            appointment.Status = Domain.Enums.AppointmentStatus.Rejected;
            await _appointmentsRepository.UpdateAsync(appointment);
            return true;
        }

        public async Task<IEnumerable<PatientDTO>> ListPatientAsync(int professionalId)
        {
            // Verificar que el profesional existe
            var professional = await _professionalsRepository.GetByIdAsync(professionalId);
            if (professional == null)
                throw new NotFoundException($"Profesional con ID {professionalId} no encontrado.");

            // Obtener turnos del profesional
            var appointments = await _appointmentsRepository.GetByProfessionalIdAsync(professionalId);

            // Extraer IDs únicos de pacientes
            var patientIds = appointments.Select(a => a.PatientId).Distinct();
            var patients = new List<PatientDTO>();

            // Obtener información de cada paciente
            foreach (var patientId in patientIds)
            {
                var patient = await _patientsRepository.GetByIdAsync(patientId);
                if (patient != null)
                {
                    var insurance = await _insuranceRepository.GetByIdAsync(patient.InsuranceId);
                    string insuranceName = insurance?.Name ?? "";
                    patients.Add(PatientDTO.FromEntity(patient, insuranceName));
                }
            }

            return patients;
        }

        // Método auxiliar para validar formato de email
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}