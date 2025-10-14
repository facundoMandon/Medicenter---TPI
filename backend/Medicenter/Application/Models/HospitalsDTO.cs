using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Application.Models
{
    internal class HospitalsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Adress { get; set; }

        public static HospitalsDTO FromEntity(Hospitals hospital)
        {
            return new HospitalsDTO
            {
                Id = hospital.Id,
                Name = hospital.Name,
                Adress = hospital.Adress,
            };
        }
    }
}
