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
        public string Name { get; set; } = string.Empty; // Name: string

        public string Description { get; set; } = string.Empty; // Descripción: text

        [Required]
        public MedicalCoverageType MedicalCoverageType { get; set; } // enum(común, media, premium)

        // Relación 1:N con Patient (Afiliados)
        public ICollection<Patient> Patient { get; set; } = new List<Patient>();

        // Relación N:M con Professional
        public ICollection<Professional> Professional { get; set; } = new List<Professional>();
    }
}
