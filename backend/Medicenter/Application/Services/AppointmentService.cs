using Application.Interfaces;
using Application.Models;
using Application.Models.Request;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Application.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentsRepository;
        private readonly IPatientRepository _patientsRepository;
        private readonly IProfessionalRepository _professionalsRepository;
        private readonly IHolidaysService _holidaysService;

        public AppointmentService(
            IAppointmentRepository appointmentsRepository,
            IPatientRepository patientsRepository,
            IProfessionalRepository professionalsRepository,
            IHolidaysService holidaysService)
        {
            _appointmentsRepository = appointmentsRepository;
            _patientsRepository = patientsRepository;
            _professionalsRepository = professionalsRepository;
            _holidaysService = holidaysService;
        }

        public async Task<AppointmentDTO> GetByIdAsync(int id)
        {
            var appointment = await _appointmentsRepository.GetByIdAsync(id);
            if (appointment == null)
                throw new KeyNotFoundException("Appointment not found.");

            return await MapToDtoAsync(appointment);
        }

        public async Task<IEnumerable<AppointmentDTO>> GetAllAsync()
        {
            var appointments = await _appointmentsRepository.GetAllAsync();
            var dtos = new List<AppointmentDTO>();

            foreach (var appointment in appointments)
            {
                dtos.Add(await MapToDtoAsync(appointment));
            }

            return dtos;
        }

        public async Task<AppointmentDTO> AssignAppointmentAsync(CreationAppointmentDTO dto)
        {
            var patient = await _patientsRepository.GetByIdAsync(dto.PatientId);
            if (patient == null)
                throw new KeyNotFoundException($"Patient with ID {dto.PatientId} not found.");

            var professional = await _professionalsRepository.GetByIdAsync(dto.ProfessionalId);
            if (professional == null)
                throw new KeyNotFoundException($"Professional with ID {dto.ProfessionalId} not found.");

            // Validar fecha y verificar feriados
            await ValidateDateAndHolidaysAsync(dto.Year, dto.Month, dto.Day);

            // ✅ Validar que no haya otro turno en la misma fecha y hora para el mismo profesional
            var existingAppointment = await _appointmentsRepository.GetAllAsync();
            bool alreadyExists = existingAppointment.Any(a =>
                a.ProfessionalId == dto.ProfessionalId &&
                a.Year == dto.Year.PadLeft(4, '0') &&
                a.Month == dto.Month.PadLeft(2, '0') &&
                a.Day == dto.Day.PadLeft(2, '0') &&
                string.Equals(a.Time, dto.Time, StringComparison.OrdinalIgnoreCase) &&
                a.Status != AppointmentStatus.Cancelled // permitir si el turno anterior fue cancelado
            );

            if (alreadyExists)
            {
                throw new ArgumentException($"Ya existe un turno asignado al profesional en la fecha {dto.Day}/{dto.Month}/{dto.Year} a las {dto.Time}.");
            }

            var appointment = new Appointment
            {
                PatientId = dto.PatientId,
                ProfessionalId = dto.ProfessionalId,
                Year = dto.Year.PadLeft(4, '0'),
                Month = dto.Month.PadLeft(2, '0'),
                Day = dto.Day.PadLeft(2, '0'),
                Time = dto.Time,
                Description = dto.Description,
                Status = AppointmentStatus.Requested
            };

            var created = await _appointmentsRepository.CreateAsync(appointment);
            return await MapToDtoAsync(created);
        }

        public async Task<AppointmentDTO> UpdateAppointmentAsync(int appointmentId, CreationAppointmentDTO dto)
        {
            var existing = await _appointmentsRepository.GetByIdAsync(appointmentId);
            if (existing == null)
                throw new KeyNotFoundException("Appointment not found.");

            // Validar fecha y verificar feriados
            await ValidateDateAndHolidaysAsync(dto.Year, dto.Month, dto.Day);

            existing.PatientId = dto.PatientId;
            existing.ProfessionalId = dto.ProfessionalId;
            existing.Year = dto.Year.PadLeft(4, '0');
            existing.Month = dto.Month.PadLeft(2, '0');
            existing.Day = dto.Day.PadLeft(2, '0');
            existing.Time = dto.Time;
            existing.Description = dto.Description;

            await _appointmentsRepository.UpdateAsync(existing);
            return await MapToDtoAsync(existing);
        }

        public async Task<AppointmentDTO> ConfirmAppointmentAsync(int appointmentId)
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

        /// <summary>
        /// Valida que la fecha sea válida y no sea feriado en Argentina
        /// Lanza ArgumentException (400 Bad Request) si es feriado
        /// </summary>
        private async Task ValidateDateAndHolidaysAsync(string year, string month, string day)
        {
            // Normalizar mes y día para consulta (agregar 0 si es necesario)
            string monthNormalized = month.PadLeft(2, '0');
            string dayNormalized = day.PadLeft(2, '0');

            // Validar que la fecha sea válida
            try
            {
                var date = new DateTime(int.Parse(year), int.Parse(monthNormalized), int.Parse(dayNormalized));

                // Validar que la fecha no sea en el pasado
                if (date.Date < DateTime.Now.Date)
                {
                    throw new ArgumentException("No se pueden crear turnos en fechas pasadas");
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new ArgumentException($"La fecha {day}/{month}/{year} no es válida");
            }

            // Consultar feriados de Argentina
            var holidaysJson = await _holidaysService.GetHolidaysAsync("AR", year, monthNormalized, dayNormalized);

            // Verificar si hay feriados
            if (!string.IsNullOrWhiteSpace(holidaysJson) &&
                holidaysJson != "{}" &&
                holidaysJson != "[]" &&
                !holidaysJson.Contains("error"))
            {
                // Intentar parsear para verificar que sea un array con elementos
                try
                {
                    var holidays = JsonSerializer.Deserialize<JsonElement>(holidaysJson);

                    if (holidays.ValueKind == JsonValueKind.Array && holidays.GetArrayLength() > 0)
                    {
                        // Es un feriado - lanzar ArgumentException para que retorne 400
                        var holidayName = holidays[0].GetProperty("name").GetString();
                        throw new ArgumentException(
                            $"La fecha {dayNormalized}/{monthNormalized}/{year} es feriado en Argentina ({holidayName}). No se pueden asignar turnos en días feriados."
                        );
                    }
                }
                catch (JsonException)
                {
                    // Si no se puede parsear, continuar (asumir que no es feriado)
                }
            }
        }

        private async Task<AppointmentDTO> MapToDtoAsync(Appointment appointment)
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
                specialtyName = professional.Specialty?.Type ?? "";
            }

            return AppointmentDTO.FromEntity(appointment, patientName, professionalName, specialtyName);
        }
    }
}