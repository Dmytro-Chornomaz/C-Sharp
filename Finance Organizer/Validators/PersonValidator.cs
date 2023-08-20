using Finance_Organizer.Business;
using FluentValidation;

namespace Finance_Organizer.Validators
{
    public class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(2, 20);
        }
    }
}
