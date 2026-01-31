namespace Infra.Repositories;

public interface IAccountRepository
{
    Task<Account?> GetByIdAsync(Guid id);
    Task<Account> AddAsync(Account account, Guid userId);
}