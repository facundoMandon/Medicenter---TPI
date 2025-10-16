using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Request
{
    public class CreationHospitalsDTO
    {
        [Required] public string Name { get; set; } = string.Empty;
        [Required] public string Address { get; set; } = string.Empty;
    }
}
