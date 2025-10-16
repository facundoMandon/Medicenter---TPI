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
        // Método de negocio (ej: para asignar la especialidad a un profesional)
        Task AssignToProfessionalAsync(int specialtyId, int professionalId);
        Task RemoveFromProfessionalAsync(int specialtyId, int professionalId);
    }
}
