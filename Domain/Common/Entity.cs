namespace Domain.Common;

public abstract class Entity
{
    public Guid Id { get; init; }

    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }

    protected virtual void UpdateTimestamp()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    protected Entity(Guid id)
    {
        this.Id = id;
    }
}