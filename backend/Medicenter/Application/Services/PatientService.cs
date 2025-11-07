using Application.Interfaces;
using Application.Models;
using Application.Models.Request;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Application.Services
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patientsRepository;
        private readonly IAppointmentRepository _appointmentsRepository;
        private readonly IInsuranceRepository _insuranceRepository;
        private readonly IProfessionalRepository _professionalsRepository;

        public PatientService(
            IPatientRepository patientsRepository,
            IAppointmentRepository appointmentsRepository,
            IInsuranceRepository insuranceRepository,
            IProfessionalRepository professionalsRepository)
        {
            _patientsRepository = patientsRepository;
            _appointmentsRepository = appointmentsRepository;
            _insuranceRepository = insuranceRepository;
            _professionalsRepository = professionalsRepository;
        }

        public async Task<PatientDTO> CreatePatientAsync(CreationPatientDTO dto)
        {
            // Validaciones de datos de entrada
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

            if (dto.AffiliateNumber <= 0)
                throw new ValidationException("El número de afiliado es requerido.");

            if (dto.InsuranceId <= 0)
                throw new ValidationException("Debe especificar un ID de obra social válido.");

            // Verificar duplicados (DNI y Email)
            var existingDni = await _patientsRepository.GetByDniAsync(dto.DNI);
            if (existingDni != null)
                throw new DuplicateException($"Ya existe un paciente con el DNI {dto.DNI}.");

            var existingEmail = await _patientsRepository.GetByEmailAsync(dto.Email);
            if (existingEmail != null)
                throw new DuplicateException($"Ya existe un paciente con el email '{dto.Email}'.");

            // Verificar que la obra social existe
            var insurance = await _insuranceRepository.GetByIdAsync(dto.InsuranceId);
            if (insurance == null)
                throw new NotFoundException($"No existe una obra social con el ID {dto.InsuranceId}.");

            // Crear el paciente
            var patient = new Patient
            {
                Name = dto.Name,
                LastName = dto.LastName,
                DNI = dto.DNI,
                Email = dto.Email,
                Password = dto.Password, // Debería hashearse
                Rol = dto.Rol,
                AffiliateNumber = dto.AffiliateNumber,
                InsuranceId = dto.InsuranceId
            };

            var created = await _patientsRepository.CreateAsync(patient);
            string insuranceName = insurance.Name;

            return PatientDTO.FromEntity(created, insuranceName);
        }

        public async Task<IEnumerable<AppointmentDTO>> ViewAppointmentAsync(int patientId)
        {
            // Verificar que el paciente existe
            var patient = await _patientsRepository.GetByIdAsync(patientId);
            if (patient == null)
                throw new NotFoundException($"Paciente con ID {patientId} no encontrado.");

            // Obtener turnos del paciente
            var appointments = await _appointmentsRepository.GetByPatientIdAsync(patientId);

            return appointments.Select(a => AppointmentDTO.FromEntity(
                a,
                $"{a.Patient.Name} {a.Patient.LastName}",
                $"{a.Professional.Name} {a.Professional.LastName}",
                a.Professional.Specialty?.Type ?? ""
            ));
        }

        public async Task CancelAppointmentAsync(int patientId, int appointmentId)
        {
            // Verificar que el paciente existe
            var patient = await _patientsRepository.GetByIdAsync(patientId);
            if (patient == null)
                throw new NotFoundException($"Paciente con ID {patientId} no encontrado.");

            // Verificar que el turno existe
            var appointment = await _appointmentsRepository.GetByIdAsync(appointmentId);
            if (appointment == null)
                throw new NotFoundException($"Turno con ID {appointmentId} no encontrado.");

            // Verificar que el turno pertenece al paciente
            if (appointment.PatientId != patientId)
                throw new ValidationException("Este turno no pertenece al paciente.");

            // Cancelar el turno
            appointment.Status = Domain.Enums.AppointmentStatus.Cancelled;
            await _appointmentsRepository.UpdateAsync(appointment);
        }

        public async Task<AppointmentDTO> RequestAppointmentAsync(int patientId, AppointmentRequestDTO request)
        {
            // Verificar que el paciente existe
            var patient = await _patientsRepository.GetByIdAsync(patientId);
            if (patient == null)
                throw new NotFoundException($"Paciente con ID {patientId} no encontrado.");

            // Verificar que el profesional existe
            var professional = await _professionalsRepository.GetByIdAsync(request.ProfessionalId);
            if (professional == null)
                throw new NotFoundException($"Profesional con ID {request.ProfessionalId} no encontrado.");

            // Validaciones de datos del turno
            if (string.IsNullOrWhiteSpace(request.Year) || request.Year.Length != 4)
                throw new ValidationException("El año debe tener 4 dígitos.");

            if (string.IsNullOrWhiteSpace(request.Month) || request.Month.Length > 2)
                throw new ValidationException("El mes debe tener 1 o 2 dígitos.");

            if (string.IsNullOrWhiteSpace(request.Day) || request.Day.Length > 2)
                throw new ValidationException("El día debe tener 1 o 2 dígitos.");

            if (string.IsNullOrWhiteSpace(request.Time))
                throw new ValidationException("La hora es requerida.");

            // Validar fecha válida
            if (!int.TryParse(request.Year, out int year) ||
                !int.TryParse(request.Month, out int month) ||
                !int.TryParse(request.Day, out int day))
                throw new ValidationException("La fecha no es válida.");

            if (month < 1 || month > 12)
                throw new ValidationException("El mes debe estar entre 1 y 12.");

            if (day < 1 || day > 31)
                throw new ValidationException("El día debe estar entre 1 y 31.");

            // Crear el turno
            var appointment = new Appointment
            {
                PatientId = patientId,
                ProfessionalId = request.ProfessionalId,
                Year = request.Year.PadLeft(4, '0'),
                Month = request.Month.PadLeft(2, '0'),
                Day = request.Day.PadLeft(2, '0'),
                Time = request.Time,
                Description = request.Description,
                Status = Domain.Enums.AppointmentStatus.Requested
            };

            var created = await _appointmentsRepository.CreateAsync(appointment);

            // Obtener el turno con todos los datos relacionados
            var appointments = await _appointmentsRepository.GetByPatientIdAsync(patientId);
            var appointmentWithData = appointments.FirstOrDefault(a => a.Id == created.Id);

            if (appointmentWithData == null)
                throw new NotFoundException("No se pudo recuperar el turno creado.");

            return AppointmentDTO.FromEntity(
                appointmentWithData,
                $"{appointmentWithData.Patient.Name} {appointmentWithData.Patient.LastName}",
                $"{appointmentWithData.Professional.Name} {appointmentWithData.Professional.LastName}",
                appointmentWithData.Professional.Specialty?.Type ?? ""
            );
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