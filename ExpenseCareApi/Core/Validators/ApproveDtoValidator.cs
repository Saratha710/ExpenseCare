using ExpenseCareApi.Core.DTOs;
using FluentValidation;

namespace ExpenseCareApi.Core.Validators.Auth;

public class ApproveDtoValidator : AbstractValidator<ApproveDto>
{
    public ApproveDtoValidator()
    {
        RuleFor(x => x.ApprovedBy)
            .NotEmpty().WithMessage("ApprovedBy is required")
            .MaximumLength(100).WithMessage("ApprovedBy cannot exceed 100 characters");
    }
}