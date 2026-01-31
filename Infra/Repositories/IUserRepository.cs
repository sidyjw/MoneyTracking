namespace Infra.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(Domain.ValueObjects.Email email);
    Task<User> AddAsync(User user);
}