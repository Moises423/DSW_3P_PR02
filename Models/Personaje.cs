using System.ComponentModel.DataAnnotations;
using DSW_3P_PR02.Validation;

namespace DSW_3P_PR02.Models;

public sealed class Personaje
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El nombre es obligatorio.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El videojuego es obligatorio.")]
    public string Videojuego { get; set; } = string.Empty;

    [Required(ErrorMessage = "La clase es obligatoria.")]
    public string Clase { get; set; } = string.Empty;

    [Range(1, 100, ErrorMessage = "El nivel debe estar entre 1 y 100.")]
    public int Nivel { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "El poder debe ser mayor que 0.")]
    public int Poder { get; set; }

    public bool Disponible { get; set; } = true;

    [UrlHttp]
    public string? ImagenUrl { get; set; }
}
