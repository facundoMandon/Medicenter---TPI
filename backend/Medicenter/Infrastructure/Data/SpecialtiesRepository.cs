using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Specialties?> GetByNameAsync(string type)
        {
            return await _context.Set<Specialties>()
                .FirstOrDefaultAsync(s => s.Type.ToLower() == type.ToLower());
        }
    }
}
