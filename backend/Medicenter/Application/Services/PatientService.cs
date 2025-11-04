using Application.Interfaces;
using Application.Models;
using Application.Models.Request;
using Domain.Entities;
using Domain.Interfaces;

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
            // ✅ Obtener todos los pacientes existentes
            var existingPatient = await _patientsRepository.GetAllAsync();

            // Normalizar email para comparar sin mayúsculas ni espacios
            string emailNormalized = dto.Email.Trim().ToLower();

            // Verificar duplicados por DNI o Email
            bool exists = existingPatient.Any(p =>
                p.DNI == dto.DNI ||
                p.Email.Trim().ToLower() == emailNormalized
            );

            if (exists)
                throw new ArgumentException($"Ya existe un paciente registrado con el mismo DNI ({dto.DNI}) o correo electrónico ({dto.Email}).");

            // Crear nuevo paciente si no hay duplicado
            var patient = new Patient
            {
                Name = dto.Name,
                LastName = dto.LastName,
                DNI = dto.DNI,
                Email = dto.Email,
                Password = dto.Password, // ⚠️ sin hash (deberías encriptar luego)
                Rol = dto.Rol,
                AffiliateNumber = dto.AffiliateNumber,
                InsuranceId = dto.InsuranceId
            };

            var created = await _patientsRepository.CreateAsync(patient);

            var insurance = await _insuranceRepository.GetByIdAsync(dto.InsuranceId);
            string insuranceName = insurance?.Name ?? "";

            return PatientDTO.FromEntity(created, insuranceName);
        }

        public async Task<IEnumerable<AppointmentDTO>> ViewAppointmentAsync(int patientId)
        {
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
            var appointment = await _appointmentsRepository.GetByIdAsync(appointmentId);

            if (appointment == null)
                throw new KeyNotFoundException($"Appointment with ID {appointmentId} not found.");

            if (appointment.PatientId != patientId)
                throw new UnauthorizedAccessException("This appointment does not belong to the patient.");

            appointment.Status = Domain.Enums.AppointmentStatus.Cancelled;
            await _appointmentsRepository.UpdateAsync(appointment);
        }

        public async Task<AppointmentDTO> RequestAppointmentAsync(int patientId, AppointmentRequestDTO request)
        {
            var professional = await _professionalsRepository.GetByIdAsync(request.ProfessionalId);
            if (professional == null)
                throw new KeyNotFoundException($"Professional with ID {request.ProfessionalId} not found.");

            // Validar fecha (esto se hace en AppointmentService si llamas a ese servicio)
            // O puedes validar aquí también

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

            var appointments = await _appointmentsRepository.GetByPatientIdAsync(patientId);
            var appointmentWithData = appointments.FirstOrDefault(a => a.Id == created.Id);

            if (appointmentWithData == null)
                throw new KeyNotFoundException("Could not retrieve created appointment.");

            return AppointmentDTO.FromEntity(
                appointmentWithData,
                $"{appointmentWithData.Patient.Name} {appointmentWithData.Patient.LastName}",
                $"{appointmentWithData.Professional.Name} {appointmentWithData.Professional.LastName}",
                appointmentWithData.Professional.Specialty?.Type ?? ""
            );
        }
    }
}