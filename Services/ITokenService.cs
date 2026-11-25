using DSW_3P_PR02.Dtos;

namespace DSW_3P_PR02.Services;

public interface ITokenService
{
    LoginResponse GenerarToken(string usuario);
}
