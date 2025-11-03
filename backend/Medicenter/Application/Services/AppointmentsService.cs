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
    public class AppointmentsService : IAppointmentsService
    {
        private readonly IAppointmentsRepository _appointmentsRepository;
        private readonly IPatientsRepository _patientsRepository;
        private readonly IProfessionalsRepository _professionalsRepository;
        private readonly IHolidaysService _holidaysService;

        public AppointmentsService(
            IAppointmentsRepository appointmentsRepository,
            IPatientsRepository patientsRepository,
            IProfessionalsRepository professionalsRepository,
            IHolidaysService holidaysService)
        {
            _appointmentsRepository = appointmentsRepository;
            _patientsRepository = patientsRepository;
            _professionalsRepository = professionalsRepository;
            _holidaysService = holidaysService;
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

            // Validar fecha y verificar feriados
            await ValidateDateAndHolidaysAsync(dto.Year, dto.Month, dto.Day);

            var appointment = new Appointments
            {
                PatientId = dto.PatientId,
                ProfessionalId = dto.ProfessionalId,
                Year = dto.Year.PadLeft(4, '0'),
                Month = dto.Month.PadLeft(2, '0'),
                Day = dto.Day.PadLeft(2, '0'),
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

            // Validar fecha y verificar feriados
            await ValidateDateAndHolidaysAsync(dto.Year, dto.Month, dto.Day);

            existing.PatientId = dto.PatientId;
            existing.ProfessionalId = dto.ProfessionalId;
            existing.Year = dto.Year.PadLeft(4, '0');
            existing.Month = dto.Month.PadLeft(2, '0');
            existing.Day = dto.Day.PadLeft(2, '0');
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
                specialtyName = professional.Specialty?.Tipo ?? "";
            }

            return AppointmentsDTO.FromEntity(appointment, patientName, professionalName, specialtyName);
        }
    }
}