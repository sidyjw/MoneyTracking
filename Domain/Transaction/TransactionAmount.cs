namespace Domain.ValueObjects;

public record TransactionAmount
{
    public decimal Value { get; }

    private TransactionAmount(decimal value)
    {
        Value = value;
    }

    public static ResultT<TransactionAmount> Create(decimal value)
    {
        if (value == 0)
            return Error.Validation(TransactionErrors.InvalidAmount, "O valor da transação não pode ser zero.");

        return new TransactionAmount(value);
    }

    public static implicit operator decimal(TransactionAmount amount) => amount.Value;
}
