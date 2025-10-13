using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enums;

namespace Domain.Entities
{
    public class Insurance
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public MedicalCoverageType CoverageType { get; set; }

        //Relaciones

        public ICollection<Professionals> Professionals { get; set; } = new List<Professionals>();
        public ICollection<Patients> Patients { get; set; } = new List<Patients>();
        //Métodos
    }
}
