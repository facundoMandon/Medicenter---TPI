using Application.Models;
using Application.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    // Reemplaza 'HospitalDTO' y 'CreationHospitalDTO' con tus nombres reales de DTOs
    public interface IHospitalsService
    {
        // CRUD
        Task<HospitalsDTO> GetByIdAsync(int id);
        Task<IEnumerable<HospitalsDTO>> GetAllAsync();
        Task<HospitalsDTO> CreateHospitalAsync(CreationHospitalsDTO dto);
        Task<HospitalsDTO> UpdateHospitalAsync(int id, CreationHospitalsDTO dto);
        Task DeleteHospitalAsync(int id);

        // Métodos del diagrama
        Task RegisterProfessionalAsync(int hospitalId, int professionalId); // registrarProfesional
        Task EditProfessionalAsync(int hospitalId, int professionalId); // editarProfesional (no implementado)
        Task<IEnumerable<ProfessionalsDTO>> ListProfessionalsAsync(int hospitalId); // listarProfesionales
        Task RemoveProfessionalAsync(int hospitalId, int professionalId); // eliminarProfesionales
    }
}
