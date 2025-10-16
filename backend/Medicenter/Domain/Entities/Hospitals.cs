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
        [Required] public int Id { get; set; } // ID: int

        [Required, MaxLength(150)]
        public string Name { get; set; } = string.Empty; // Nombre: string

        [Required, MaxLength(250)]
        public string Address { get; set; } = string.Empty; // Dirección: string

        // Relación N:M con Professionals (asumiendo que un profesional trabaja en varios hospitales)
        // Necesaria para los métodos registrarProfesional, listarProfesionales, etc.
        public ICollection<Professionals> Professionals { get; set; } = new List<Professionals>();
    }
}
