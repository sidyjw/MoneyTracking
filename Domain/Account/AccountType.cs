namespace Domain;

public record AccountType
{
    public static readonly AccountType Checking = new AccountType(AccountTypeEnum.Checking);
    public static readonly AccountType Savings = new AccountType(AccountTypeEnum.Savings);

    public AccountTypeEnum Value { get; }
    private AccountType(AccountTypeEnum value)
    {
        Value = value;
    }

    public override string ToString() => Value.ToString();
}

public enum AccountTypeEnum
{
    Checking,
    Savings
}