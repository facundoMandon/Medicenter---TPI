using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Models
{
    public class AppointmentsDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public string Description { get; set; }
        public int ProfessionalId { get; set; }
        public int PatientId { get; set; }


        public static AppointmentsDTO FromEntity(Appointments appointment)
        {
            return new AppointmentsDTO
            {
                Id = appointment.Id,
                Date = appointment.Date,
                Time = appointment.Time,
                Description = appointment.Description,
                ProfessionalId = appointment.ProfessionalId,
                PatientId = appointment.PatientId,
            };
        }
    }
}