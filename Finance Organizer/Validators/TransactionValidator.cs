using Finance_Organizer.Model;
using FluentValidation;

namespace Finance_Organizer.Validators
{
    public class TransactionValidator : AbstractValidator<Transaction>
    {
        public TransactionValidator()
        {
            RuleFor(x => x.Name).Length(2, 20);
            RuleFor(x => x.Comment).Length(4, 50);
        }
    }
}
