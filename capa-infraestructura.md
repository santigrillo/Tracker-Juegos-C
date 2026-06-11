## Capa de Infraestructura (o datos).

1. Instalar el motor principal de EF Core para SQLite en Infraestructura
`dotnet add TrackerJuegos.Infrastructure/TrackerJuegos.Infrastructure.csproj package Microsoft.EntityFrameworkCore.Sqlite`

2. Instalar las herramientas de diseño (necesarias para crear las migraciones)
`dotnet add TrackerJuegos.Infrastructure/TrackerJuegos.Infrastructure.csproj package Microsoft.EntityFrameworkCore.Design`

3. (Opcional pero recomendado) Instalar EF Core Tools a nivel global en entorno Linux
`dotnet tool install --global dotnet-ef`

## Creación de contexto de base de datos.
El DbContext es la clase que representa la sesión con la base de datos. Acá es donde se usa Fluent API para mapear la clase TrackingRecord a una tabla SQL.

Crear ApplicationDbContext.cs dentro de un nuevo directorio en Infraestructura:
`mkdir -p TrackerJuegos.Infrastructure/Persistence`
`touch TrackerJuegos.Infrastructure/Persistence/ApplicationDbContext.cs`