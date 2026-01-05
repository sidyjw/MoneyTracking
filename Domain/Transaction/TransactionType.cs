namespace Domain.ValueObjects;

public record TransactionType
{
    public static readonly TransactionType Income = new(TransactionTypesEnum.Income);
    public static readonly TransactionType Expense = new(TransactionTypesEnum.Expense);

    public TransactionTypesEnum Value { get; private set; }

    private TransactionType(TransactionTypesEnum value)
    {
        Value = value;
    }
}

public enum TransactionTypesEnum
{
    Income,
    Expense
}