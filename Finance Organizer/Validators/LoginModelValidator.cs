using Finance_Organizer.Model;
using FluentValidation;

namespace Finance_Organizer.Validators
{
    public class LoginModelValidator : AbstractValidator<LoginModel>
    {
        public LoginModelValidator()
        {
            RuleFor(x => x.Login).NotEmpty().Length(2, 20);
            RuleFor(x => x.Password).NotEmpty().Length(8, 20);
        }
    }
}
