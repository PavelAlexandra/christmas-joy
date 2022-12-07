
using FluentValidation;

namespace ChristmasJoy.App.ViewModels.Validations
{
  public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
  {
    public LoginViewModelValidator()
    {
      RuleFor(vm => vm.Email).NotEmpty().WithMessage("Email cannot be empty");
      RuleFor(vm => vm.Email).EmailAddress().WithMessage("Invalid email address");
      RuleFor(vm => vm.Password).NotEmpty().WithMessage("Password cannot be empty");
      //RuleFor(vm => vm.Password).Length(6, 12).WithMessage("Password must be between 6 and 12 characters.");
    }
  }
}
