using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class SpecialtiesRepository : RepositoryBase<Specialties>, ISpecialtiesRepository
    {
        public SpecialtiesRepository(ApplicationDbContext context) : base(context) { }

        // No necesita métodos adicionales
    }
}
