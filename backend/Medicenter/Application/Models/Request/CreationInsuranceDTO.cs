﻿using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models.Request
{
    public class CreationInsuranceDTO
    {
        [Required] public string Name { get; set; } = string.Empty;
        [Required] public string Plan { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
