namespace Domain.ValueObjects;

public record CategoryName
{
    public string Value { get; }

    private CategoryName(string value)
    {
        Value = value.Trim();
    }

    public static ResultT<CategoryName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Error.Validation(CategoryNameErrors.EmptyOrNull, "O nome da categoria não pode ser vazio ou nulo.");

        if (value.Length < 3 || value.Length > 50)
            return Error.Validation(CategoryNameErrors.InvalidLength, "O nome da categoria deve ter entre 3 e 50 caracteres.");

        return new CategoryName(value);
    }

    public override string ToString() => Value;

    public static implicit operator string(CategoryName categoryName) => categoryName.Value;
}