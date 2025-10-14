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
        public string Type { get; set; }
        public string Description { get; set; } 

        public static SpecialtiesDTO FromEntity(Specialties specialty)
        {
            return new SpecialtiesDTO
            {
                Id = specialty.Id,
                Type = specialty.Type,
                Description = specialty.Description,
            };
        }
    }
}
