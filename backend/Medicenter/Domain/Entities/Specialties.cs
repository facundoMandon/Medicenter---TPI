using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Specialties
    {
        [Required]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Tipo { get; set; } = string.Empty; // CORREGIDO: Tipo en vez de Name

        public string Descripcion { get; set; } = string.Empty; // CORREGIDO: Descripcion

        // Relación 1:N con Professionals
        public ICollection<Professionals> Professionals { get; set; } = new List<Professionals>();
    }
}
