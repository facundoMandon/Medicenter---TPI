using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Appointments
    {
        [Required] public int Id { get; set; }
        [Required] public DateTime Date { get; set; } // Fecha y Hora del Turno
        public string Description { get; set; } = string.Empty;

        // Estado del Turno
        [Required] public AppointmentStatus Status { get; set; } = AppointmentStatus.Requested;

        // Relaciones (Foreign Keys)
        [Required] public int PatientId { get; set; }
        public Patients Patient { get; set; } = null!;

        [Required] public int ProfessionalId { get; set; }
        public Professionals Professional { get; set; } = null!;
    }
}
