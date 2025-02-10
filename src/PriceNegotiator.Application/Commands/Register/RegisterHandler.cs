using PriceNegotiator.Application.Common.Exceptions.Base;
using PriceNegotiator.Application.Dtos.Auth;
using PriceNegotiator.Application.Interfaces;
using PriceNegotiator.Application.Interfaces.Messaging;
using PriceNegotiator.Domain.Entities.Auth;
using PriceNegotiator.Domain.Enums;
using PriceNegotiator.Domain.Repositories;

namespace PriceNegotiator.Application.Commands.Register;

public record RegisterCommand(string Email, string Password, string FirstName, string LastName) : ICommand<AuthResultDto>;

public class RegisterHandler : ICommandHandler<RegisterCommand, AuthResultDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IIdentityService _identityService;
    private readonly IJwtService _jwtService;
    private readonly IDateTimeProvider _dateTimeProvider;

    public RegisterHandler(IUserRepository userRepository, IIdentityService identityService, IJwtService jwtService, IDateTimeProvider dateTimeProvider)
    {
        _userRepository = userRepository;
        _identityService = identityService;
        _jwtService = jwtService;
        _dateTimeProvider = dateTimeProvider;
    }

    public async Task<AuthResultDto> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        var isEmailUnique = await _identityService.IsEmailUniqueAsync(command.Email);
        if (isEmailUnique)
        {
            var user = new User
            {
                Email = command.Email,
                FirstName = command.FirstName,
                LastName = command.LastName,
                Role = UserRole.Employee,
                CreatedAt = _dateTimeProvider.UtcNow,
                UpdatedAt = _dateTimeProvider.UtcNow,
            };
            var hashedPassword = await _identityService.HashPassword(user, command.Password);
            user.PasswordHash = hashedPassword;
            await _userRepository.AddAsync(user);
            var token = _jwtService.GenerateJwtToken(user);

            return new AuthResultDto(token);
        }
        throw new AlreadyExistsException(command.Email);
    }
}