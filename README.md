# CharacterHub - DSW_3P_PR02

API REST para administrar personajes de videojuegos, desarrollada con ASP.NET Core Web API y controllers.
Esta fase implementa el CRUD con persistencia JSON local y separación MVC + Repository + Service.

## Requisitos

- .NET SDK 10.0 (LTS)

## Ejecutar

Desde esta carpeta:

```powershell
dotnet run --project .\DSW_3P_PR02.csproj
```

La URL HTTP de desarrollo configurada es `http://localhost:5033`.

El archivo `Data/personajes.json` y sus cuatro personajes iniciales se crean al atender la primera solicitud si el archivo todavía no existe. Las modificaciones posteriores se conservan en ese archivo.

## Endpoints disponibles

- `GET /api/personajes`
- `GET /api/personajes/{id}`
- `GET /api/personajes/videojuego/{nombre}`
- `GET /api/personajes/nivel/{nivel}`
- `POST /api/personajes`
- `PUT /api/personajes/{id}`
- `DELETE /api/personajes/{id}`

Las solicitudes de ejemplo están en [DSW_3P_PR02.http](./DSW_3P_PR02.http). Se pueden ejecutar con la extensión REST Client de VS Code.

## Estructura

- `Controllers/`: endpoints HTTP.
- `Dtos/`: contratos de entrada y salida.
- `Models/`: entidades del dominio.
- `Repositories/`: acceso a persistencia.
- `Services/`: reglas de negocio.
- `Validation/`: validaciones personalizadas.
- `Data/`: archivos de datos locales.
