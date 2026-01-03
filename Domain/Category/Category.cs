namespace Domain;

public sealed class Category : Entity
{
    public CategoryName Name { get => _name; }
    private CategoryName _name;

    public CategoryType Type { get => _type; }
    private CategoryType _type;

    //TODO: Icone da categoria

    //TODO: Cor da categoria

    public bool IsActive { get; } = true;

    public User User { get => _user; }
    private readonly User _user;

    internal Category(Guid id, CategoryName name, CategoryType type, User user) : base(id)
    {
        Id = id;
        _name = name;
        _type = type;
        _user = user;
    }

    public static Category Create(User user, CategoryName name, CategoryType type)
    {
        return new Category(
            Guid.NewGuid(),
            name,
            type,
            user
        );
    }

    public Category UpdateName(CategoryName newName)
    {
        if (newName == _name)
            throw new ArgumentException("O novo nome deve ser diferente do nome atual.", nameof(newName));

        _name = newName;
        UpdateTimestamp();

        return this;
    }

    public Category UpdateType(CategoryType newType)
    {
        if (newType == _type)
            throw new ArgumentException("O novo tipo deve ser diferente do tipo atual.", nameof(newType));

        _type = newType;
        UpdateTimestamp();

        return this;
    }
}
