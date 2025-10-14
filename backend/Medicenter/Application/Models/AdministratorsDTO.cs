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
    public class AdministratorsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int DNI { get; set; }
        public string Email { get; set; }
        // public string Password { get; set; } NO LO MANDAMOS AL FRONTEND
        public Roles Rol { get; set; }

        //ayuda a mappear el Users a formato UsersDTO (le saco el password)
        public static AdministratorsDTO FromEntity(Administrators admin)
        {
            return new AdministratorsDTO
            {
                Id = admin.Id,
                Name = admin.Name,
                LastName = admin.LastName,
                DNI = admin.DNI,
                Email = admin.Email,
                Rol = admin.Rol
            };
        }
    }
}
