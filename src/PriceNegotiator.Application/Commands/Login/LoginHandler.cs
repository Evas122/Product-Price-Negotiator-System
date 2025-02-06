using PriceNegotiator.Application.Dtos.Auth;
using PriceNegotiator.Application.Interfaces;
using PriceNegotiator.Application.Interfaces.Messaging;
using PriceNegotiator.Domain.Repositories;

namespace PriceNegotiator.Application.Commands.Login;

public record LoginCommand(string Email, string Password) : ICommand<AuthResultDto>;
public class LoginHandler : ICommandHandler<LoginCommand, AuthResultDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IIdentityService _identityService;
    private readonly IJwtService _jwtService;

    public LoginHandler(IUserRepository userRepository, IIdentityService identityService, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _identityService = identityService;
        _jwtService = jwtService;
    }

    public async Task<AuthResultDto> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmailAsync(command.Email) ?? throw new ApplicationException("User not found");

        var passwordVerificationResult = await _identityService.VerifyPassword(user, command.Password, user.PasswordHash);
        if (!passwordVerificationResult)
        {
            throw new ApplicationException("Invalid password");
        }

        var token = _jwtService.GenerateJwtToken(user);

        return new AuthResultDto(token);
    }
}