namespace Features;

public partial class AccountFeature
{
    private readonly IUserRepository _userRepository;
    private readonly IAccountRepository _accountRepository;

    public AccountFeature(IUserRepository userRepository, IAccountRepository accountRepository)
    {
        _userRepository = userRepository;
        _accountRepository = accountRepository;
    }

    private static Error CreateUserNotFoundError(Guid userId) =>
        Error.NotFound(userId.ToString(), "Usuário não encontrado");
}