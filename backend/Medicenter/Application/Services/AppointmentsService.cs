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
    public class AppointmentsService : IAppointmentsService
    {
        private readonly IAppointmentsRepository _appointmentsRepository;
        private readonly IPatientsRepository _patientsRepository;
        private readonly IProfessionalsRepository _professionalsRepository;

        public AppointmentsService(
            IAppointmentsRepository appointmentsRepository,
            IPatientsRepository patientsRepository,
            IProfessionalsRepository professionalsRepository)
        {
            _appointmentsRepository = appointmentsRepository;
            _patientsRepository = patientsRepository;
            _professionalsRepository = professionalsRepository;
        }

        public async Task<AppointmentsDTO> GetByIdAsync(int id)
        {
            var appointment = await _appointmentsRepository.GetByIdAsync(id);
            if (appointment == null)
                throw new KeyNotFoundException("Appointment not found.");

            return await MapToDtoAsync(appointment);
        }

        public async Task<IEnumerable<AppointmentsDTO>> GetAllAsync()
        {
            var appointments = await _appointmentsRepository.GetAllAsync();
            var dtos = new List<AppointmentsDTO>();

            foreach (var appointment in appointments)
            {
                dtos.Add(await MapToDtoAsync(appointment));
            }

            return dtos;
        }

        public async Task<AppointmentsDTO> AssignAppointmentAsync(CreationAppointmentDTO dto)
        {
            var patient = await _patientsRepository.GetByIdAsync(dto.PatientId);
            if (patient == null)
                throw new KeyNotFoundException($"Patient with ID {dto.PatientId} not found.");

            var professional = await _professionalsRepository.GetByIdAsync(dto.ProfessionalId);
            if (professional == null)
                throw new KeyNotFoundException($"Professional with ID {dto.ProfessionalId} not found.");

            var appointment = new Appointments
            {
                PatientId = dto.PatientId,
                ProfessionalId = dto.ProfessionalId,
                Fecha = dto.Fecha,
                Hora = dto.Hora,
                Descripcion = dto.Descripcion,
                Status = AppointmentStatus.Requested
            };

            var created = await _appointmentsRepository.CreateAsync(appointment);
            return await MapToDtoAsync(created);
        }

        public async Task<AppointmentsDTO> UpdateAppointmentAsync(int appointmentId, CreationAppointmentDTO dto)
        {
            var existing = await _appointmentsRepository.GetByIdAsync(appointmentId);
            if (existing == null)
                throw new KeyNotFoundException("Appointment not found.");

            existing.PatientId = dto.PatientId;
            existing.ProfessionalId = dto.ProfessionalId;
            existing.Fecha = dto.Fecha;
            existing.Hora = dto.Hora;
            existing.Descripcion = dto.Descripcion;

            await _appointmentsRepository.UpdateAsync(existing);
            return await MapToDtoAsync(existing);
        }

        public async Task<AppointmentsDTO> ConfirmAppointmentAsync(int appointmentId)
        {
            var existing = await _appointmentsRepository.GetByIdAsync(appointmentId);
            if (existing == null)
                throw new KeyNotFoundException("Appointment not found.");

            existing.Status = AppointmentStatus.Confirmed;
            await _appointmentsRepository.UpdateAsync(existing);

            return await MapToDtoAsync(existing);
        }

        public async Task CancelAppointmentAsync(int appointmentId)
        {
            var existing = await _appointmentsRepository.GetByIdAsync(appointmentId);
            if (existing == null)
                throw new KeyNotFoundException("Appointment not found.");

            existing.Status = AppointmentStatus.Cancelled;
            await _appointmentsRepository.UpdateAsync(existing);
        }

        private async Task<AppointmentsDTO> MapToDtoAsync(Appointments appointment)
        {
            string patientName = "";
            string professionalName = "";
            string specialtyName = "";

            var patient = await _patientsRepository.GetByIdAsync(appointment.PatientId);
            if (patient != null)
                patientName = $"{patient.Name} {patient.LastName}";

            var professional = await _professionalsRepository.GetByIdAsync(appointment.ProfessionalId);
            if (professional != null)
            {
                professionalName = $"{professional.Name} {professional.LastName}";
                specialtyName = professional.Specialty?.Tipo ?? ""; // CORREGIDO: Tipo
            }

            return AppointmentsDTO.FromEntity(appointment, patientName, professionalName, specialtyName);
        }
    }
}
