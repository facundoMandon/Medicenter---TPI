using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Hospitals
    {
        [Required]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Nombre { get; set; } = string.Empty; // CORREGIDO: Nombre

        [Required, MaxLength(250)]
        public string Direccion { get; set; } = string.Empty; // CORREGIDO: Direccion

        // Relación N:M con Professionals
        public ICollection<Professionals> Professionals { get; set; } = new List<Professionals>();
    }
}
