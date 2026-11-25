using DSW_3P_PR02.Models;

namespace DSW_3P_PR02.Services;

public interface IPersonajeService
{
    Task<IReadOnlyList<Personaje>> ObtenerTodosAsync(CancellationToken cancellationToken = default);

    Task<Personaje?> ObtenerPorIdAsync(
        int id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Personaje>> ObtenerPorVideojuegoAsync(
        string nombre,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Personaje>> ObtenerPorNivelMinimoAsync(
        int nivel,
        CancellationToken cancellationToken = default);

    Task<Personaje> CrearAsync(
        Personaje personaje,
        CancellationToken cancellationToken = default);

    Task<bool> ActualizarAsync(
        Personaje personaje,
        CancellationToken cancellationToken = default);

    Task<bool> EliminarAsync(
        int id,
        CancellationToken cancellationToken = default);
}
