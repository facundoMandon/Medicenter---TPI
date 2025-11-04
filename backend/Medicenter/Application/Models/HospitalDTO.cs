using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Models
{
    public class HospitalDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Adress { get; set; } = string.Empty;

        // Método FromEntity
        public static HospitalDTO FromEntity(Hospital hospital)
        {
            return new HospitalDTO
            {
                Id = hospital.Id,
                Name = hospital.Name,
                Adress = hospital.Adress
            };
        }
    }
}
