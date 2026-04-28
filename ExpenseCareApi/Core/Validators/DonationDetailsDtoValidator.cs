using FluentValidation;
using ExpenseCareApi.Core.DTOs;


namespace ExpenseCareApi.Core.Validators;

public class CreateDonationValidator : AbstractValidator<CreateDonationDetailsDto>
{
    // valid payment modes — single source of truth
    private static readonly string[] ValidPaymentModes =
        { "Cash", "Cheque", "UPI", "Bank Transfer", "DD" };

    public CreateDonationValidator()
    {
        // DonorName — required, max 200 chars, no numbers
        RuleFor(x => x.DonorName)
            .NotEmpty()
                .WithMessage("Donor name is required")
            .MaximumLength(200)
                .WithMessage("Donor name cannot exceed 200 characters")
            .Matches(@"^[a-zA-Z\s]+$")
                .WithMessage("Donor name can only contain letters");

        // Amount — must be greater than 0
        RuleFor(x => x.Amount)
            .GreaterThan(0)
                .WithMessage("Amount must be greater than 0")
            .LessThanOrEqualTo(10_000_000)
                .WithMessage("Amount seems too large — please verify");

        // DonorMobile — optional but if provided must be 10 digits
        RuleFor(x => x.DonorMobile)
            .Matches(@"^\d{10}$")
                .WithMessage("Mobile number must be 10 digits")
            .When(x => !string.IsNullOrEmpty(x.DonorMobile)); // only validate if provided

        // DonorAddress — optional, max 500 chars
        RuleFor(x => x.DonorAddress)
            .MaximumLength(500)
                .WithMessage("Address cannot exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.DonorAddress));

        // PaymentMode — must be one of the valid values
        RuleFor(x => x.PaymentMode)
            .NotEmpty()
                .WithMessage("Payment mode is required")
            .Must(mode => ValidPaymentModes.Contains(mode))
                .WithMessage($"Payment mode must be one of: {string.Join(", ", ValidPaymentModes)}");

        // DonationDate — cannot be future date
        RuleFor(x => x.DonationDate)
            .LessThanOrEqualTo(DateTime.Today)
                .WithMessage("Donation date cannot be a future date")
            .When(x => x.DonationDate.HasValue);

        // DonationFor — optional, max 200 chars
        RuleFor(x => x.DonationFor)
            .MaximumLength(200)
                .WithMessage("Donation for cannot exceed 200 characters")
            .When(x => !string.IsNullOrEmpty(x.DonationFor));

        // Notes — optional, max 1000 chars
        RuleFor(x => x.Notes)
            .MaximumLength(1000)
                .WithMessage("Notes cannot exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}

public class UpdateDonationValidator : AbstractValidator<UpdateDonationDto>
{
    private static readonly string[] ValidPaymentModes =
        { "Cash", "Cheque", "UPI", "Bank Transfer", "DD" };

    public UpdateDonationValidator()
    {
        // same rules as create — same fields are editable
        RuleFor(x => x.DonorName)
            .NotEmpty()
                .WithMessage("Donor name is required")
            .MaximumLength(200)
                .WithMessage("Donor name cannot exceed 200 characters")
            .Matches(@"^[a-zA-Z\s]+$")
                .WithMessage("Donor name can only contain letters");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
                .WithMessage("Amount must be greater than 0")
            .LessThanOrEqualTo(10_000_000)
                .WithMessage("Amount seems too large — please verify");

        RuleFor(x => x.DonorMobile)
            .Matches(@"^\d{10}$")
                .WithMessage("Mobile number must be 10 digits")
            .When(x => !string.IsNullOrEmpty(x.DonorMobile));

        RuleFor(x => x.DonorAddress)
            .MaximumLength(500)
                .WithMessage("Address cannot exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.DonorAddress));

        RuleFor(x => x.PaymentMode)
            .NotEmpty()
                .WithMessage("Payment mode is required")
            .Must(mode => ValidPaymentModes.Contains(mode))
                .WithMessage($"Payment mode must be one of: {string.Join(", ", ValidPaymentModes)}");

        RuleFor(x => x.DonationDate)
            .LessThanOrEqualTo(DateTime.Today)
                .WithMessage("Donation date cannot be a future date")
            .When(x => x.DonationDate.HasValue);

        RuleFor(x => x.DonationFor)
            .MaximumLength(200)
                .WithMessage("Donation for cannot exceed 200 characters")
            .When(x => !string.IsNullOrEmpty(x.DonationFor));

        RuleFor(x => x.Notes)
            .MaximumLength(1000)
                .WithMessage("Notes cannot exceed 1000 characters")
            .When(x => !string.IsNullOrEmpty(x.Notes));
    }
}