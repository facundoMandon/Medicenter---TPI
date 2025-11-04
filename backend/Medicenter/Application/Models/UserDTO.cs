using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int DNI { get; set; }
        public string Email { get; set; } = string.Empty;
        public Roles Rol { get; set; }

        // Método para mappear de entidad a DTO
        public static UserDTO FromEntity(User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                LastName = user.LastName,
                DNI = user.DNI,
                Email = user.Email,
                Rol = user.Rol
            };
        }
    }
}
