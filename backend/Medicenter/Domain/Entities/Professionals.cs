using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Entities
{
    public class Professionals : Users
    {
        // Atributo mapeado del diagrama: n_matrícula -> LicenseNumber
        public int LicenseNumber { get; set; }

        // Relaciones (para completitud del modelo, asumiendo su existencia)
        public int SpecialtyId { get; set; }
        public Specialties Specialty { get; set; } = null!;
        public ICollection<Appointments> Appointments { get; set; } = new List<Appointments>();
        public ICollection<Hospitals> Hospitals { get; set; } = new List<Hospitals>();
        public ICollection<Insurance> Insurances { get; set; } = new List<Insurance>();
    }
}
