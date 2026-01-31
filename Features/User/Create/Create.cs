namespace Features;

public record CreateUserRequest(string Name, string Email);

public partial class UserFeature
{
    public async Task<ResultT<CreateUserResult>> CreateUserAsync(CreateUserRequest request)
    {
        var emailResult = Email.Create(request.Email);
        if (emailResult.IsFailure)
            return emailResult.Errors!;

        var existingUser = await _userRepository.GetByEmailAsync(emailResult.Value);
        if (existingUser is not null)
            return CreateUserUserAlreadyExists(request.Email);

        var userResult = User.Create(request.Name, request.Email);
        if (userResult.IsFailure)
            return userResult.Errors!;

        await _userRepository.AddAsync(userResult.Value);

        return new CreateUserResult(userResult.Value.Id);
    }
}

public record CreateUserResult(Guid UserId);