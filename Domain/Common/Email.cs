using System.Text.RegularExpressions;

namespace Domain.ValueObjects;

public record Email
{
    private static readonly Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase
    );

    public string Value { get; }

    private Email(string value)
    {
        Value = value.Trim().ToLowerInvariant();
    }

    public static ResultT<Email> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Error.Validation(EmailErrors.EmptyOrNull, "O email não pode ser vazio ou nulo.");

        if (!EmailRegex.IsMatch(value))
            return Error.Validation(EmailErrors.InvalidFormat, $"O email '{value}' não é válido.");

        return new Email(value);
    }

    public override string ToString() => Value;

    public static implicit operator string(Email email) => email.Value;
}