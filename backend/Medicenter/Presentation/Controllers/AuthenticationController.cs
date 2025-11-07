using Application.Interfaces;
using Application.Models.Request;
using Domain.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ICustomAuthenticationService _customAuthenticationService;
        private readonly IPatientService _patientService;
        private readonly IProfessionalService _professionalService;
        private readonly IAdministratorService _administratorService;

        public AuthenticationController(
            ICustomAuthenticationService customAuthenticationService,
            IPatientService patientService,
            IProfessionalService professionalService,
            IAdministratorService administratorService)
        {
            _customAuthenticationService = customAuthenticationService;
            _patientService = patientService;
            _professionalService = professionalService;
            _administratorService = administratorService;
        }

        /// Login - Retorna el token JWT
        /// POST: api/Authentication/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Authenticate([FromBody] AuthenticationRequestDTO authenticationRequestDTO)
        {
            try
            {
                string newToken = await _customAuthenticationService.AuthenticateAsync(authenticationRequestDTO);
                return Ok(new { token = newToken });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }

        /// Restablecer contraseña - Público
        /// POST: api/Authentication/password/reset
        [HttpPost("password/reset")]
        [AllowAnonymous]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordRequestDTO request)
        {
            try
            {
                await _customAuthenticationService.ResetPasswordAsync(request);
                return Ok(new { message = "Contraseña actualizada exitosamente." });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// Registro de Paciente - Público
        /// POST: api/Authentication/register/patient
        [HttpPost("register/patient")]
        [AllowAnonymous]
        public async Task<ActionResult> RegisterPatient([FromBody] RegisterPatientDTO dto)
        {
            try
            {
                var creationDto = new CreationPatientDTO
                {
                    Name = dto.Name,
                    LastName = dto.LastName,
                    DNI = dto.DNI,
                    Email = dto.Email,
                    Password = dto.Password,
                    AffiliateNumber = dto.AffiliateNumber,
                    InsuranceId = dto.InsuranceId,
                    Rol = Domain.Enums.Roles.Patient
                };

                var patient = await _patientService.CreatePatientAsync(creationDto);
                return Ok(new
                {
                    message = "Paciente registrado exitosamente. Ya puedes iniciar sesión.",
                    user = patient
                });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (DuplicateException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// Registro de Profesional - Público
        /// POST: api/Authentication/register/professional
        [HttpPost("register/professional")]
        [AllowAnonymous]
        public async Task<ActionResult> RegisterProfessional([FromBody] RegisterProfessionalDTO dto)
        {
            try
            {
                var creationDto = new CreationProfessionalDTO
                {
                    Name = dto.Name,
                    LastName = dto.LastName,
                    DNI = dto.DNI,
                    Email = dto.Email,
                    Password = dto.Password,
                    LicenseNumber = dto.LicenseNumber,
                    SpecialtyId = dto.SpecialtyId,
                    Rol = Domain.Enums.Roles.Professional
                };

                var professional = await _professionalService.CreateProfessionalAsync(creationDto);
                return Ok(new
                {
                    message = "Profesional registrado exitosamente. Ya puedes iniciar sesión.",
                    user = professional
                });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (DuplicateException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        /// Registro de Administrador - SOLO admin autenticado puede crear administradores
        /// POST: api/Authentication/register/administrator
        [HttpPost("register/administrator")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> RegisterAdministrator([FromBody] RegisterAdministratorDTO dto)
        {
            try
            {
                var creationDto = new CreationUserDTO
                {
                    Name = dto.Name,
                    LastName = dto.LastName,
                    DNI = dto.DNI,
                    Email = dto.Email,
                    Password = dto.Password,
                    Rol = Domain.Enums.Roles.Administrator
                };

                var admin = await _administratorService.CreateAdministratorAsync(creationDto);
                return Ok(new
                {
                    message = "Administrador registrado exitosamente.",
                    user = admin
                });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (DuplicateException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }


    }
}