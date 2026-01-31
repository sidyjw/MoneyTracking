namespace Domain.ValueObjects;

public record AccountName
{
    public string Value { get; }

    private AccountName(string value)
    {
        Value = value.Trim();
    }

    public static ResultT<AccountName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Error.Validation(AccountNameErrors.EmptyOrNull, "O nome da conta não pode ser vazio ou nulo.");

        if (value.Length < 3 || value.Length > 50)
            return Error.Validation(AccountNameErrors.InvalidLength, "O nome da conta deve ter entre 3 e 50 caracteres.");

        return new AccountName(value);
    }

    public override string ToString() => Value;

    public static implicit operator string(AccountName accountName) => accountName.Value;
}