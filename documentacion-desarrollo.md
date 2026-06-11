## Etapa inicial.

1. Inicializar proyecto en blanco -> `dotnet new sln -n TrackerJuegos`

2. Crear capa de dominio -> `dotnet new classlib -n TrackerJuegos.Domain` Contiene entidades puras e interfaces de contratos.

3. Crear capa de aplicación -> `dotnet new classlib -n TrackerJuegos.Application` Contiene casos de uso, DTOs, interfaces de servicios. Depende unicamente del dominio.

4. Crear capa de Infraestructura -> `dotnet new classlib -n TrackerJuegos.Infrastructure` Contiene implementación de repositorios, cliente HTTP de RAWG, Caché, Base de Datos. Depende de Application y Domain.

5. Crear la capa API Web -> `dotnet new webapi -n TrackerJuegos.WebApi` Contiene Controladores, Middlewares, Endpoints REST.

6. Registrar todos los proyectos dentro de la solución principal  
`dotnet sln add TrackerJuegos.Domain/TrackerJuegos.Domain.csproj` 
`dotnet sln add TrackerJuegos.Application/TrackerJuegos.Application.csproj`
`dotnet sln add TrackerJuegos.Infrastructure/TrackerJuegos.Infrastructure.csproj`
`dotnet sln add TrackerJuegos.WebApi/TrackerJuegos.WebApi.csproj`

7. Establecer las referencias correctas para evitar dependencias circulares (Efecto Zig-Zag) 
* Aplicación referencia a Dominio  
`dotnet add TrackerJuegos.Application/TrackerJuegos.Application.csproj reference TrackerJuegos.Domain/TrackerJuegos.Domain.csproj`

* Infraestructura referencia a Aplicación  
`dotnet add TrackerJuegos.Infrastructure/TrackerJuegos.Infrastructure.csproj reference TrackerJuegos.Application/TrackerJuegos.Application.csproj`

* Infraestructura referencia a Dominio
`dotnet add TrackerJuegos.Infrastructure/TrackerJuegos.Infrastructure.csproj reference TrackerJuegos.Domain/TrackerJuegos.Domain.csproj`

* WebApi referencia a Infraestructura y Aplicación  
`dotnet add TrackerJuegos.WebApi/TrackerJuegos.WebApi.csproj reference TrackerJuegos.Infrastructure/TrackerJuegos.Infrastructure.csproj`
`dotnet add TrackerJuegos.WebApi/TrackerJuegos.WebApi.csproj reference TrackerJuegos.Application/TrackerJuegos.Application.csproj`

### ¿Para que refinir referencias? -> La regla de Dependencia.
Con el comando `dotnet add ... reference ... ` le decimos al compilador: "Permite que el Proyecto A pueda ver y usar las clases que están dentro del Proyecto B".
Es decir:
* Aplicación -> Dominio: Le decimos a la Aplicación que mire al Dominio, la Aplicación necesita conocer la clase `Game` para poder crear casos de uso (ej, "Guardar Juego"). Pero el Dominio no tiene referencia a la Aplicación.
* Infraestructura -> Aplicación: La infraestructura mira a la aplicación, porque en la capa de Aplicación/Dominio definimos la interfaz de `IGameRepository`. La infraestructura necesita ver esa interfaz para poder decir: "Ah, yo (SQLite) voy a cumplir con este contrato".
* Infraestructura -> Dominio: La infraestructura mira al Dominio porque es la encargada de guardar o recuperar datos. Aunque la base de datos SQLite y las peticiones de red (APIs) son detalles externos, necesitan saber qué es lo que están manejando. Para  configurar la base de datos con Entity Framework (ApplicatiónDbContext), la infraestructura necesita ver las entidades puras como TrackingRecord para poder decirle al motor SQL: "Toma las propiedades de esta clase y mapea sus columnas". Ademas, cuando el repositorio consulte la base de datos o la API, debe construir y volver un objeto puro del Dominio.
* WebAPI -> Infraestructura/Aplicación -> La WebAPI (controladores, etc) es la capa externa. Necesita ver la aplicación para poder llamar a los casos de uso cuando llega una petición HTTP. Y necesita ver la Infraestructura solo en el momento del arranque (Program.cs) para poder "conectar" los cables y decirle al sistema: "Cuando alguien pida un `IGameRepository` entregale la base de datos SQLite".

### ¿Por qué esto es Clean Architecture puro?

Al principio, puede parecer contraintuitivo que la Infraestructura se salte la capa de Aplicación y mire directamente al Dominio. Sin embargo, si revisas el diagrama clásico de círculos concéntricos de Clean Architecture (creado por Robert C. Martin), la regla principal dice: "El código fuente de las capas exteriores puede depender del código fuente de las capas interiores".

    El Dominio es el centro (Círculo 0).

    La Aplicación es el anillo interno (Círculo 1).

    La Infraestructura es el anillo externo (Círculo 2).

Por lo tanto, que el Círculo 2 mire al Círculo 0 es un movimiento válido "hacia adentro". Lo que está estrictamente prohibido es que el Círculo 0 mire al Círculo 2 (que tus entidades puras dependan de Entity Framework).

#### ¿Por qué esto evita el efecto "Zig-Zag" o dependencia circular?
1. Imaginar el caso, tenemos un caso de uso que guarda un juego en la base de datos (Aplicación llama a Infraestructura).
2. Pero la base de datos falla y decide lanzar un evento de error de negocio (Infraestructura llama a aplicación).

Si ambos proyectos se referencian mutuamente, se crea un bucle infinito. El compilador es estricto, si detecta una dependencia circular, lanza un error fatal y bloquea la compilación.
Al configurar estas referencias manualmente desde la terminal mediante proyectos separados, estamos uando al propio compilador como nuestro control de calidad. Si en el futuro, por error, intentas importar tu base de datos SQLite, dentro de las reglas puras del Dominio, el IDE marcará un error rojo yel proyecto no compilará.

### Arquitectura Limpia

**WebApi** -> Puntos de entrada, controladores, DI.  
**Infraestructura** -> Persistencia, servicios externos.  
**Aplicación** -> Casos de uso, logica de negocio.  
**Dominio** -> Entidades, interfaces, reglas core.  
