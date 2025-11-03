using System.ComponentModel.DataAnnotations;

namespace Application.Models.Request
{
    public class CreationAppointmentDTO
    {
        [Required]
        public int PatientId { get; set; }

        [Required]
        public int ProfessionalId { get; set; }

        [Required]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "El año debe tener 4 dígitos")]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "El año debe ser numérico")]
        public string Year { get; set; } = string.Empty; // Ej: "2025"

        [Required]
        [StringLength(2, MinimumLength = 1, ErrorMessage = "El mes debe tener 1 o 2 dígitos")]
        [RegularExpression(@"^(0?[1-9]|1[0-2])$", ErrorMessage = "El mes debe estar entre 01 y 12")]
        public string Month { get; set; } = string.Empty; // Ej: "11" o "1"

        [Required]
        [StringLength(2, MinimumLength = 1, ErrorMessage = "El día debe tener 1 o 2 dígitos")]
        [RegularExpression(@"^(0?[1-9]|[12][0-9]|3[01])$", ErrorMessage = "El día debe estar entre 01 y 31")]
        public string Day { get; set; } = string.Empty; // Ej: "23" o "5"

        [Required]
        [RegularExpression(@"^([01]?[0-9]|2[0-3]):[0-5][0-9]$", ErrorMessage = "La hora debe tener formato HH:mm")]
        public string Hora { get; set; } = string.Empty; // Ej: "14:30"

        public string Descripcion { get; set; } = string.Empty;
    }
}