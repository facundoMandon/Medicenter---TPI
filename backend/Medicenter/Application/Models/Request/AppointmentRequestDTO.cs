using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Request
{
    public class AppointmentRequestDTO
    {
        [Required]
        public int ProfessionalId { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        public string Hora { get; set; } = string.Empty; // Ej: "14:30"

        public string Descripcion { get; set; } = string.Empty;
    }
}
