using PriceNegotiator.Domain.Entities.Auth;

namespace PriceNegotiator.Application.Common.Interfaces;

public interface IJwtService
{
    string GenerateJwtToken(User user);
}