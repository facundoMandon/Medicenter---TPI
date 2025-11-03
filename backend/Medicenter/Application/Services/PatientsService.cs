using Application.Interfaces;
using Application.Models;
using Application.Models.Request;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services
{
    public class PatientsService : IPatientsService
    {
        private readonly IPatientsRepository _patientsRepository;
        private readonly IAppointmentsRepository _appointmentsRepository;
        private readonly IInsuranceRepository _insuranceRepository;
        private readonly IProfessionalsRepository _professionalsRepository;

        public PatientsService(
            IPatientsRepository patientsRepository,
            IAppointmentsRepository appointmentsRepository,
            IInsuranceRepository insuranceRepository,
            IProfessionalsRepository professionalsRepository)
        {
            _patientsRepository = patientsRepository;
            _appointmentsRepository = appointmentsRepository;
            _insuranceRepository = insuranceRepository;
            _professionalsRepository = professionalsRepository;
        }

        public async Task<PatientsDTO> CreatePatientAsync(CreationPatientsDTO dto)
        {
            var patient = new Patients
            {
                Name = dto.Name,
                LastName = dto.LastName,
                DNI = dto.DNI,
                Email = dto.Email,
                Password = dto.Password, // Sin hash
                Rol = dto.Rol,
                AffiliateNumber = dto.AffiliateNumber,
                InsuranceId = dto.InsuranceId
            };

            var created = await _patientsRepository.CreateAsync(patient);

            var insurance = await _insuranceRepository.GetByIdAsync(dto.InsuranceId);
            string insuranceName = insurance?.Nombre ?? "";

            return PatientsDTO.FromEntity(created, insuranceName);
        }

        public async Task<IEnumerable<AppointmentsDTO>> ViewAppointmentsAsync(int patientId)
        {
            var appointments = await _appointmentsRepository.GetByPatientIdAsync(patientId);

            return appointments.Select(a => AppointmentsDTO.FromEntity(
                a,
                $"{a.Patient.Name} {a.Patient.LastName}",
                $"{a.Professional.Name} {a.Professional.LastName}",
                a.Professional.Specialty?.Tipo ?? ""
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

        public async Task<AppointmentsDTO> RequestAppointmentAsync(int patientId, AppointmentRequestDTO request)
        {
            var professional = await _professionalsRepository.GetByIdAsync(request.ProfessionalId);
            if (professional == null)
                throw new KeyNotFoundException($"Professional with ID {request.ProfessionalId} not found.");

            // Validar fecha (esto se hace en AppointmentsService si llamas a ese servicio)
            // O puedes validar aquí también

            var appointment = new Appointments
            {
                PatientId = patientId,
                ProfessionalId = request.ProfessionalId,
                Year = request.Year.PadLeft(4, '0'),
                Month = request.Month.PadLeft(2, '0'),
                Day = request.Day.PadLeft(2, '0'),
                Hora = request.Hora,
                Descripcion = request.Descripcion,
                Status = Domain.Enums.AppointmentStatus.Requested
            };

            var created = await _appointmentsRepository.CreateAsync(appointment);

            var appointments = await _appointmentsRepository.GetByPatientIdAsync(patientId);
            var appointmentWithData = appointments.FirstOrDefault(a => a.Id == created.Id);

            if (appointmentWithData == null)
                throw new KeyNotFoundException("Could not retrieve created appointment.");

            return AppointmentsDTO.FromEntity(
                appointmentWithData,
                $"{appointmentWithData.Patient.Name} {appointmentWithData.Patient.LastName}",
                $"{appointmentWithData.Professional.Name} {appointmentWithData.Professional.LastName}",
                appointmentWithData.Professional.Specialty?.Tipo ?? ""
            );
        }
    }
}