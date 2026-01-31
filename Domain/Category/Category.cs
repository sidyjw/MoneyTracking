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

    public static ResultT<Category> Create(CategoryName name, CategoryType type)
    {
        return new Category(
            Guid.NewGuid(),
            name,
            type
        );
    }

    public ResultT<Category> UpdateName(CategoryName newName)
    {
        if (newName == _name)
            return Error.Validation(CategoryErrors.NameUnchanged, "O novo nome deve ser diferente do nome atual.");

        _name = newName;
        UpdateTimestamp();

        return this;
    }

    public ResultT<Category> UpdateType(CategoryType newType)
    {
        if (newType == _type)
            return Error.Validation(CategoryErrors.TypeUnchanged, "O novo tipo deve ser diferente do tipo atual.");

        _type = newType;
        UpdateTimestamp();

        return this;
    }

    public ResultT<Category> SetMonthlyBudget(MonthlyBudget monthlyBudget)
    {
        if (monthlyBudget is null)
            return Error.Validation(CategoryErrors.MonthlyBudgetNull, "O orçamento mensal não pode ser nulo.");

        _monthlyBudget = monthlyBudget;
        UpdateTimestamp();

        return this;
    }

    public ResultT<Category> RemoveMonthlyBudget()
    {
        if (_monthlyBudget is null)
            AddError(Error.Validation(CategoryErrors.MonthlyBudgetNull, "O orçamento mensal já está nulo."));

        if (HasValidationErrors())
            return GetValidationErrors();

        _monthlyBudget = null;
        UpdateTimestamp();

        return this;
    }
}
