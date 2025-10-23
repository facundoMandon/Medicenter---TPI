using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Request
{
    public class CreationAdministratorsDTO : CreationUsersDTO
    {
        // No requiere atributos adicionales
        // Hereda todo de CreationUsersDTO: Name, LastName, DNI, Email, Password, Rol
    }
}
