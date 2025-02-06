using PriceNegotiator.Domain.Entities.Auth;

namespace PriceNegotiator.Domain.Repositories;

public interface IUserRepository
{
    Task AddUserAsync(User user);
    Task<User?> GetUserByEmailAsync(string email);
}