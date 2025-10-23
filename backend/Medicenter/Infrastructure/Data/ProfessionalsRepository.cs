using Domain.Entities;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class ProfessionalsRepository : RepositoryBase<Professionals>, IProfessionalsRepository
    {
        public ProfessionalsRepository(ApplicationDbContext context) : base(context) { }
    }
}
