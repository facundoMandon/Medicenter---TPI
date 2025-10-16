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
    public class AdministratorsDTO : UsersDTO
    {
        // No se requieren atributos específicos en el DTO de salida
        // Los datos comunes vienen de UsersDTO
    }
}
