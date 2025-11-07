using Application.Interfaces;
using Application.Models;
using Application.Models.Request;
using Domain.Entities;
using Domain.Enums;
using Domain.Exceptions;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
                throw new NotFoundException($"Turno con ID {id} no encontrado.");

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
            // Verificar que el paciente existe
            var patient = await _patientsRepository.GetByIdAsync(dto.PatientId);
            if (patient == null)
                throw new NotFoundException($"Paciente con ID {dto.PatientId} no encontrado.");

            // Verificar que el profesional existe
            var professional = await _professionalsRepository.GetByIdAsync(dto.ProfessionalId);
            if (professional == null)
                throw new NotFoundException($"Profesional con ID {dto.ProfessionalId} no encontrado.");

            // Validaciones de datos de entrada
            if (string.IsNullOrWhiteSpace(dto.Year) || dto.Year.Length != 4)
                throw new ValidationException("El año debe tener 4 dígitos.");

            if (string.IsNullOrWhiteSpace(dto.Month) || dto.Month.Length > 2)
                throw new ValidationException("El mes debe tener 1 o 2 dígitos.");

            if (string.IsNullOrWhiteSpace(dto.Day) || dto.Day.Length > 2)
                throw new ValidationException("El día debe tener 1 o 2 dígitos.");

            if (string.IsNullOrWhiteSpace(dto.Time))
                throw new ValidationException("La hora es requerida.");

            // Validar fecha y feriados
            await ValidateDateAndHolidaysAsync(dto.Year, dto.Month, dto.Day);

            // Verificar si ya existe un turno asignado al mismo profesional, fecha y hora
            var existingAppointments = await _appointmentsRepository.GetAllAsync();
            bool isDuplicate = existingAppointments.Any(a =>
                a.ProfessionalId == dto.ProfessionalId &&
                a.Year == dto.Year.PadLeft(4, '0') &&
                a.Month == dto.Month.PadLeft(2, '0') &&
                a.Day == dto.Day.PadLeft(2, '0') &&
                a.Time == dto.Time &&
                a.Status != Domain.Enums.AppointmentStatus.Cancelled
            );

            if (isDuplicate)
                throw new DuplicateException($"Ya existe un turno asignado al profesional en la fecha {dto.Day}/{dto.Month}/{dto.Year} a las {dto.Time}.");

            // Crear el turno
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

            var created = await _appointmentsRepository.AddAsync(appointment);
            return await MapToDtoAsync(created);
        }

        public async Task<AppointmentDTO> UpdateAppointmentAsync(int appointmentId, CreationAppointmentDTO dto)
        {
            // Verificar que el turno existe
            var existing = await _appointmentsRepository.GetByIdAsync(appointmentId);
            if (existing == null)
                throw new NotFoundException($"Turno con ID {appointmentId} no encontrado.");

            // Verificar que el turno no se cree por duplicado (no modificar un turno y que se solape con uno ya creado)
            var allAppointments = await _appointmentsRepository.GetAllAsync();
            bool duplicateExists = allAppointments.Any(a =>
                a.Id != appointmentId &&
                a.ProfessionalId == dto.ProfessionalId &&
                a.Year == dto.Year.PadLeft(4, '0') &&
                a.Month == dto.Month.PadLeft(2, '0') &&
                a.Day == dto.Day.PadLeft(2, '0') &&
                a.Time == dto.Time &&
                a.Status != Domain.Enums.AppointmentStatus.Cancelled
            );

            if (duplicateExists)
                throw new DuplicateException($"El profesional ya tiene un turno en la fecha {dto.Day}/{dto.Month}/{dto.Year} a las {dto.Time}.");

            // Validaciones de datos de entrada
            if (string.IsNullOrWhiteSpace(dto.Year) || dto.Year.Length != 4)
                throw new ValidationException("El año debe tener 4 dígitos.");

            if (string.IsNullOrWhiteSpace(dto.Month) || dto.Month.Length > 2)
                throw new ValidationException("El mes debe tener 1 o 2 dígitos.");

            if (string.IsNullOrWhiteSpace(dto.Day) || dto.Day.Length > 2)
                throw new ValidationException("El día debe tener 1 o 2 dígitos.");

            if (string.IsNullOrWhiteSpace(dto.Time))
                throw new ValidationException("La hora es requerida.");

            // Validar fecha y verificar feriados
            await ValidateDateAndHolidaysAsync(dto.Year, dto.Month, dto.Day);

            // Actualizar el turno
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
            // Verificar que el turno existe
            var existing = await _appointmentsRepository.GetByIdAsync(appointmentId);
            if (existing == null)
                throw new NotFoundException($"Turno con ID {appointmentId} no encontrado.");

            // Confirmar el turno
            existing.Status = AppointmentStatus.Confirmed;
            await _appointmentsRepository.UpdateAsync(existing);

            return await MapToDtoAsync(existing);
        }

        public async Task CancelAppointmentAsync(int appointmentId)
        {
            // Verificar que el turno existe
            var existing = await _appointmentsRepository.GetByIdAsync(appointmentId);
            if (existing == null)
                throw new NotFoundException($"Turno con ID {appointmentId} no encontrado.");

            // Cancelar el turno
            existing.Status = AppointmentStatus.Cancelled;
            await _appointmentsRepository.UpdateAsync(existing);
        }

        /// Valida que la fecha sea válida y no sea feriado en Argentina
        /// Lanza ValidationException si la fecha es inválida, pasada o es feriado
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
                    throw new ValidationException("No se pueden crear turnos en fechas pasadas.");
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new ValidationException($"La fecha {day}/{month}/{year} no es válida.");
            }
            catch (FormatException)
            {
                throw new ValidationException($"La fecha {day}/{month}/{year} tiene un formato inválido.");
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
                        // Es un feriado - lanzar ValidationException
                        var holidayName = holidays[0].GetProperty("name").GetString();
                        throw new ValidationException(
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

        // Método auxiliar para mapear entidad a DTO
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