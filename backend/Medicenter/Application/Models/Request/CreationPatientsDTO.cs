using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Request
{
    public class CreationPatientsDTO : CreationUsersDTO
    {
        [Required]
        public int AffiliateNumber { get; set; }

        [Required]
        public int InsuranceId { get; set; }
    }
}
