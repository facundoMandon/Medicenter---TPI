using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Patients : Users
    {
        public int affiliate_number { get; set; }

        //FKs

        public int InsuranceId { get; set; }

        //Propiedad de Navegación

        public Insurance Insurance { get; set; }

        //Relaciones

        public ICollection<Appointments> Appointments { get; set; }

        //Métodos
    }
}
