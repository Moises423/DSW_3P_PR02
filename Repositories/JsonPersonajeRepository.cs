using System.Text.Json;
using DSW_3P_PR02.Models;

namespace DSW_3P_PR02.Repositories;

public sealed class JsonPersonajeRepository : IPersonajeRepository
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    private readonly string _rutaArchivo;
    private readonly SemaphoreSlim _semaforo = new(1, 1);

    public JsonPersonajeRepository(IHostEnvironment environment)
    {
        _rutaArchivo = Path.Combine(environment.ContentRootPath, "Data", "personajes.json");
    }

    public async Task<IReadOnlyList<Personaje>> ObtenerTodosAsync(
        CancellationToken cancellationToken = default)
    {
        await _semaforo.WaitAsync(cancellationToken);
        try
        {
            var personajes = await LeerPersonajesAsync(cancellationToken);
            return personajes.ToArray();
        }
        finally
        {
            _semaforo.Release();
        }
    }

    public async Task<Personaje?> ObtenerPorIdAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        await _semaforo.WaitAsync(cancellationToken);
        try
        {
            var personajes = await LeerPersonajesAsync(cancellationToken);
            return personajes.FirstOrDefault(personaje => personaje.Id == id);
        }
        finally
        {
            _semaforo.Release();
        }
    }

    public async Task<IReadOnlyList<Personaje>> ObtenerPorVideojuegoAsync(
        string nombre,
        CancellationToken cancellationToken = default)
    {
        await _semaforo.WaitAsync(cancellationToken);
        try
        {
            var personajes = await LeerPersonajesAsync(cancellationToken);
            return personajes
                .Where(personaje => string.Equals(
                    personaje.Videojuego,
                    nombre,
                    StringComparison.OrdinalIgnoreCase))
                .ToArray();
        }
        finally
        {
            _semaforo.Release();
        }
    }

    public async Task<IReadOnlyList<Personaje>> ObtenerPorNivelMinimoAsync(
        int nivel,
        CancellationToken cancellationToken = default)
    {
        await _semaforo.WaitAsync(cancellationToken);
        try
        {
            var personajes = await LeerPersonajesAsync(cancellationToken);
            return personajes
                .Where(personaje => personaje.Nivel >= nivel)
                .ToArray();
        }
        finally
        {
            _semaforo.Release();
        }
    }

    public async Task<Personaje> CrearAsync(
        Personaje personaje,
        CancellationToken cancellationToken = default)
    {
        await _semaforo.WaitAsync(cancellationToken);
        try
        {
            var personajes = await LeerPersonajesAsync(cancellationToken);
            var siguienteId = personajes.Count == 0 ? 1 : checked(personajes.Max(p => p.Id) + 1);

            personaje.Id = siguienteId;
            personajes.Add(personaje);
            await GuardarPersonajesAsync(personajes, cancellationToken);

            return personaje;
        }
        finally
        {
            _semaforo.Release();
        }
    }

    public async Task<bool> ActualizarAsync(
        Personaje personaje,
        CancellationToken cancellationToken = default)
    {
        await _semaforo.WaitAsync(cancellationToken);
        try
        {
            var personajes = await LeerPersonajesAsync(cancellationToken);
            var indice = personajes.FindIndex(actual => actual.Id == personaje.Id);
            if (indice < 0)
            {
                return false;
            }

            personajes[indice] = personaje;
            await GuardarPersonajesAsync(personajes, cancellationToken);
            return true;
        }
        finally
        {
            _semaforo.Release();
        }
    }

    public async Task<bool> EliminarAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        await _semaforo.WaitAsync(cancellationToken);
        try
        {
            var personajes = await LeerPersonajesAsync(cancellationToken);
            var personaje = personajes.FirstOrDefault(actual => actual.Id == id);
            if (personaje is null)
            {
                return false;
            }

            personajes.Remove(personaje);
            await GuardarPersonajesAsync(personajes, cancellationToken);
            return true;
        }
        finally
        {
            _semaforo.Release();
        }
    }

    private async Task<List<Personaje>> LeerPersonajesAsync(
        CancellationToken cancellationToken)
    {
        await AsegurarArchivoAsync(cancellationToken);

        await using var stream = new FileStream(
            _rutaArchivo,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read,
            bufferSize: 4096,
            useAsync: true);

        return await JsonSerializer.DeserializeAsync<List<Personaje>>(
                   stream,
                   JsonOptions,
                   cancellationToken)
               ?? throw new InvalidDataException(
                   $"El archivo de personajes '{_rutaArchivo}' no contiene una lista JSON válida.");
    }

    private async Task AsegurarArchivoAsync(CancellationToken cancellationToken)
    {
        if (File.Exists(_rutaArchivo))
        {
            return;
        }

        Directory.CreateDirectory(Path.GetDirectoryName(_rutaArchivo)!);
        await GuardarPersonajesAsync(CrearSemilla(), cancellationToken);
    }

    private async Task GuardarPersonajesAsync(
        List<Personaje> personajes,
        CancellationToken cancellationToken)
    {
        await using var stream = new FileStream(
            _rutaArchivo,
            FileMode.Create,
            FileAccess.Write,
            FileShare.None,
            bufferSize: 4096,
            useAsync: true);

        await JsonSerializer.SerializeAsync(
            stream,
            personajes,
            JsonOptions,
            cancellationToken);
    }

    private static List<Personaje> CrearSemilla() =>
    [
        new()
        {
            Id = 1,
            Nombre = "Lara Croft",
            Videojuego = "Tomb Raider",
            Clase = "Exploradora",
            Nivel = 95,
            Poder = 90,
            Disponible = true
        },
        new()
        {
            Id = 2,
            Nombre = "Link",
            Videojuego = "The Legend of Zelda",
            Clase = "Espadachín",
            Nivel = 85,
            Poder = 88,
            Disponible = true
        },
        new()
        {
            Id = 3,
            Nombre = "Samus Aran",
            Videojuego = "Metroid",
            Clase = "Cazarrecompensas",
            Nivel = 92,
            Poder = 96,
            Disponible = true
        },
        new()
        {
            Id = 4,
            Nombre = "Geralt de Rivia",
            Videojuego = "The Witcher 3",
            Clase = "Brujo",
            Nivel = 90,
            Poder = 93,
            Disponible = true
        }
    ];
}
