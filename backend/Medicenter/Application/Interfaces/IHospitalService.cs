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
    public interface IHospitalService
    {
        // CRUD
        Task<HospitalDTO> GetByIdAsync(int id);
        Task<IEnumerable<HospitalDTO>> GetAllAsync();
        Task<HospitalDTO> CreateHospitalAsync(CreationHospitalDTO dto);
        Task<HospitalDTO> UpdateHospitalAsync(int id, CreationHospitalDTO dto);
        Task DeleteHospitalAsync(int id);

        // Métodos del diagrama
        Task RegisterProfessionalAsync(int hospitalId, int professionalId); // registrarProfesional
        Task EditProfessionalAsync(int hospitalId, int professionalId); // editarProfesional (no implementado)
        Task<IEnumerable<ProfessionalDTO>> ListProfessionalAsync(int hospitalId); // listarProfesionales
        Task RemoveProfessionalAsync(int hospitalId, int professionalId); // eliminarProfesionales
    }
}
