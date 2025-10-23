using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Models
{
    public class PatientsDTO : UsersDTO
    {
        public int AffiliateNumber { get; set; }
        public string InsuranceName { get; set; } = string.Empty;

        // Método para mappear de Patients a PatientsDTO
        public static PatientsDTO FromEntity(Patients patient, string insuranceName = "")
        {
            return new PatientsDTO
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
