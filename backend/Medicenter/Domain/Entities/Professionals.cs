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
        public int n_matricula { get; set; }

        //FKs
        public int SpecialtyId { get; set; }

        // Propiedades de navegación
        public Specialties Specialty { get; set; }

        public ICollection<Appointments> Appointments { get; set; } = new List<Appointments>();
        public ICollection<Hospitals> Hospitals { get; set; } = new HashSet<Hospitals>();
        public ICollection<Insurance> Insurances { get; set; } = new List<Insurance>();
    }
}
