namespace Domain.ValueObjects;

public record Balance
{
    public decimal Amount { get; }

    private Balance(decimal amount)
    {
        Amount = amount;
    }

    public static ResultT<Balance> Create(decimal amount)
    {
        return new Balance(amount);
    }

    public ResultT<Balance> Credit(decimal amount)
    {
        if (amount <= 0)
            return Error.Validation(BalanceErrors.InvalidCreditAmount, "O valor de crédito deve ser positivo.");

        return new Balance(Amount + amount);
    }


    public ResultT<Balance> Debit(decimal amount)
    {
        if (amount >= 0)
            return Error.Validation(BalanceErrors.InvalidDebitAmount, "O valor de débito deve ser negativo.");

        return new Balance(Amount - amount);
    }

    public static implicit operator decimal(Balance balance) => balance.Amount;

    public static implicit operator Balance(decimal amount) => new(amount);
}
