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
        public int AffiliateNumber { get; set; } // n_Afiliado
        // Propiedad opcional para mostrar el nombre de la obra social
        public string InsuranceName { get; set; } = string.Empty;
    }
}
