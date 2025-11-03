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
        [StringLength(4)] // Año: "2025"
        public string Year { get; set; } = string.Empty;

        [Required]
        [StringLength(2)] // Mes: "01", "02", ..., "12"
        public string Month { get; set; } = string.Empty;

        [Required]
        [StringLength(2)] // Día: "01", "02", ..., "31"
        public string Day { get; set; } = string.Empty;

        [Required]
        [StringLength(5)] // Hora: "14:30"
        public string Hora { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;

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

        // Helper para obtener fecha completa (opcional, útil para validaciones)
        public DateTime GetFullDate()
        {
            return new DateTime(int.Parse(Year), int.Parse(Month), int.Parse(Day));
        }
    }
}