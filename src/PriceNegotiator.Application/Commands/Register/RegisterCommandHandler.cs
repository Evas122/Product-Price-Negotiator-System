using PriceNegotiator.Application.Dtos;
using PriceNegotiator.Application.Interfaces;
using PriceNegotiator.Application.Interfaces.Messaging;
using PriceNegotiator.Domain.Entities.Auth;
using PriceNegotiator.Domain.Enums;
using PriceNegotiator.Domain.Repositories;

namespace PriceNegotiator.Application.Commands.Register;

public record RegisterCommand(string Email, string Password, string FirstName, string LastName) : ICommand<AuthResultDto>;

public class RegisterCommandHandler : ICommandHandler<RegisterCommand, AuthResultDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IIdentityService _identityService;
    private readonly IJwtService _jwtService;

    public RegisterCommandHandler(IUserRepository userRepository, IIdentityService identityService, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _identityService = identityService;
        _jwtService = jwtService;
    }

    public async Task<AuthResultDto> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var isEmailUnique = await _identityService.IsEmailUniqueAsync(command.Email);
        if (!isEmailUnique)
        {
            throw new Exception("Email is already taken");
        }
        var user = new User
        {
            Email = command.Email,
            FirstName = command.FirstName,
            LastName = command.LastName,
            Role = UserRole.Employee
        };
        var hashedPassword = await _identityService.HashPassword(user, command.Password);
        user.PasswordHash = hashedPassword;
        await _userRepository.AddAsync(user);
        var token = _jwtService.GenerateJwtToken(user);

        return new AuthResultDto(token);
    }
}