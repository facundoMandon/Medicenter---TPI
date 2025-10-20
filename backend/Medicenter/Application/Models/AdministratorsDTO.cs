﻿using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class AdministratorsDTO : UsersDTO
    {
        // No requiere atributos adicionales

        // Método para mappear de Administrators a AdministratorsDTO
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
