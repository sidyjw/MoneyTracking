namespace Domain.Entities;

public sealed class MonthlyBudget : Entity
{
    public decimal Amount { get => _amount; private set => _amount = value; }
    private decimal _amount;

    public MonthYear MonthYear { get => _monthYear; private set => _monthYear = value; }
    private MonthYear _monthYear;

    public Category Category { get => _category; private set => _category = value; }
    private Category _category;

    public User User => _category.User;

    internal MonthlyBudget(Guid id, decimal amount, MonthYear monthYear, Category category) : base(id)
    {
        Id = id;
        _amount = amount;
        _monthYear = monthYear;
        _category = category;
    }

    public static MonthlyBudget Create(decimal amount, MonthYear monthYear, Category category)
    {
        return new MonthlyBudget(
            Guid.NewGuid(),
            amount,
            monthYear,
            category
        );
    }
}
