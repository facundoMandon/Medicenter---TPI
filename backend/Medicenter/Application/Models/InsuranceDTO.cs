using Domain.Enums;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    internal class InsuranceDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public MedicalCoverageType CoverageType { get; set; }

        public static InsuranceDTO FromEntiy(Insurance insurance)
        {
            return new InsuranceDTO
            {
                Id = insurance.Id,
                Name = insurance.Name,
                Description = insurance.Description,
                CoverageType = insurance.CoverageType,
            };
        }
    }
}
