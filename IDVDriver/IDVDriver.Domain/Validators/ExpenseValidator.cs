using FluentValidation;

namespace IDVDriver.Domain
{
    public class ExpenseValidator : AbstractValidator<Expense>
    {
        public ExpenseValidator()
        {
            RuleFor(x => x.Type).NotEqual(ExpenseType.None);
            RuleFor(x => x.Date).NotEmpty();
            RuleFor(x => x.Amount).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}