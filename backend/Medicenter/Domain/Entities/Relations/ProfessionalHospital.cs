using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.Relations
{
    public class ProfessionalHospital
    {
        // Claves Foráneas que forman la Clave Compuesta
        public int ProfessionalId { get; set; }
        public int HospitalId { get; set; }

        // Propiedades de Navegación de Referencia
        public Professionals Professional { get; set; }
        public Hospitals Hospital { get; set; }
    }
}