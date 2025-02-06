using PriceNegotiator.Domain.Entities.Auth;

namespace PriceNegotiator.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<bool> IsEmailUniqueAsync(string email);
    Task<string> HashPassword(User user, string password);
    Task<bool> VerifyPassword(User user, string password, string hashedPassword);
}