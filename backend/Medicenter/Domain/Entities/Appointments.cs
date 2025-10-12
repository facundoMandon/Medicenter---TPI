using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Appointments
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } // Fecha
        public TimeSpan Time { get; set; } // Hora
        public string Description { get; set; }

        // Claves Foráneas (FK)
        public int ProfessionalId { get; set; }
        public int PatientId { get; set; }

        // Propiedades de Navegación de Referencia (N:1)
        public Professionals Professional { get; set; }
        public Patients Patient { get; set; }

        // Métodos del diagrama
        public void AssignProfessional() { /* Lógica */ }
        public void CancelAppointment() { /* Lógica */ } //posible bool
        public void ConfirmAppointment() { /* Lógica */ } //posible bool
        public void ModifyAppointment() { /* Lógica */ }
    }
}