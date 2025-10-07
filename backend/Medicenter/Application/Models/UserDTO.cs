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
    internal class UsersDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int DNI { get; set; }
        public string Email { get; set; }
        // public string Password { get; set; } NO LO MANDAMOS AL FRONTEND
        public Roles Rol { get; set; }
    }

        //ayuda a mappear el Users a formato UsersDTO (le saco el password)
        public static UsersDTO FromEntity(Users user)
        {
            return new UsersDTO
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
