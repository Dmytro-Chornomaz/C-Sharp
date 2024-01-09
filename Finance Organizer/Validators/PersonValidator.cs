using Finance_Organizer.Model;
using FluentValidation;

namespace Finance_Organizer.Validators
{
    public class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(2, 20);
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
