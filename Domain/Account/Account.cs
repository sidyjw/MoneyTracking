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

    internal Account(Guid id, AccountName name, AccountType type, Balance balance, HashSet<Transaction>? transactions = default) : base(id)
    {
        Id = id;
        _name = name;
        _type = type;
        _balance = balance;
        _transactions = transactions;
    }

    public static ResultT<Account> Create(string name, AccountTypeEnum type, decimal balance, HashSet<Transaction>? transactions = default)
    {
        var nameResult = AccountName.Create(name);
        if (nameResult.IsFailure)
            return nameResult.Errors!;

        return new Account(
            Guid.NewGuid(),
            nameResult.Value,
            type,
            balance,
            transactions ?? []
        );
    }

    public ResultT<Account> UpdateName(AccountName newName)
    {
        if (newName.Value == _name.Value)
            return Error.Validation(AccountErrors.NameUnchanged, "O novo nome deve ser diferente do nome atual.");

        _name = newName;
        UpdateTimestamp();

        return this;
    }

    public ResultT<Account> UpdateType(AccountType newType)
    {
        if (newType == _type)
            return Error.Validation(AccountErrors.TypeUnchanged, "O novo tipo deve ser diferente do tipo atual.");

        _type = newType;
        UpdateTimestamp();

        return this;
    }

    public ResultT<Account> Credit(Transaction newTransaction)
    {
        if (newTransaction is null)
            AddError(Error.Validation(AccountErrors.TransactionNull, "A transação não pode ser nula."));

        if (newTransaction.Type != TransactionType.Income)
            AddError(Error.Validation(AccountErrors.InvalidTransactionType, "A transação deve ser do tipo crédito."));

        var creditResult = _balance.Credit(newTransaction.Amount.Value);
        if (creditResult.IsFailure)
            AddErrors(creditResult.Errors!);

        if (HasValidationErrors())
            return GetValidationErrors();

        _transactions?.Add(newTransaction);
        UpdateTimestamp();
        return this;
    }

    public ResultT<Account> Debit(Transaction newTransaction)
    {
        if (newTransaction is null)
            AddError(Error.Validation(AccountErrors.TransactionNull, "A transação não pode ser nula."));

        if (newTransaction.Type != TransactionType.Expense)
            AddError(Error.Validation(AccountErrors.InvalidTransactionType, "A transação deve ser do tipo débito."));

        var debitResult = _balance.Debit(newTransaction.Amount.Value);
        if (debitResult.IsFailure)
            AddErrors(debitResult.Errors!);

        if (HasValidationErrors())
            return GetValidationErrors();

        _transactions?.Add(newTransaction);
        UpdateTimestamp();
        return this;
    }
}
