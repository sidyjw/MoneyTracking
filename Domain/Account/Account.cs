namespace Domain.Entities;

public sealed class Account : Entity
{
    public AccountName Name { get => _name; }
    private AccountName _name;

    public AccountType Type { get => _type; }
    private AccountType _type;

    public Balance Balance { get => _balance; }
    private Balance _balance;

    public IReadOnlySet<Transaction>? Transactions => _transactions?.AsReadOnly();
    private HashSet<Transaction>? _transactions = [];

    public bool IsActive { get; } = true;

    internal Account(Guid id, AccountName name, AccountType type, Balance balance, HashSet<Transaction>? transactions) : base(id)
    {
        Id = id;
        _name = name;
        _type = type;
        _balance = balance;
        _transactions = transactions;
    }

    public static Account Create(AccountName name, AccountType type, Balance balance, HashSet<Transaction>? transactions)
    {
        return new Account(
            Guid.NewGuid(),
            name,
            type,
            balance,
            transactions
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

    public void Credit(Transaction newTransaction)
    {
        if (newTransaction.Type != TransactionType.Income)
            throw new ArgumentException("A transação deve ser do tipo crédito.", nameof(newTransaction));

        _transactions?.Add(newTransaction);
        _balance += newTransaction.Amount.Value;
        UpdateTimestamp();
    }

    public void Debit(Transaction newTransaction)
    {
        if (newTransaction.Type != TransactionType.Expense)
            throw new ArgumentException("A transação deve ser do tipo débito.", nameof(newTransaction));

        _transactions?.Add(newTransaction);
        _balance -= newTransaction.Amount.Value;
        UpdateTimestamp();
    }
}
