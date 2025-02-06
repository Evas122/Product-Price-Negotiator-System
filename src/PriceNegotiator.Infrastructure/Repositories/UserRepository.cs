using Microsoft.EntityFrameworkCore;
using PriceNegotiator.Domain.Entities.Auth;
using PriceNegotiator.Domain.Repositories;
using PriceNegotiator.Infrastructure.Data;

namespace PriceNegotiator.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;

    public UserRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(User user)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(User => User.Email == email);
    }

    public async Task<bool> IsEmailExistAsync(string email)
    {
        return await _dbContext.Users.AnyAsync(User => User.Email == email);
    }
}