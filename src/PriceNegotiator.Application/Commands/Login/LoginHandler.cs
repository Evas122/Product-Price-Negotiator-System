using PriceNegotiator.Application.Common.Exceptions.Base;
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
        var user = await _userRepository.GetByEmailAsync(command.Email);
        if (user == null)
        {
            throw new NotFoundException(nameof(user), command.Email);
        }

        var passwordVerificationResult = await _identityService.VerifyPassword(user, command.Password, user.PasswordHash);
        if (!passwordVerificationResult)
        {
            throw new InvalidCredentialsException();
        }

        var token = _jwtService.GenerateJwtToken(user);

        return new AuthResultDto(token);
    }
}