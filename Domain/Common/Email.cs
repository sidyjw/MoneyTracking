using System.Text.RegularExpressions;

namespace Domain.Common;

public record Email
{
    private static readonly Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase
    );

    public string Value { get; }

    public Email(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("O email não pode ser vazio ou nulo.", nameof(value));

        if (!EmailRegex.IsMatch(value))
            throw new ArgumentException($"O email '{value}' não é válido.", nameof(value));

        Value = value.Trim().ToLowerInvariant();
    }

    public override string ToString() => Value;

    public static implicit operator string(Email email) => email.Value;
}