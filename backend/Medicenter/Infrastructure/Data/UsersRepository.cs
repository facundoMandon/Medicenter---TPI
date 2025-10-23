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
    public class UsersRepository : RepositoryBase<Users>, IUsersRepository
    {
        public UsersRepository(ApplicationDbContext context) : base(context) { }

        public async Task<Users?> GetByEmailAsync(string email)
        {
            // Usa _context de RepositoryBase
            return await _context.Set<Users>().FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
