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
        [Required] public int PatientId { get; set; }
        [Required] public int ProfessionalId { get; set; }
        [Required] public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;

        // Opcionalmente, permitir que el Admin o el sistema definan el estado inicial
        [Required] public AppointmentStatus Status { get; set; } = AppointmentStatus.Requested;
    }
}
