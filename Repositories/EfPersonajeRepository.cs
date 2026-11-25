using DSW_3P_PR02.Data;
using DSW_3P_PR02.Models;
using Microsoft.EntityFrameworkCore;

namespace DSW_3P_PR02.Repositories;

public sealed class EfPersonajeRepository(AppDbContext dbContext) : IPersonajeRepository
{
    public async Task<IReadOnlyList<Personaje>> ObtenerTodosAsync(
        CancellationToken cancellationToken = default) =>
        await dbContext.Personajes
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public async Task<Personaje?> ObtenerPorIdAsync(
        int id,
        CancellationToken cancellationToken = default) =>
        await dbContext.Personajes
            .AsNoTracking()
            .FirstOrDefaultAsync(personaje => personaje.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Personaje>> ObtenerPorVideojuegoAsync(
        string nombre,
        CancellationToken cancellationToken = default) =>
        await dbContext.Personajes
            .AsNoTracking()
            .Where(personaje => personaje.Videojuego == nombre)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Personaje>> ObtenerPorNivelMinimoAsync(
        int nivel,
        CancellationToken cancellationToken = default) =>
        await dbContext.Personajes
            .AsNoTracking()
            .Where(personaje => personaje.Nivel >= nivel)
            .ToListAsync(cancellationToken);

    public async Task<Personaje> CrearAsync(
        Personaje personaje,
        CancellationToken cancellationToken = default)
    {
        personaje.Id = 0;
        dbContext.Personajes.Add(personaje);
        await dbContext.SaveChangesAsync(cancellationToken);
        return personaje;
    }

    public async Task<bool> ActualizarAsync(
        Personaje personaje,
        CancellationToken cancellationToken = default)
    {
        var existente = await dbContext.Personajes
            .FirstOrDefaultAsync(actual => actual.Id == personaje.Id, cancellationToken);

        if (existente is null)
        {
            return false;
        }

        dbContext.Entry(existente).CurrentValues.SetValues(personaje);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> EliminarAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var personaje = await dbContext.Personajes
            .FirstOrDefaultAsync(actual => actual.Id == id, cancellationToken);

        if (personaje is null)
        {
            return false;
        }

        dbContext.Personajes.Remove(personaje);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
