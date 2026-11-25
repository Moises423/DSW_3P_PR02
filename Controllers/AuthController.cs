using System.Security.Cryptography;
using System.Text;
using DSW_3P_PR02.Dtos;
using DSW_3P_PR02.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DSW_3P_PR02.Controllers;

[ApiController]
[Route("api/auth")]
public sealed class AuthController(
    DemoCredentials demoCredentials,
    ITokenService tokenService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public ActionResult<LoginResponse> Login([FromBody] LoginRequest request)
    {
        var usuarioValido = CoincideEnTiempoConstante(
            request.Usuario,
            demoCredentials.Username);
        var contrasenaValida = CoincideEnTiempoConstante(
            request.Contrasena,
            demoCredentials.Password);

        if (!(usuarioValido & contrasenaValida))
        {
            return Unauthorized(new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Credenciales inválidas",
                Detail = "El usuario o la contraseña no son válidos.",
                Instance = HttpContext.Request.Path
            });
        }

        return Ok(tokenService.GenerarToken(request.Usuario));
    }

    private static bool CoincideEnTiempoConstante(string valor, string esperado) =>
        CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(valor),
            Encoding.UTF8.GetBytes(esperado));
}
