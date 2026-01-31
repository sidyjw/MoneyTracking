namespace Domain.Entities;

public sealed class User : Entity
{
    public UserFullName Name { get => _name; private set => _name = value; }
    private UserFullName _name;

    public Email Email { get => _email; private set => _email = value; }
    private Email _email;

    public IReadOnlySet<Account> Accounts => _accounts.AsReadOnly();
    private HashSet<Account> _accounts = [];

    public IReadOnlySet<Category> Categories => _categories.AsReadOnly();
    private HashSet<Category> _categories = [];

    internal User(Guid id, UserFullName name, Email email) : base(id)
    {
        Id = id;
        _name = name;
        _email = email;
    }

    public static ResultT<User> Create(string name, string email)
    {
        var nameResult = UserFullName.Create(name);
        var emailResult = Email.Create(email);

        if (nameResult.IsFailure || emailResult.IsFailure)
        {
            var errors = new List<Error>();
            if (nameResult.IsFailure) errors.AddRange(nameResult.Errors!);
            if (emailResult.IsFailure) errors.AddRange(emailResult.Errors!);
            return ResultT<User>.Failure(errors);
        }

        return new User(
            Guid.NewGuid(),
            nameResult.Value,
            emailResult.Value
        );
    }

    public ResultT<User> UpdateName(string newName)
    {
        if (newName == _name.Full)
            AddError(Error.Validation(UserErrors.NameUnchanged, "O novo nome deve ser diferente do nome atual."));

        var nameResult = UserFullName.Create(newName);
        if (nameResult.IsFailure)
            AddErrors(nameResult.Errors!);

        if (HasValidationErrors())
            return GetValidationErrors();

        _name = nameResult.Value;
        UpdateTimestamp();

        return this;
    }

    public ResultT<User> UpdateEmail(string newEmail)
    {
        if (newEmail == _email.Value)
            AddError(Error.Validation(UserErrors.EmailUnchanged, "O novo email deve ser diferente do email atual."));

        var emailResult = Email.Create(newEmail);
        if (emailResult.IsFailure)
            AddErrors(emailResult.Errors!);

        if (HasValidationErrors())
            return GetValidationErrors();

        _email = emailResult.Value;
        UpdateTimestamp();

        return this;
    }

    public ResultT<User> AddCategory(Category newCategory)
    {
        if (newCategory is null)
            AddError(Error.Validation("User.CategoryNull", "A categoria não pode ser nula."));

        if (_categories.Any(c => c.Name == newCategory.Name && c.Type == newCategory.Type))
            AddError(Error.Conflict(UserErrors.CategoryAlreadyExists, $"Já existe uma categoria com esse nome ({newCategory.Name.Value}) e tipo ({newCategory.Type.Value})"));

        if (HasValidationErrors())
            return GetValidationErrors();

        _categories.Add(newCategory);
        UpdateTimestamp();

        return this;
    }

    public ResultT<User> RemoveCategory(Category categoryToRemove)
    {
        if (categoryToRemove is null)
            AddError(Error.Validation("User.CategoryNull", "A categoria não pode ser nula."));

        if (!_categories.Contains(categoryToRemove))
            AddError(Error.NotFound(UserErrors.CategoryNotFound, "A categoria não foi encontrada no usuário."));

        if (HasValidationErrors())
            return GetValidationErrors();

        _categories.Remove(categoryToRemove);
        UpdateTimestamp();

        return this;
    }

    public ResultT<User> AddAccount(Account newAccount)
    {
        if (newAccount is null)
            AddError(Error.Validation("User.AccountNull", "A conta não pode ser nula."));

        if (_accounts.Any(a => a.Name == newAccount.Name && a.Type == newAccount.Type))
            AddError(Error.Conflict(UserErrors.AccountAlreadyExists, $"Já existe uma conta com esse nome ({newAccount.Name.Value}) e tipo ({newAccount.Type.Value})"));

        if (HasValidationErrors())
            return GetValidationErrors();

        _accounts.Add(newAccount);
        UpdateTimestamp();

        return this;
    }
}