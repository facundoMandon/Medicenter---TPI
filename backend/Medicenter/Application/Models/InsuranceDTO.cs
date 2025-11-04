using Domain.Enums;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class InsuranceDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public MedicalCoverageType MedicalCoverageType { get; set; }

        // Método FromEntity
        public static InsuranceDTO FromEntity(Insurance insurance)
        {
            return new InsuranceDTO
            {
                Id = insurance.Id,
                Name = insurance.Name,
                Description = insurance.Description,
                MedicalCoverageType = insurance.MedicalCoverageType
            };
        }
    }
}
