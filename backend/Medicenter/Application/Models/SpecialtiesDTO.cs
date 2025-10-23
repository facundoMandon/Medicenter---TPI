using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Models
{
    public class SpecialtiesDTO
    {
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;

        // Método FromEntity
        public static SpecialtiesDTO FromEntity(Specialties specialty)
        {
            return new SpecialtiesDTO
            {
                Id = specialty.Id,
                Tipo = specialty.Tipo,
                Descripcion = specialty.Descripcion
            };
        }
    }
}
