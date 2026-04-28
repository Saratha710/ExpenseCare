using ExpenseCareApi.Core.DTOs;
using FluentValidation;

namespace ExpenseCareApi.Core.Validators.Auth;

public class RequestOtpValidator : AbstractValidator<RequestOtpDto>
{
    public RequestOtpValidator()
    {
        RuleFor(x => x.MobileNumber)
            .NotEmpty().WithMessage("Mobile number is required")
            .Matches(@"^\d{10}$").WithMessage("Enter a valid 10-digit mobile number");
    }
}

public class VerifyOtpValidator : AbstractValidator<VerifyOtpDto>
{
    public VerifyOtpValidator()
    {
        RuleFor(x => x.MobileNumber)
            .NotEmpty().WithMessage("Mobile number is required")
            .Matches(@"^\d{10}$").WithMessage("Enter a valid 10-digit mobile number");

        RuleFor(x => x.Otp)
            .NotEmpty().WithMessage("OTP is required")
            .Matches(@"^\d{4,6}$").WithMessage("OTP must be 4–6 digits");
    }
}

public class UserLoginValidator : AbstractValidator<UserLoginDto>
{
    public UserLoginValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("Username is required")
            .MinimumLength(3).WithMessage("Username must be at least 3 characters")
            .MaximumLength(50).WithMessage("Username cannot exceed 50 characters")
            .Matches(@"^[a-zA-Z0-9_]+$").WithMessage("Username can only contain letters, numbers, and underscores");

        // login — basic checks only, BCrypt handles real verification
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters")
            .MaximumLength(100).WithMessage("Password cannot exceed 100 characters");
    }
}