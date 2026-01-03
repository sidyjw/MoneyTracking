namespace Domain;

public sealed class User : Entity
{
    public UserFullName Name { get => _name; private set => _name = value; }
    private UserFullName _name;

    public Email Email { get => _email; private set => _email = value; }
    private Email _email;

    public IReadOnlyList<Account> Accounts => _accounts.AsReadOnly();
    private List<Account> _accounts = [];

    public IReadOnlyList<Category> Categories => _categories.AsReadOnly();
    private List<Category> _categories = [];

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

    public User UpdateName(string newName)
    {
        if (newName == _name.Full)
            throw new ArgumentException("O novo nome deve ser diferente do nome atual.", nameof(newName));

        _name = new UserFullName(newName);
        UpdateTimestamp();

        return this;
    }

    public User UpdateEmail(string newEmail)
    {
        if (newEmail == _email.Value)
            throw new ArgumentException("O novo email deve ser diferente do email atual.", nameof(newEmail));

        _email = new Email(newEmail);
        UpdateTimestamp();

        return this;
    }

    public User AddCategory(Category newCategory)
    {
        ArgumentNullException.ThrowIfNull(newCategory);

        if (_categories.Contains(newCategory))
            throw new ArgumentException("A categoria já foi adicionada ao usuário.", nameof(newCategory));

        if (_categories.Any(c => c.Name == newCategory.Name && c.Type == newCategory.Type))
            throw new ArgumentException($"Já existe uma categoria com esse nome ({nameof(newCategory.Name)}) e tipo ({newCategory.Type.Value})", nameof(newCategory));

        _categories.Add(newCategory);
        UpdateTimestamp();

        return this;
    }

    public User RemoveCategory(Category categoryToRemove)
    {
        ArgumentNullException.ThrowIfNull(categoryToRemove);

        if (!_categories.Contains(categoryToRemove))
            throw new ArgumentException("A categoria não foi encontrada no usuário.", nameof(categoryToRemove));

        _categories.Remove(categoryToRemove);
        UpdateTimestamp();

        return this;
    }

    public User AddAccount(Account newAccount)
    {
        ArgumentNullException.ThrowIfNull(newAccount);

        if (_accounts.Contains(newAccount))
            throw new ArgumentException("A conta já foi adicionada ao usuário.", nameof(newAccount));

        if (_accounts.Any(a => a.Name == newAccount.Name && a.Type == newAccount.Type))
            throw new ArgumentException($"Já existe uma conta com esse nome ({nameof(newAccount.Name)}) e tipo ({newAccount.Type.Value})", nameof(newAccount));

        _accounts.Add(newAccount);
        UpdateTimestamp();

        return this;
    }
}