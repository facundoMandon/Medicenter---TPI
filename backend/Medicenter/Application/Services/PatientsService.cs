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
    public class PatientsService : IPatientsService
    {
        private readonly IPatientsRepository _patientsRepository;
        // Repositorios auxiliares requeridos para la lógica de negocio del paciente
        private readonly IAppointmentsRepository _appointmentsRepository;
        private readonly IInsuranceRepository _insuranceRepository;

        public PatientsService(
            IPatientsRepository patientsRepository,
            IAppointmentsRepository appointmentsRepository,
            IInsuranceRepository insuranceRepository
        )
        {
            _patientsRepository = patientsRepository;
            _appointmentsRepository = appointmentsRepository;
            _insuranceRepository = insuranceRepository;
        }

        // 1. Creación de Paciente (crearUsuario)
        public async Task<PatientsDTO> CreatePatientAsync(CreationPatientsDTO dto)
        {
            var patient = new Patients
            {
                Name = dto.Name,
                LastName = dto.LastName,
                DNI = dto.DNI,
                Email = dto.Email,
                Password = dto.Password,
                Rol = dto.rol,
                AffiliateNumber = dto.AffiliateNumber, // n_Afiliado
                InsuranceId = dto.InsuranceId // FK a Obra Social
            };

            var created = await _patientsRepository.CreateAsync(patient);

            // Simulación de búsqueda de nombre de obra social para el DTO de salida
            string insuranceName = "Insurance Name (lookup simulated)";

            var patientDto = new PatientsDTO
            {
                Id = created.Id,
                Name = created.Name,
                LastName = created.LastName,
                DNI = created.DNI,
                Email = created.Email,
                Rol = created.Rol,
                AffiliateNumber = created.AffiliateNumber,
                InsuranceName = insuranceName
            };
            return patientDto;
        }

        // 2. verTurnos (ViewAppointmentsAsync)
        public Task<IEnumerable<AppointmentsDTO>> ViewAppointmentsAsync(int patientId)
        {
            // Lógica: await _appointmentsRepository.GetByPatientIdAsync(patientId);
            // Simulación
            var appointments = new List<AppointmentsDTO>
            {
                new AppointmentsDTO { Id = 201 },
                new AppointmentsDTO { Id = 202 }
            };
            return Task.FromResult<IEnumerable<AppointmentsDTO>>(appointments);
        }

        // 3. cancelarTurno (CancelAppointmentAsync)
        public Task CancelAppointmentAsync(int patientId, int appointmentId)
        {
            // Lógica de negocio:
            // 1. Buscar turno por appointmentId.
            // 2. Verificar que el turno pertenece a este patientId.
            // 3. Actualizar el estado del turno (Status = Cancelled).

            // Simulación
            return Task.CompletedTask;
        }

        // 4. pedirTurno (RequestAppointmentAsync)
        public Task<AppointmentsDTO> RequestAppointmentAsync(int patientId, AppointmentRequestDTO request)
        {
            // Lógica de negocio:
            // 1. Verificar disponibilidad del profesional/horario.
            // 2. Crear nueva entidad Appointment (usando patientId, request.ProfessionalId, request.AppointmentDate).
            // 3. Establecer el estado inicial a "Pending" o "Requested".
            // 4. Guardar en el repositorio (_appointmentsRepository.CreateAsync).

            // Simulación: Asumimos la creación exitosa y devolvemos un DTO con ID generado
            var newAppointment = new AppointmentsDTO
            {
                Id = new Random().Next(300, 999),
                // ... campos mapeados de la request
            };
            return Task.FromResult(newAppointment);
        }
    }
}
