using Microsoft.EntityFrameworkCore;
using PriceNegotiator.Domain.Entities.Auth;
using PriceNegotiator.Domain.Repositories;
using PriceNegotiator.Infrastructure.Data;

namespace PriceNegotiator.Infrastructure.Services;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddUserAsync(User user)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(User => User.Email == email);
    }
}