using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Models
{
    internal class ProfessionalsDTO
    {
        public int n_matricula { get; set; }
        public int SpecialtyId { get; set; }

        public static ProfessionalsDTO FromEntity(Professionals professional)
        {
            return new ProfessionalsDTO
            {
                n_matricula = professional.n_matricula,
                SpecialtyId = professional.SpecialtyId
            };
        }
    }
}
