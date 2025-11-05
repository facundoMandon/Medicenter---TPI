using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Entities
{
    public class Professional : User
    {
        // Atributo mapeado del diagrama: n_matrícula -> LicenseNumber
        public int LicenseNumber { get; set; }

        // Relaciones (para completitud del modelo, asumiendo su existencia)
        public int SpecialtyId { get; set; }
        public Specialties Specialty { get; set; } = null!;
        public ICollection<Appointment> Appointment { get; set; } = new List<Appointment>();
        public ICollection<Hospital> Hospital { get; set; } = new List<Hospital>();
        public ICollection<Insurance> Insurance { get; set; } = new List<Insurance>();
    }
}
