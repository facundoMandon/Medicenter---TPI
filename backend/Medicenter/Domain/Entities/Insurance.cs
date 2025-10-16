using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Insurance
    {
        [Required] public int Id { get; set; } // ID: int

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty; // Nombre: string

        [Required, MaxLength(50)]
        public string Plan { get; set; } = string.Empty; // Mapea TipoCobertura: enum(común, media, premium)

        public string Description { get; set; } = string.Empty; // Mapea Descripción: text

        // Relación 1:N con Patients (Afiliados). El FK está en Patients.
        public ICollection<Patients> Patients { get; set; } = new List<Patients>();

        // Relación N:M con Professionals (quiénes la aceptan).
        public ICollection<Professionals> Professionals { get; set; } = new List<Professionals>();
    }
}
