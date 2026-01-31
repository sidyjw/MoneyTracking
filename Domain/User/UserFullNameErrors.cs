namespace Domain.ValueObjects;

public static class UserFullNameErrors
{
    public const string EmptyOrNull = "UserFullName.EmptyOrNull";
    public const string TooShort = "UserFullName.TooShort";
    public const string InvalidLength = "UserFullName.InvalidLength";
    public const string InvalidCharacters = "UserFullName.InvalidCharacters";
}
