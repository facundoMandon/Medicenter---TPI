using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Request
{
    public class CreationUsersDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public int DNI { get; set; }

        [Required]
        [EmailAddress] // Para validación en la capa de Presentación
        public string Email { get; set; }

        [Required]
        [MinLength(8)] // Para validación de seguridad
        public string Password { get; set; }
                                       
        [Required]
        public Roles rol { get; set; }
    }
}