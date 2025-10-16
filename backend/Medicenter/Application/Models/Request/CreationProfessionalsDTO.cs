using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Request
{
    public class CreationProfessionalsDTO : CreationUsersDTO
    {
        [Required]
        public int LicenseNumber { get; set; } // n_matrícula: int

        [Required]
        public int SpecialtyId { get; set; } // FK a Especialidad
    }
}

