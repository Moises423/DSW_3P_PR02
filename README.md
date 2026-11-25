# CharacterHub - DSW_3P_PR02

API REST para administrar personajes de videojuegos, desarrollada con ASP.NET Core Web API y controllers.
El CRUD usa MVC + Repository + Service y persiste los personajes en SQL Server mediante Entity Framework Core.

## Requisitos

- .NET SDK 10.0 (LTS)

## Ejecutar

Desde esta carpeta:

```powershell
dotnet run --project .\DSW_3P_PR02.csproj --launch-profile http
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

Si aún no se aplicó la migración en el entorno actual, ejecuta los comandos anteriores desde PowerShell.

## Autenticación JWT

`appsettings.json` contiene únicamente los valores no sensibles `Jwt:Issuer`, `Jwt:Audience` y `Jwt:ExpireMinutes`. La clave de firma y las credenciales de demostración se leen de User Secrets en desarrollo (o de variables de entorno fuera de desarrollo); nunca las guardes en archivos del proyecto.

Configura las credenciales que el endpoint de login aceptará:

```powershell
dotnet user-secrets set "DemoCredentials:Username" "<usuario demo>" --project .\DSW_3P_PR02.csproj
dotnet user-secrets set "DemoCredentials:Password" "<contraseña demo>" --project .\DSW_3P_PR02.csproj
```

Genera y almacena una clave aleatoria de 32 bytes (más de 32 caracteres), sin imprimirla:

```powershell
$bytesJwt = New-Object 'System.Byte[]' 32
$rngJwt = [Security.Cryptography.RandomNumberGenerator]::Create()
$rngJwt.GetBytes($bytesJwt)
$claveJwt = [Convert]::ToBase64String($bytesJwt)
dotnet user-secrets set "Jwt:Key" $claveJwt --project .\DSW_3P_PR02.csproj
$rngJwt.Dispose()
Remove-Variable bytesJwt, rngJwt, claveJwt
```

Para probarlo en Postman:

1. Envía un `POST` a `http://localhost:5033/api/auth/login` con un JSON `{"usuario":"<usuario demo>","contrasena":"<contraseña demo>"}` usando los valores configurados en User Secrets.
2. Copia el campo `token` de la respuesta.
3. En las solicitudes a `/api/personajes`, selecciona **Authorization > Bearer Token** y pega el token. Sin token, con uno inválido o expirado, la API responde `401 Unauthorized`.

El archivo [DSW_3P_PR02.http](./DSW_3P_PR02.http) también incluye el login y el encabezado Bearer para probar los endpoints.

## Endpoints disponibles

- `GET /api/personajes`
- `GET /api/personajes/{id}`
- `GET /api/personajes/videojuego/{nombre}`
- `GET /api/personajes/nivel/{nivel}`
- `POST /api/personajes`
- `PUT /api/personajes/{id}`
- `DELETE /api/personajes/{id}`
- `POST /api/auth/login`

Las solicitudes de ejemplo están en [DSW_3P_PR02.http](./DSW_3P_PR02.http). Se pueden ejecutar con la extensión REST Client de VS Code.

## Estructura

- `Controllers/`: endpoints HTTP.
- `Dtos/`: contratos de entrada y salida.
- `Models/`: entidades del dominio.
- `Data/`: `AppDbContext` y configuración de la base de datos.
- `Repositories/`: acceso a SQL Server mediante EF Core.
- `Services/`: reglas de negocio.
- `Validation/`: validaciones personalizadas.
