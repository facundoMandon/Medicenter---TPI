using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Models
{
    public class ProfessionalsDTO : UsersDTO
    {
        public int LicenseNumber { get; set; }
        public string SpecialtyName { get; set; } = string.Empty;

        // Método para mappear de Professionals a ProfessionalsDTO
        public static ProfessionalsDTO FromEntity(Professionals professional, string specialtyName = "")
        {
            return new ProfessionalsDTO
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
