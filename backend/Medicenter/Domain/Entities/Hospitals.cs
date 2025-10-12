using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Hospitals
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Adress { get; set; }

        public void registerProfessionals(Professionals professional) 
        {
            //logica
        }

        public void updateProfessionals(Professionals professional)
        {
            //logica
        }

        public List<Professionals> HospitalStaff() 
        { 
            //logica 
            return new List<Professionals>();
        }

        public void DeleteProfessionals(Professionals professional)
        {
            //lógica
        }

    }
}
