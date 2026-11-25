using DSW_3P_PR02.Models;
using DSW_3P_PR02.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DSW_3P_PR02.Controllers;

[ApiController]
[Authorize]
[Route("api/personajes")]
public sealed class PersonajesController(IPersonajeService personajeService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<Personaje>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<Personaje>>> ObtenerTodos(
        CancellationToken cancellationToken)
    {
        var personajes = await personajeService.ObtenerTodosAsync(cancellationToken);
        return Ok(personajes);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Personaje), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Personaje>> ObtenerPorId(
        int id,
        CancellationToken cancellationToken)
    {
        var personaje = await personajeService.ObtenerPorIdAsync(id, cancellationToken);
        if (personaje is null)
        {
            return NotFound(CrearProblema(
                StatusCodes.Status404NotFound,
                "Personaje no encontrado",
                $"No existe un personaje con el id {id}."));
        }

        return Ok(personaje);
    }

    [HttpGet("videojuego/{nombre}")]
    [ProducesResponseType(typeof(IReadOnlyList<Personaje>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<Personaje>>> ObtenerPorVideojuego(
        string nombre,
        CancellationToken cancellationToken)
    {
        var personajes = await personajeService.ObtenerPorVideojuegoAsync(
            nombre,
            cancellationToken);
        return Ok(personajes);
    }

    [HttpGet("nivel/{nivel:int}")]
    [ProducesResponseType(typeof(IReadOnlyList<Personaje>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<Personaje>>> ObtenerPorNivel(
        int nivel,
        CancellationToken cancellationToken)
    {
        var personajes = await personajeService.ObtenerPorNivelMinimoAsync(
            nivel,
            cancellationToken);
        return Ok(personajes);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Personaje), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Personaje>> Crear(
        [FromBody] Personaje personaje,
        CancellationToken cancellationToken)
    {
        var creado = await personajeService.CrearAsync(personaje, cancellationToken);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.Id }, creado);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Actualizar(
        int id,
        [FromBody] Personaje personaje,
        CancellationToken cancellationToken)
    {
        if (id != personaje.Id)
        {
            return BadRequest(CrearProblema(
                StatusCodes.Status400BadRequest,
                "Id no coincidente",
                "El id de la ruta debe coincidir con el id del cuerpo."));
        }

        var actualizado = await personajeService.ActualizarAsync(personaje, cancellationToken);
        if (!actualizado)
        {
            return NotFound(CrearProblema(
                StatusCodes.Status404NotFound,
                "Personaje no encontrado",
                $"No existe un personaje con el id {id}."));
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Eliminar(
        int id,
        CancellationToken cancellationToken)
    {
        var eliminado = await personajeService.EliminarAsync(id, cancellationToken);
        if (!eliminado)
        {
            return NotFound(CrearProblema(
                StatusCodes.Status404NotFound,
                "Personaje no encontrado",
                $"No existe un personaje con el id {id}."));
        }

        return NoContent();
    }

    private ProblemDetails CrearProblema(int status, string title, string detail) =>
        new()
        {
            Status = status,
            Title = title,
            Detail = detail,
            Instance = HttpContext.Request.Path
        };
}
