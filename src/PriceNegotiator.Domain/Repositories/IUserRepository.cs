using PriceNegotiator.Domain.Entities.Auth;

namespace PriceNegotiator.Domain.Repositories;

public interface IUserRepository
{
    Task AddAsync(User user);
    Task<User?> GetByEmailAsync(string email);
    Task<bool> ExistsAsync(string email);
}