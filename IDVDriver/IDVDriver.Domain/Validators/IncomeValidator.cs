using FluentValidation;

namespace IDVDriver.Domain
{
    public class IncomeValidator : AbstractValidator<Income>
    {
        public IncomeValidator()
        {
            RuleFor(x => x.Date).NotEmpty();
            RuleFor(x => x.Amount).NotEmpty();
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}