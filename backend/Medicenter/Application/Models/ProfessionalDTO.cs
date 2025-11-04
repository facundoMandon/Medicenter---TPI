using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Models
{
    public class ProfessionalDTO : UserDTO
    {
        public int LicenseNumber { get; set; }
        public string SpecialtyName { get; set; } = string.Empty;

        // Método para mappear de Professional a ProfessionalDTO
        public static ProfessionalDTO FromEntity(Professional professional, string specialtyName = "")
        {
            return new ProfessionalDTO
            {
                Id = professional.Id,
                Name = professional.Name,
                LastName = professional.LastName,
                DNI = professional.DNI,
                Email = professional.Email,
                Rol = professional.Rol,
                LicenseNumber = professional.LicenseNumber,
                SpecialtyName = specialtyName
            };
        }
    }
}
