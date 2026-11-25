# CharacterHub - DSW_3P_PR02

API REST para administrar personajes de videojuegos, desarrollada con ASP.NET Core Web API y controllers.
El CRUD usa MVC + Repository + Service y persiste los personajes en SQL Server mediante Entity Framework Core.

## Requisitos

- .NET SDK 10.0 (LTS)

## Ejecutar

Desde esta carpeta:

```powershell
dotnet run --project .\DSW_3P_PR02.csproj
```

La URL HTTP de desarrollo configurada es `http://localhost:5033`.

## SQL Server y User Secrets

En desarrollo, `appsettings.Development.json` contiene solo una referencia local a `.\SQLEXPRESS`; no contiene credenciales. Configura la cadena completa de autenticación SQL en User Secrets bajo `ConnectionStrings:DefaultConnection`. La configuración de User Secrets reemplaza la referencia del archivo durante el entorno `Development`.

Si necesitas establecerla o actualizarla desde tu terminal, usa el siguiente comando y sustituye el marcador localmente; no guardes la cadena en archivos del repositorio:

```powershell
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<cadena SQL Server completa>" --project .\DSW_3P_PR02.csproj
```

Para crear la migración inicial y aplicar el esquema a la base configurada:

```powershell
$env:ASPNETCORE_ENVIRONMENT = "Development"
dotnet ef migrations add InitialCreate
dotnet ef database update
```

`AppDbContext` incluye cuatro personajes iniciales mediante `HasData`; se insertarán al aplicar la migración inicial. El contexto define longitudes máximas, campos requeridos y comparación de videojuego sin distinguir mayúsculas.

> La migración no está generada ni aplicada en este cambio. Ejecuta los comandos anteriores en tu terminal para capturar ese proceso.

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
- `Data/`: `AppDbContext` y configuración de la base de datos.
- `Repositories/`: acceso a SQL Server mediante EF Core.
- `Services/`: reglas de negocio.
- `Validation/`: validaciones personalizadas.
