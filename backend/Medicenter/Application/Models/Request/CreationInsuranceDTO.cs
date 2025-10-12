using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Request
{
    public class CreationInsuranceDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public MedicalCoverageType CoverageType { get; set; }
    } 
}
