using System.ComponentModel.DataAnnotations;

namespace DSW_3P_PR02.Dtos;

/// <summary>Credenciales de acceso para solicitar un token.</summary>
public sealed record LoginRequest
{
    [Required(ErrorMessage = "El usuario es obligatorio.")]
    public required string Usuario { get; init; }

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    public required string Contrasena { get; init; }
}
