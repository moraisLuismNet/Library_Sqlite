using Library.DTOs;

namespace Library.Services
{
    public interface ITokenService
    {
        LoginResponseDTO GenerateToken(UserDTO credentialsUser);
    }

}