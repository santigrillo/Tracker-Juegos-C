using TrackerJuegos.Domain.Enums;

namespace TrackerJuegos.Domain.Entities;

public class TrackingRecord
{
    public Guid Id {get; private set; }
    public Guid UserId {get; private set; }
    public Guid GameId {get; private set; }
    public TrackingStatus Status {get; private set; }
    public int? PersonalRating {get; private set; }
    public string Notes {get; private set; }
    public DateTime UpdatedAt {get; private set; }


    #pragma warning disable CS8618
    private TrackingRecord() { }
    #pragma warning disable CS8618

    public TrackingRecord(Guid id, Guid userId, Guid gameId, TrackingStatus status)
    {
        Id = id;
        UserId = userId;
        GameId = gameId;
        Status = status;
        Notes = string.Empty;
        UpdatedAt = DateTime.UtcNow;
    }

    // Encapsulación de la regla de negocio: Controlamos los cambios de estados internamente.
    public void UpdateProgress(TrackingStatus newStatus, string notes, int? rating)
    {
        if (newStatus == TrackingStatus.Pending && rating.HasValue)
            throw new InvalidOperationException ("No se puede asignar una calificación a un juego en estado pendiente.");

        if (rating.HasValue && (rating.Value < 1 || rating.Value > 10))
            throw new ArgumentOutOfRangeException(nameof(rating), "La calificación debe estar entre 1 y 10.");

        Status = newStatus;
        Notes = notes ?? string.Empty;
        PersonalRating = rating;
        UpdatedAt = DateTime.UtcNow;
    }


}