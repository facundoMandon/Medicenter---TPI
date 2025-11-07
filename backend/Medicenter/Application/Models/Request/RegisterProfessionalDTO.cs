using System.ComponentModel.DataAnnotations;

namespace Application.Models.Request
{
    public class RegisterProfessionalDTO
    {
        [Required] public string Name { get; set; } = string.Empty;
        [Required] public string LastName { get; set; } = string.Empty;
        [Required] public int DNI { get; set; }
        [Required, EmailAddress] public string Email { get; set; } = string.Empty;
        [Required, MinLength(8)] public string Password { get; set; } = string.Empty;
        [Required] public int LicenseNumber { get; set; }
        [Required] public int SpecialtyId { get; set; }
    }
}