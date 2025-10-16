using Domain.Entities;
using Domain.Interfaces;
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

        // Implementación de métodos de negocio (si Specialties tuviera lógica específica aquí)

        // Asumo que la relación es 1:N (Professional tiene FK a Specialty) y se gestiona 
        // a través de ProfessionalsRepository, por lo que estas implementaciones quedan simples.
        public Task AssignToProfessionalAsync(int specialtyId, int professionalId)
        {
            // La lógica pesada se manejó en el Service. Aquí solo se retorna.
            return Task.CompletedTask;
        }

        public Task RemoveFromProfessionalAsync(int specialtyId, int professionalId)
        {
            return Task.CompletedTask;
        }
    }
}
