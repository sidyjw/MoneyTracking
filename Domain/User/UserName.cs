namespace Domain;

public record UserFullName
{
    public string FirstName { get; }
    public string LastName { get; }
    public string Full => $"{FirstName} {LastName}";

    public UserFullName(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new ArgumentException("Name cannot be null or empty.", nameof(fullName));

        var nameParts = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (nameParts.Length < 2)
            throw new ArgumentException("Name must contain at least first name and last name.", nameof(fullName));

        if (nameParts.Any(part => part.Length < 2))
            throw new ArgumentException("Each part of the name must have at least 2 characters.", nameof(fullName));

        if (nameParts.Any(part => !part.All(c => char.IsLetter(c) || char.IsWhiteSpace(c))))
            throw new ArgumentException("Name must contain only letters.", nameof(fullName));

        FirstName = CapitalizeName(nameParts[0]);
        LastName = CapitalizeName(string.Join(" ", nameParts.Skip(1)));
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