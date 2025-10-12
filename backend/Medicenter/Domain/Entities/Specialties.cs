using Domain.Entities.Relations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Specialties
    {
        public int Id { get; set; } // ID: int
        public string Type { get; set; } // Tipo: string
        public string Description { get; set; } // Descripción: string

        // Propiedad de Navegación de Colección para M:M (con ProfessionalSpecialty)
        public ICollection<ProfessionalSpecialty> ProfessionalsEnlazados { get; private set; } = new HashSet<ProfessionalSpecialty>();

        // Métodos del diagrama
        public void AddSpecialty() { /* Lógica */ }
        public void AsignSpecialty() { /* Lógica */ }
        public void RevoqueSpecialty() { /* Lógica */ }
        public void DeleteSpecialty() { /* Lógica */ }
        public void ModifySpecialty() { /* Lógica */ }
    }
}