namespace Domain;

public sealed class User : Entity
{
    public UserFullName Name { get => _name; private set => _name = value; }
    private UserFullName _name;

    public Email Email { get => _email; private set => _email = value; }
    private Email _email;

    public IReadOnlyList<Account> Accounts => _accounts.AsReadOnly();
    private List<Account> _accounts = new();

    internal User(Guid id, UserFullName name, Email email) : base(id)
    {
        Id = id;
        _name = name;
        _email = new Email(email);
    }

    public static User Create(string name, string email)
    {
        return new User(
            Guid.NewGuid(),
            new UserFullName(name),
            new Email(email)
        );
    }

    public void UpdateName(string newName)
    {
        if (newName == _name.Full)
            throw new ArgumentException("O novo nome deve ser diferente do nome atual.", nameof(newName));

        _name = new UserFullName(newName);
        UpdateTimestamp();
    }

    public void UpdateEmail(string newEmail)
    {
        if (newEmail == _email.Value)
            throw new ArgumentException("O novo email deve ser diferente do email atual.", nameof(newEmail));

        _email = new Email(newEmail);
        UpdateTimestamp();
    }
}