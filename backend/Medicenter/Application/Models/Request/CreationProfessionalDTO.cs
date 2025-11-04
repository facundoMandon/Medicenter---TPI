using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Request
{
    public class CreationProfessionalDTO : CreationUserDTO
    {
        [Required]
        public int LicenseNumber { get; set; }

        [Required]
        public int SpecialtyId { get; set; }
    }
}

