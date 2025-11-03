using Domain.Entities;
using Domain.Enums;

namespace Application.Models
{
    public class AppointmentsDTO
    {
        public int Id { get; set; }
        public string Year { get; set; } = string.Empty;
        public string Month { get; set; } = string.Empty;
        public string Day { get; set; } = string.Empty;
        public string Hora { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public AppointmentStatus Status { get; set; }

        public int PatientId { get; set; }
        public int ProfessionalId { get; set; }

        // Información adicional para mostrar
        public string PatientName { get; set; } = string.Empty;
        public string ProfessionalName { get; set; } = string.Empty;
        public string SpecialtyName { get; set; } = string.Empty;

        // Fecha formateada para mostrar (opcional)
        public string FormattedDate => $"{Day}/{Month}/{Year}";

        // Método FromEntity para mapear
        public static AppointmentsDTO FromEntity(Appointments appointment, string patientName = "", string professionalName = "", string specialtyName = "")
        {
            return new AppointmentsDTO
            {
                Id = appointment.Id,
                Year = appointment.Year,
                Month = appointment.Month,
                Day = appointment.Day,
                Hora = appointment.Hora,
                Descripcion = appointment.Descripcion,
                Status = appointment.Status,
                PatientId = appointment.PatientId,
                ProfessionalId = appointment.ProfessionalId,
                PatientName = patientName,
                ProfessionalName = professionalName,
                SpecialtyName = specialtyName
            };
        }
    }
}