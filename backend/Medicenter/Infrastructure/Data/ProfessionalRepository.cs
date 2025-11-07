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
    public class ProfessionalRepository : RepositoryBase<Professional>, IProfessionalRepository
    {
        public ProfessionalRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Professional?> GetByEmailAsync(string email)
        {
            return await _context.Set<Professional>()
                .FirstOrDefaultAsync(p => p.Email.ToLower() == email.ToLower());
        }

        public async Task<Professional?> GetByDniAsync(int dni)
        {
            return await _context.Set<Professional>()
                .FirstOrDefaultAsync(p => p.DNI == dni);
        }
    }
}
