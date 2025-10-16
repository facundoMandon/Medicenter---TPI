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
        [Required] public int Id { get; set; }

        // Mapeado de Tipo: string (nombre de la especialidad)
        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        // Mapeado de Descripción: string
        public string Description { get; set; } = string.Empty;

        // Relaciones (opcional: lista de profesionales con esta especialidad)
        public ICollection<Professionals> Professionals { get; set; } = new List<Professionals>();
    }
}