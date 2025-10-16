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
        public int LicenseNumber { get; set; } // n_matrícula
        // Propiedad opcional para mostrar el nombre de la especialidad
        public string SpecialtyName { get; set; } = string.Empty;
    }
}
