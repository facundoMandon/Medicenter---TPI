using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Models
{
    internal class PatientsDTO
    {
        public int affiliate_number { get; set; }
        public int InsuranceId { get; set; }

        public static PatientsDTO FromEntity(Patients patient)
        {
            return new PatientsDTO
            {
                affiliate_number = patient.affiliate_number,
                InsuranceId = patient.InsuranceId,
            };
        }
    }
}
