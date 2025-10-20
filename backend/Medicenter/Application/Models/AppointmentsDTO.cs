﻿using Application.Models;
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
        public DateTime Fecha { get; set; }
        public string Hora { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public AppointmentStatus Status { get; set; }

        public int PatientId { get; set; }
        public int ProfessionalId { get; set; }

        // Información adicional para mostrar
        public string PatientName { get; set; } = string.Empty;
        public string ProfessionalName { get; set; } = string.Empty;
        public string SpecialtyName { get; set; } = string.Empty;

        // Método FromEntity para mapear
        public static AppointmentsDTO FromEntity(Appointments appointment, string patientName = "", string professionalName = "", string specialtyName = "")
        {
            return new AppointmentsDTO
            {
                Id = appointment.Id,
                Fecha = appointment.Fecha,
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