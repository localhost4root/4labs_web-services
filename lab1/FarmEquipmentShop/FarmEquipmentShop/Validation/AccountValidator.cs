using FarmEquipmentShop.Models;
using FluentValidation;

namespace FarmEquipmentShop.Validation
{
    public class AccountValidator: AbstractValidator<AccountModel>
    {
        public AccountValidator()
        {
            RuleFor(a => a.Email)
                .EmailAddress()
                .NotEmpty();

            RuleFor(a => a.Address)
                .MaximumLength(100)
                .NotEmpty();

            RuleFor(a => a.City)
                .MaximumLength(100)
                .NotEmpty();

            RuleFor(a => a.Name)
                .MaximumLength(100)
                .NotEmpty();

            RuleFor(a => a.Middlename)
                .MaximumLength(100)
                .NotEmpty();

            RuleFor(a => a.Lastname)
                .MaximumLength(100)
                .NotEmpty();
        }
    }
}
