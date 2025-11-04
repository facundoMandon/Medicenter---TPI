using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Hospital
    {
        [Required]
        public int Id { get; set; }

        [Required, MaxLength(150)]
        public string Name { get; set; } = string.Empty; // CORREGIDO: Name

        [Required, MaxLength(250)]
        public string Adress { get; set; } = string.Empty; // CORREGIDO: Adress

        // Relación N:M con Professional
        public ICollection<Professional> Professional { get; set; } = new List<Professional>();
    }
}
