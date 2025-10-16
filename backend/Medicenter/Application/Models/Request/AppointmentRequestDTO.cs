using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Request
{
    public class AppointmentRequestDTO
    {
        [Required] public int ProfessionalId { get; set; }
        [Required] public DateTime AppointmentDate { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
