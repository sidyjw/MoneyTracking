namespace Domain.Common;

public abstract class Entity : IEquatable<Entity>
{
    public Guid Id { get; init; }

    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.UtcNow;
    public DateTimeOffset? UpdatedAt { get; set; }

    protected Entity(Guid id)
    {
        this.Id = id;
    }

    protected virtual void UpdateTimestamp()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
    }

    // Igualdade por identidade (Id)
    public override bool Equals(object? obj)
    {
        if (obj is not Entity other) return false;
        if (ReferenceEquals(this, other)) return true;
        if (GetType() != other.GetType()) return false;

        return Id == other.Id;
    }

    public override int GetHashCode() => Id.GetHashCode();

    public bool Equals(Entity? other)
    {
        return Equals((object?)other);
    }

    public static bool operator ==(Entity? left, Entity? right)
    {
        if (left is null) return right is null;
        return left.Equals(right);
    }

    public static bool operator !=(Entity? left, Entity? right) => !(left == right);
}