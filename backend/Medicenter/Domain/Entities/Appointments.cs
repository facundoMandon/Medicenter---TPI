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
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime Fecha { get; set; } // Fecha del turno (solo fecha)

        [Required]
        public string Hora { get; set; } = string.Empty; // Hora como string (ej: "14:30")

        public string Descripcion { get; set; } = string.Empty; // Descripción del turno

        // Estado del Turno
        [Required]
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Requested;

        // Relaciones (Foreign Keys)
        [Required]
        public int PatientId { get; set; }
        public Patients Patient { get; set; } = null!;

        [Required]
        public int ProfessionalId { get; set; }
        public Professionals Professional { get; set; } = null!;
    }
}
