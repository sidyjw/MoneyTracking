namespace Domain;

public record AccountName
{
    public string Value { get; }

    public AccountName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("O nome da conta não pode ser vazio ou nulo.", nameof(value));

        if (value.Length < 3 || value.Length > 50)
            throw new ArgumentException("O nome da conta deve ter entre 3 e 50 caracteres.", nameof(value));

        Value = value.Trim();
    }

    public override string ToString() => Value;

    public static implicit operator string(AccountName accountName) => accountName.Value;
}