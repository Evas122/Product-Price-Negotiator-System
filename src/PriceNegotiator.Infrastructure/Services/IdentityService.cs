using Microsoft.AspNetCore.Identity;
using PriceNegotiator.Application.Interfaces;
using PriceNegotiator.Domain.Entities.Auth;
using PriceNegotiator.Domain.Repositories;

namespace PriceNegotiator.Infrastructure.Services;

public class IdentityService : IIdentityService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher<User> _passwordHasher;

    public IdentityService(IUserRepository userRepository, IPasswordHasher<User> passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<bool> IsEmailUniqueAsync(string email)
    {
       return await _userRepository.ExistsAsync(email);
    }

    public Task<string> HashPassword(User user, string password)
    {
        var HashedPassword = _passwordHasher.HashPassword(user, password);
        return Task.FromResult(HashedPassword);
    }

    public Task<bool> VerifyPassword(User user, string password, string hashedPassword)
    {
        var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, hashedPassword, password);
        return Task.FromResult(passwordVerificationResult == PasswordVerificationResult.Success);
    }
}