﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Request
{
    public class CreationSpecialtiesDTO
    {
        [Required]
        public string Tipo { get; set; } = string.Empty;

        public string Descripcion { get; set; } = string.Empty;
    }
}
