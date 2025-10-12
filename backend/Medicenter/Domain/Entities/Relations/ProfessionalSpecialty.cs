using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Relations
{
    public class ProfessionalSpecialty
    {
        public int ProfessionalId { get; set; }
        public int SpecialtyId { get; set; }

        // Propiedades de Navegación de Referencia
        public Professionals Professional { get; set; }
        public Specialties Specialty { get; set; }
    }
}

