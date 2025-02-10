using PriceNegotiator.Domain.Entities.Auth;

namespace PriceNegotiator.Domain.Interfaces;

public interface IJwtService
{
    string GenerateJwtToken(User user);
}