using PriceNegotiator.Domain.Common;
using PriceNegotiator.Domain.Enums;

namespace PriceNegotiator.Domain.Entities.Auth;

public class User : BaseEntity
{
    public string Email { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public UserRole Role { get; set; }
    public string PasswordHash { get; set; } = null!;
}