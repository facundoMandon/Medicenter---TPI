using Application.Models;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class AppointmentsDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } = string.Empty;
        public AppointmentStatus Status { get; set; }

        public int PatientId { get; set; }
        public int ProfessionalId { get; set; }

        // Opcional: para mostrar información en lugar de solo IDs
        public string PatientName { get; set; } = string.Empty;
        public string ProfessionalName { get; set; } = string.Empty;
    }
}