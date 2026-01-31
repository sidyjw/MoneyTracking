namespace Features;

public record AddAccountRequest(Guid UserId, string AccountName, decimal InitialBalance, AccountTypeEnum AccountType);
public partial class AccountFeature
{
    public async Task<ResultT<AddAccountResult>> AddAccountAsync(AddAccountRequest request)
    {
        var (userId, accountName, initialBalance, accountType) = request;
        var existingUser = await _userRepository.GetByIdAsync(userId);

        if (existingUser is null)
        {
            return CreateUserNotFoundError(userId);
        }

        var accountResult = Account.Create(accountName, accountType, initialBalance);
        if (accountResult.IsFailure)
            return ResultT<AddAccountResult>.Failure(accountResult.Errors!);

        await _accountRepository.AddAsync(accountResult.Value, existingUser.Id);

        return new AddAccountResult(accountResult.Value.Id);
    }
}
public record AddAccountResult(Guid AccountId);