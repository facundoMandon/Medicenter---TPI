using Application.Models;
using Application.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ISpecialtiesService
    {
        // CRUD Básico (Añadir, Modificar, Eliminar)
        Task<SpecialtiesDTO> GetByIdAsync(int id);
        Task<IEnumerable<SpecialtiesDTO>> GetAllAsync();
        Task<SpecialtiesDTO> CreateSpecialtyAsync(CreationSpecialtiesDTO dto); // añadirEspecialidad
        Task<SpecialtiesDTO> UpdateSpecialtyAsync(int id, CreationSpecialtiesDTO dto); // modificarEspecialidad
        Task DeleteSpecialtyAsync(int id); // eliminarEspecialidad

        // Métodos de Relación
        Task AssignSpecialtyToProfessionalAsync(int specialtyId, int professionalId); // asignarEspecialidad
        Task RemoveSpecialtyFromProfessionalAsync(int specialtyId, int professionalId); // quitarEspecialidad
    }
}
