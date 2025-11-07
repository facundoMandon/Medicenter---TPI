using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IProfessionalRepository : IRepositoryBase<Professional>
    {
        Task<Professional?> GetByEmailAsync(string email);
        Task<Professional?> GetByDniAsync(int dni);

    }
}
