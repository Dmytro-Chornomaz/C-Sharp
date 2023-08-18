using FluentValidation;
using static HomeWork7.Controllers.PiratesController;

namespace HomeWork7
{
    public class PirateValidator : AbstractValidator<Pirate>
    {
        public PirateValidator() 
        { 
            RuleFor(x => x.Name).NotEmpty().Length(2, 20);
            RuleFor(x => x.Age).Length(1, 20);
            RuleFor(x => x.Description).Length(4, 50);
        }
    }
}
