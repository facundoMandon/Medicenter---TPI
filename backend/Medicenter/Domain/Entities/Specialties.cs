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
        public string Type { get; set; } = string.Empty; // CORREGIDO: Type en vez de Name

        public string Description { get; set; } = string.Empty; // CORREGIDO: Description

        // Relación 1:N con Professional
        public ICollection<Professional> Professional { get; set; } = new List<Professional>();
    }
}
