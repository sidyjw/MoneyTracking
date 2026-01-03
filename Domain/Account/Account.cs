namespace Domain;

public sealed class Account : Entity
{
    public AccountName Name { get => _name; }
    private AccountName _name;

    public AccountType Type { get => _type; }
    private AccountType _type;

    public Balance Balance { get => _balance; }
    private Balance _balance;

    public User User { get => _user; }
    private readonly User _user;

    public bool IsActive { get; } = true;

    internal Account(Guid id, AccountName name, AccountType type, Balance balance, User user) : base(id)
    {
        Id = id;
        _name = name;
        _type = type;
        _balance = balance;
        _user = user;
    }

    public static Account Create(User user, AccountName name, AccountType type, Balance balance)
    {
        return new Account(
            Guid.NewGuid(),
            name,
            type,
            balance,
            user
        );
    }

    public Account UpdateName(AccountName newName)
    {
        if (newName.Value == _name.Value)
            throw new ArgumentException("O novo nome deve ser diferente do nome atual.", nameof(newName));

        _name = newName;
        UpdateTimestamp();

        return this;
    }

    public Account UpdateType(AccountType newType)
    {
        if (newType == _type)
            throw new ArgumentException("O novo tipo deve ser diferente do tipo atual.", nameof(newType));

        _type = newType;
        UpdateTimestamp();

        return this;
    }

    public void Credit(decimal newBalance)
    {
        _balance += newBalance;
        UpdateTimestamp();
    }

    public void Debit(decimal newBalance)
    {
        _balance -= newBalance;
        UpdateTimestamp();
    }
}
