using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IHospitalsRepository : IRepositoryBase<Hospitals>
    {
        Task RegisterProfessionalAsync(int hospitalId, int professionalId); // registrarProfesional
        Task RemoveProfessionalAsync(int hospitalId, int professionalId); // eliminarProfesionales
        Task<List<Professionals>> GetProfessionalsByHospitalIdAsync(int hospitalId); // listarProfesionales
    }
}
