using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ISpecialtiesRepository : IRepositoryBase<Specialties>
    {
        // No necesita métodos adicionales porque la relación se maneja en ProfessionalsRepository
    }
}
