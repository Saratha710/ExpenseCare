using FluentValidation;
using ExpenseCareApi.Core.DTOs;

namespace ExpenseCareApi.Core.Validators;

public class CreateExpenseValidator : AbstractValidator<CreateExpenseDto>
{
    private static readonly string[] ValidExpenseTypes =
        { "Food", "Transport", "Utilities", "Maintenance",
          "Salary", "Office", "Medical", "Other" };

    public CreateExpenseValidator()
    {
        RuleFor(x => x.ExpenseType)
            .NotEmpty().WithMessage("Expense type is required")
            .Must(type => ValidExpenseTypes.Contains(type))
            .WithMessage($"Expense type must be one of: {string.Join(", ", ValidExpenseTypes)}");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0")
            .LessThanOrEqualTo(10_000_000).WithMessage("Amount seems too large — please verify");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");

        RuleFor(x => x.ExpenseDate)
            .LessThanOrEqualTo(DateTime.Today).WithMessage("Expense date cannot be a future date")
            .When(x => x.ExpenseDate.HasValue);

        RuleFor(x => x.AttachImage)
            .MaximumLength(1000).WithMessage("Image URL cannot exceed 1000 characters")
            .Must(url => url!.StartsWith("http://") || url!.StartsWith("https://"))
            .WithMessage("Image must be a valid URL")
            .When(x => !string.IsNullOrEmpty(x.AttachImage));

        RuleFor(x => x.Notes)
            .MaximumLength(2000).WithMessage("Notes cannot exceed 2000 characters")
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}
public class UpdateExpenseValidator : AbstractValidator<UpdateExpenseDto>
{
    private static readonly string[] ValidExpenseTypes =
        { "Food", "Transport", "Utilities", "Maintenance",
          "Salary", "Office", "Medical", "Other" };

    public UpdateExpenseValidator()
    {
        RuleFor(x => x.ExpenseType)
            .NotEmpty().WithMessage("Expense type is required")
            .Must(type => ValidExpenseTypes.Contains(type))
            .WithMessage($"Expense type must be one of: {string.Join(", ", ValidExpenseTypes)}");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Amount must be greater than 0")
            .LessThanOrEqualTo(10_000_000).WithMessage("Amount seems too large — please verify");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");

        RuleFor(x => x.ExpenseDate)
            .LessThanOrEqualTo(DateTime.Today).WithMessage("Expense date cannot be a future date")
            .When(x => x.ExpenseDate.HasValue);

        RuleFor(x => x.AttachImage)
            .MaximumLength(1000).WithMessage("Image URL cannot exceed 1000 characters")
            .Must(url => url!.StartsWith("http://") || url!.StartsWith("https://"))
            .WithMessage("Image must be a valid URL")
            .When(x => !string.IsNullOrEmpty(x.AttachImage));

        RuleFor(x => x.Notes)
            .MaximumLength(2000).WithMessage("Notes cannot exceed 2000 characters")
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}
