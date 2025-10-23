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
    public class ProfessionalsService : IProfessionalsService
    {
        private readonly IProfessionalsRepository _professionalsRepository;
        private readonly IAppointmentsRepository _appointmentsRepository;
        private readonly IPatientsRepository _patientsRepository;
        private readonly ISpecialtiesRepository _specialtiesRepository;
        private readonly IInsuranceRepository _insuranceRepository;

        public ProfessionalsService(
            IProfessionalsRepository professionalsRepository,
            IAppointmentsRepository appointmentsRepository,
            IPatientsRepository patientsRepository,
            ISpecialtiesRepository specialtiesRepository,
            IInsuranceRepository insuranceRepository)
        {
            _professionalsRepository = professionalsRepository;
            _appointmentsRepository = appointmentsRepository;
            _patientsRepository = patientsRepository;
            _specialtiesRepository = specialtiesRepository;
            _insuranceRepository = insuranceRepository;
        }

        public async Task<ProfessionalsDTO> CreateProfessionalAsync(CreationProfessionalsDTO dto)
        {
            var professional = new Professionals
            {
                Name = dto.Name,
                LastName = dto.LastName,
                DNI = dto.DNI,
                Email = dto.Email,
                Password = dto.Password,
                Rol = dto.Rol,
                LicenseNumber = dto.LicenseNumber,
                SpecialtyId = dto.SpecialtyId
            };

            var created = await _professionalsRepository.CreateAsync(professional);

            var specialty = await _specialtiesRepository.GetByIdAsync(dto.SpecialtyId);
            string specialtyName = specialty?.Tipo ?? ""; // CORREGIDO: Tipo en vez de Name

            return ProfessionalsDTO.FromEntity(created, specialtyName);
        }

        public async Task<IEnumerable<AppointmentsDTO>> ViewAppointmentsAsync(int professionalId)
        {
            var appointments = await _appointmentsRepository.GetByProfessionalIdAsync(professionalId);

            return appointments.Select(a => AppointmentsDTO.FromEntity(
                a,
                $"{a.Patient.Name} {a.Patient.LastName}",
                $"{a.Professional.Name} {a.Professional.LastName}",
                a.Professional.Specialty?.Tipo ?? "" // CORREGIDO: Tipo en vez de Name
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

        public async Task<IEnumerable<PatientsDTO>> ListPatientsAsync(int professionalId)
        {
            var appointments = await _appointmentsRepository.GetByProfessionalIdAsync(professionalId);

            var patientIds = appointments.Select(a => a.PatientId).Distinct();
            var patients = new List<PatientsDTO>();

            foreach (var patientId in patientIds)
            {
                var patient = await _patientsRepository.GetByIdAsync(patientId);
                if (patient != null)
                {
                    var insurance = await _insuranceRepository.GetByIdAsync(patient.InsuranceId);
                    patients.Add(PatientsDTO.FromEntity(patient, insurance?.Nombre ?? ""));
                }
            }

            return patients;
        }
    }
}
