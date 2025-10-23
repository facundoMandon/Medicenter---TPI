using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Models
{
    public class HospitalsDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;

        // Método FromEntity
        public static HospitalsDTO FromEntity(Hospitals hospital)
        {
            return new HospitalsDTO
            {
                Id = hospital.Id,
                Nombre = hospital.Nombre,
                Direccion = hospital.Direccion
            };
        }
    }
}
