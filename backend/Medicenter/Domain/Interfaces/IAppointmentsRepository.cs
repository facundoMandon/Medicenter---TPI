using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IAppointmentsRepository : IRepositoryBase<Appointments>
    {
        Task<List<Appointments>> GetByPatientIdAsync(int patientId);
        Task<List<Appointments>> GetByProfessionalIdAsync(int professionalId);
    }
}
