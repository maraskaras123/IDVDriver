using FluentValidation;

namespace IDVDriver.Domain
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}