namespace TrackerJuegos.Domain.Entities;

public class Game
{   
    // El id único interno de nuestro sistema (UUID/GUID)
    public Guid Id {get; private set; }

    public int RawgID {get; private set; } // ID de RAWG.
    public string Title {get; private set; }
    public string Description {get; private set; }
    public DateTime? ReleasedAt {get; private set; }
    public string BackgroundImage {get; private set; }
    public List<string> Plataforms {get; private set; }

    #pragma warning disable CS8618
    private Game(){ }
    #pragma warning disable CS8618

    //Constructor de negocio que asegura que la entidad siempre se cree en un estado válido.
    public Game(Guid id, int rawgid, string title, string description, DateTime? releasedAt, string backgroundImage, List <string> plataforms)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("El título del juego no puede estar vacío.");

        Id = id;
        RawgID = rawgid;
        Title = title;
        Description = description;
        ReleasedAt = releasedAt;
        BackgroundImage = backgroundImage;
        Plataforms = plataforms;
    }
}