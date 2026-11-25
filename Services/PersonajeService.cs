using DSW_3P_PR02.Models;
using DSW_3P_PR02.Repositories;

namespace DSW_3P_PR02.Services;

public sealed class PersonajeService(IPersonajeRepository personajeRepository)
    : IPersonajeService
{
    public async Task<IReadOnlyList<Personaje>> ObtenerTodosAsync(
        CancellationToken cancellationToken = default) =>
        await personajeRepository.ObtenerTodosAsync(cancellationToken);

    public async Task<Personaje?> ObtenerPorIdAsync(
        int id,
        CancellationToken cancellationToken = default) =>
        await personajeRepository.ObtenerPorIdAsync(id, cancellationToken);

    public async Task<IReadOnlyList<Personaje>> ObtenerPorVideojuegoAsync(
        string nombre,
        CancellationToken cancellationToken = default) =>
        await personajeRepository.ObtenerPorVideojuegoAsync(nombre, cancellationToken);

    public async Task<IReadOnlyList<Personaje>> ObtenerPorNivelMinimoAsync(
        int nivel,
        CancellationToken cancellationToken = default) =>
        await personajeRepository.ObtenerPorNivelMinimoAsync(nivel, cancellationToken);

    public async Task<Personaje> CrearAsync(
        Personaje personaje,
        CancellationToken cancellationToken = default)
    {
        personaje.Disponible = true;
        return await personajeRepository.CrearAsync(personaje, cancellationToken);
    }

    public async Task<bool> ActualizarAsync(
        Personaje personaje,
        CancellationToken cancellationToken = default) =>
        await personajeRepository.ActualizarAsync(personaje, cancellationToken);

    public async Task<bool> EliminarAsync(
        int id,
        CancellationToken cancellationToken = default) =>
        await personajeRepository.EliminarAsync(id, cancellationToken);
}
