using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context) { }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Set<User>()
                                 .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User?> GetByDniAsync(int dni)
        {
            return await _context.Set<User>()
                                 .FirstOrDefaultAsync(u => u.DNI == dni);
        }
    }
}
