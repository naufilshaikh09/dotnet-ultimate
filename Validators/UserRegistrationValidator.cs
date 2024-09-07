using dotnet_ultimate.Model;
using FluentValidation;

namespace dotnet_ultimate.Validators;

public class UserRegistrationValidator : AbstractValidator<UserRegistrationRequest>
{
    public UserRegistrationValidator()
    {
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(10);
        RuleFor(x => x.Password).Equal(z => z.ConfirmPassword).WithMessage("Passwords do not match!");
        RuleFor(x => x.Email).EmailAddress().WithName("MailID").WithMessage("{PropertyName} is invalid! Please check!");
        RuleFor(x => x.FirstName)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .MinimumLength(4)
            .Must(IsValidName).WithMessage("{PropertyName} should be all letters.");
    }
    
    private bool IsValidName(string name)
    {
        return name.All(Char.IsLetter);
    }
}
