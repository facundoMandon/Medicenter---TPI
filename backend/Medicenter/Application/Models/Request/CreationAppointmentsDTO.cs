using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Request
{
    public class CreationAppointmentDTO
    {
        [Required]
        public int PatientId { get; set; }

        [Required]
        public int ProfessionalId { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public string Hora { get; set; } = string.Empty; // Ej: "14:30"

        public string Descripcion { get; set; } = string.Empty;

        // El estado inicial se define en el servicio, no en el DTO de entrada
    }
}
