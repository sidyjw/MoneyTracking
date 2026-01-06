namespace Domain.Entities;

public sealed class Category : Entity
{
    public CategoryName Name { get => _name; }
    private CategoryName _name;

    public CategoryType Type { get => _type; }
    private CategoryType _type;

    //TODO: Icone da categoria

    //TODO: Cor da categoria

    public bool IsActive { get; } = true;

    public MonthlyBudget? MonthlyBudget { get => _monthlyBudget; private set => _monthlyBudget = value; }
    private MonthlyBudget? _monthlyBudget;

    internal Category(Guid id, CategoryName name, CategoryType type) : base(id)
    {
        Id = id;
        _name = name;
        _type = type;
    }

    public static Category Create(CategoryName name, CategoryType type)
    {
        return new Category(
            Guid.NewGuid(),
            name,
            type
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

    public Category SetMonthlyBudget(MonthlyBudget monthlyBudget)
    {
        ArgumentNullException.ThrowIfNull(monthlyBudget);

        _monthlyBudget = monthlyBudget;
        UpdateTimestamp();

        return this;
    }

    public Category RemoveMonthlyBudget()
    {
        _monthlyBudget = null;
        UpdateTimestamp();

        return this;
    }
}
