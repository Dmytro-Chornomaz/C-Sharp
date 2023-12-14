using Finance_Organizer.Model;
using FluentValidation;

namespace Finance_Organizer.Validators
{
    public class CategoriesValidator : AbstractValidator<Categories>
    {
        public CategoriesValidator()
        {
            RuleFor(x => x.Meal).InclusiveBetween(0, 1000000000);
            RuleFor(x => x.CommunalServices).InclusiveBetween(0, 1000000000);
            RuleFor(x => x.Medicine).InclusiveBetween(0, 1000000000);
            RuleFor(x => x.Transport).InclusiveBetween(0, 1000000000);
            RuleFor(x => x.Purchases).InclusiveBetween(0, 1000000000);
            RuleFor(x => x.Leisure).InclusiveBetween(0, 1000000000);
            RuleFor(x => x.Others).InclusiveBetween(0, 1000000000);
            RuleFor(x => x.Savings).InclusiveBetween(0, 1000000000);            
        }
    }
}
