﻿using FluentValidation;

namespace PriceNegotiator.Application.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Email is required");
        RuleFor(x => x.Password)
             .NotEmpty().WithMessage("Password is required.")
             .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
             .Matches(@"\d").WithMessage("Password must contain at least one number.")
             .Matches(@"[\W_]").WithMessage("Password must contain at least one special character.")
             .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First Name is required");
        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last Name is required");
    }
}