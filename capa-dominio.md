## Capa de dominio.
Creamos una carpeta para las entidades y otra para los enumeradores

`mkdir -p TrackerJuegos.Domain/Entities`
`mkdir -p TrackerJuegos.Domain/Enums`

El Beneficio a Largo Plazo (Clean Code) de esta solución.

Agrupar elementos por su naturaleza evita el antipatrón de la "God Folder" (Carpeta Dios). Si se me entidades, enumeradores, interfaces de contratos y excepciones personalizadas en el directorio raíz del Dominio, cuando el sistema escale a 40 entidades y 20 enums, el costo cognitivo de buscar un archivo y comprender el mapa mental del proyecto sería altísimo. Separar responsabilidades no solo aplica al código, sino a la arquitectura de archivos.

## Entidades y enumeradores.

Se creó el enumerador TrackingStatus con los posibles valores que puede obtener el estado de un juego -> [pending, abandoned, completed, playing].

Se creó las entidades Game con la información que trae RAWG.
Se creó la entidad TrackingRecord que almacena la información acerca del juego trackeado, incluyendo implementación de lógica de negocio al controlar los cambios de estado, incluyendo manejo de excepciones.


