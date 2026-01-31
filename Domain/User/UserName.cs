namespace Domain.ValueObjects;

public record UserFullName
{
    public string FirstName { get; }
    public string LastName { get; }
    public string Full => $"{FirstName} {LastName}";

    private UserFullName(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public static ResultT<UserFullName> Create(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            return Error.Validation(UserFullNameErrors.EmptyOrNull, "Name cannot be null or empty.");

        var nameParts = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (nameParts.Length < 2)
            return Error.Validation(UserFullNameErrors.TooShort, "Name must contain at least first name and last name.");

        if (nameParts.Any(part => part.Length < 2))
            return Error.Validation(UserFullNameErrors.InvalidLength, "Each part of the name must have at least 2 characters.");

        if (nameParts.Any(part => !part.All(c => char.IsLetter(c) || char.IsWhiteSpace(c))))
            return Error.Validation(UserFullNameErrors.InvalidCharacters, "Name must contain only letters.");

        var firstName = CapitalizeName(nameParts[0]);
        var lastName = CapitalizeName(string.Join(" ", nameParts.Skip(1)));

        return new UserFullName(firstName, lastName);
    }

    private static string CapitalizeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return name;

        return string.Join(" ", name.Split(' ')
            .Select(word => char.ToUpper(word[0]) + word[1..].ToLower()));
    }

    public override string ToString() => Full;

    public static implicit operator string(UserFullName name) => name.Full;
}