using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Patients : Users
    {
        // Atributo mapeado del diagrama: n_Afiliado -> AffiliateNumber
        public int AffiliateNumber { get; set; }

        // Relaciones (asumiendo Obra Social para completitud)
        public int InsuranceId { get; set; }
        public Insurance Insurance { get; set; } = null!;
        public ICollection<Appointments> Appointments { get; set; } = new List<Appointments>();
    }
}
