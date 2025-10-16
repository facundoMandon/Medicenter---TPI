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
        // Asumimos IUsersRepository para lookups de nombres, etc.
        private readonly IUsersRepository _usersRepository;

        public AppointmentsService(IAppointmentsRepository appointmentsRepository, IUsersRepository usersRepository)
        {
            _appointmentsRepository = appointmentsRepository;
            _usersRepository = usersRepository;
        }

        // --- CRUD Base ---
        public async Task<AppointmentsDTO> GetByIdAsync(int id)
        {
            var appointment = await _appointmentsRepository.GetByIdAsync(id);
            return appointment != null ? MapToDto(appointment) : throw new KeyNotFoundException("Appointment not found.");
        }

        public async Task<IEnumerable<AppointmentsDTO>> GetAllAsync()
        {
            var appointments = await _appointmentsRepository.GetAllAsync();
            return appointments.Select(MapToDto);
        }

        // --- Métodos de Negocio ---

        // asignarTurno
        public async Task<AppointmentsDTO> AssignAppointmentAsync(CreationAppointmentDTO dto)
        {
            // Lógica de validación (ej: el profesional está disponible?)

            var appointment = new Appointments
            {
                PatientId = dto.PatientId,
                ProfessionalId = dto.ProfessionalId,
                Date = dto.Date,
                Description = dto.Description,
                Status = dto.Status // Permitimos al Admin definir el estado inicial
            };

            var created = await _appointmentsRepository.CreateAsync(appointment);
            return MapToDto(created);
        }

        // modificarTurno
        public async Task<AppointmentsDTO> UpdateAppointmentAsync(int appointmentId, CreationAppointmentDTO dto)
        {
            var existing = await _appointmentsRepository.GetByIdAsync(appointmentId);
            if (existing == null) throw new KeyNotFoundException("Appointment not found.");

            existing.PatientId = dto.PatientId;
            existing.ProfessionalId = dto.ProfessionalId;
            existing.Date = dto.Date;
            existing.Description = dto.Description;
            existing.Status = dto.Status;

            await _appointmentsRepository.UpdateAsync(existing);
            return MapToDto(existing);
        }

        // cancelarTurno
        public async Task CancelAppointmentAsync(int appointmentId)
        {
            var existing = await _appointmentsRepository.GetByIdAsync(appointmentId);
            if (existing == null) throw new KeyNotFoundException("Appointment not found.");

            // Asumimos que esta cancelación es administrativa, o la lógica se maneja en Patients/ProfessionalsService
            existing.Status = AppointmentStatus.CancelledByProfessional;

            await _appointmentsRepository.UpdateAsync(existing);
        }

        // --- Método de Mapeo Interno ---
        private AppointmentsDTO MapToDto(Appointments appointment)
        {
            // En una implementación real, aquí buscarías los nombres del paciente y profesional
            // usando _usersRepository para enriquecer el DTO.
            return new AppointmentsDTO
            {
                Id = appointment.Id,
                Date = appointment.Date,
                Description = appointment.Description,
                Status = appointment.Status,
                PatientId = appointment.PatientId,
                ProfessionalId = appointment.ProfessionalId,
                PatientName = $"Patient #{appointment.PatientId}", // Simulación
                ProfessionalName = $"Prof #{appointment.ProfessionalId}" // Simulación
            };
        }
    }
}
