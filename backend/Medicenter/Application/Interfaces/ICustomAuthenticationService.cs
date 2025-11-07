using Application.Models.Request;

namespace Application.Interfaces
{
    public interface ICustomAuthenticationService
    {
        Task<string> AuthenticateAsync(AuthenticationRequestDTO authenticationRequestDTO);
        Task ResetPasswordAsync(ResetPasswordRequestDTO request);
    }
}