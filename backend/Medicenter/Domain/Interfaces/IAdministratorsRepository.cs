using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IAdministratorsRepository : IRepositoryBase<Administrators>
    {
        Task<Administrators> DeleteUsersAsync(int userId);
        Task<Administrators> DeleteSpecialtyAsync(int specialtyId);
        Task<Administrators> DeleteAppointmentAsync(int appointmentId);
        Task<Administrators> UpdateUsersAsync(Users user);
        Task<List<Administrators>> GetAllProffesionalsAsync();
    }
}