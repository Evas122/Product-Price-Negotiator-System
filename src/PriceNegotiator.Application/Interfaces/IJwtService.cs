using PriceNegotiator.Domain.Entities.Auth;

namespace PriceNegotiator.Application.Interfaces;

public interface IJwtService
{
    string GenerateJwtToken(User user);
}