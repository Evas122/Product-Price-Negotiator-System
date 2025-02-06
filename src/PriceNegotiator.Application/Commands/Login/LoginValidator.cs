﻿using FluentValidation;

namespace PriceNegotiator.Application.Commands.Login;

public class LoginValidator : AbstractValidator<LoginCommand>
{
    public LoginValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Email is required");
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required");
    }
}