using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Models
{
    public class PatientDTO : UserDTO
    {
        public int AffiliateNumber { get; set; }
        public string InsuranceName { get; set; } = string.Empty;

        // Método para mappear de Patient a PatientDTO
        public static PatientDTO FromEntity(Patient patient, string insuranceName = "")
        {
            return new PatientDTO
            {
                Id = patient.Id,
                Name = patient.Name,
                LastName = patient.LastName,
                DNI = patient.DNI,
                Email = patient.Email,
                Rol = patient.Rol,
                AffiliateNumber = patient.AffiliateNumber,
                InsuranceName = insuranceName
            };
        }
    }
}
