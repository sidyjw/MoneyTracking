namespace Domain;

public sealed class Transaction : Entity
{
    public decimal Amount { get => _amount; private set => _amount = value; }
    private decimal _amount;

    public TransactionType Type
    {
        get => _type;
        private set => _type = value;
    }
    private TransactionType _type;

    public DateTimeOffset Date { get => _date; private set => _date = value; }
    private DateTimeOffset _date;

    public string Description { get => _description; private set => _description = value; }
    private string _description;

    public Account Account { get => _account; private set => _account = value; }
    private Account _account;

    public Category Category { get => _category; private set => _category = value; }
    private Category _category;

    public User User => _account.User;

    internal Transaction(Guid id, decimal amount, DateTimeOffset date, string description, Account account, Category category, TransactionType type) : base(id)
    {
        Id = id;
        _amount = amount;
        _date = date;
        _description = description;
        _type = type;
        _account = account;
        _category = category;
    }

    public static Transaction Create(decimal amount, TransactionType type, DateTimeOffset date, string description, Account account, Category category)
    {
        if (type == TransactionType.Income && amount <= 0)
            throw new ArgumentException("O valor de crédito deve ser positivo.", nameof(amount));

        if (type == TransactionType.Expense && amount >= 0)
            throw new ArgumentException("O valor de débito deve ser negativo.", nameof(amount));

        return new Transaction(
            Guid.NewGuid(),
            amount,
            date,
            description,
            account,
            category,
            type
        );
    }
}
