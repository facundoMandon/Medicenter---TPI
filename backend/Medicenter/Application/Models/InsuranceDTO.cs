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
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public MedicalCoverageType TipoCobertura { get; set; }

        // Método FromEntity
        public static InsuranceDTO FromEntity(Insurance insurance)
        {
            return new InsuranceDTO
            {
                Id = insurance.Id,
                Nombre = insurance.Nombre,
                Descripcion = insurance.Descripcion,
                TipoCobertura = insurance.TipoCobertura
            };
        }
    }
}
