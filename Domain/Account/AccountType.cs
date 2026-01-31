namespace Domain.ValueObjects;

public record AccountType
{
    public static readonly AccountType Checking = new(AccountTypeEnum.Checking);
    public static readonly AccountType Savings = new(AccountTypeEnum.Savings);

    public AccountTypeEnum Value { get; }
    public AccountType(AccountTypeEnum value)
    {
        Value = value;
    }

    public override string ToString() => Value.ToString();
    public static implicit operator AccountTypeEnum(AccountType accountType) => accountType.Value;
    public static implicit operator AccountType(AccountTypeEnum value) => new(value);
}

public enum AccountTypeEnum
{
    Checking,
    Savings
}