namespace Features;

public partial class UserFeature
{
    private readonly IUserRepository _userRepository;

    public UserFeature(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public static Error CreateUserUserAlreadyExists(string email) =>
        Error.Conflict(email, "Usuário já existe");
}
