namespace Domain;

public record CategoryName
{
    public string Value { get; } = string.Empty;
    public CategoryName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("O nome da categoria não pode ser vazio ou nulo.", nameof(value));

        if (value.Length < 3 || value.Length > 50)
            throw new ArgumentException("O nome da categoria deve ter entre 3 e 50 caracteres.", nameof(value));

        Value = value.Trim();
    }

    public override string ToString() => Value;

    public static implicit operator string(CategoryName categoryName) => categoryName.Value;
}