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
        [Required]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Nombre { get; set; } = string.Empty; // Nombre: string

        public string Descripcion { get; set; } = string.Empty; // Descripción: text

        [Required]
        public MedicalCoverageType TipoCobertura { get; set; } // enum(común, media, premium)

        // Relación 1:N con Patients (Afiliados)
        public ICollection<Patients> Patients { get; set; } = new List<Patients>();

        // Relación N:M con Professionals
        public ICollection<Professionals> Professionals { get; set; } = new List<Professionals>();
    }
}
