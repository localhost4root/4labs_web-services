using FarmEquipmentShop.Models;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace FarmEquipmentShop.Controllers
{
    public class AccountController : Controller
    {
        public readonly IValidator<AccountModel> _validator;

        public AccountController(IValidator<AccountModel> validator)
        {
            _validator = validator;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Save(AccountModel account)
        {
            var result = _validator.Validate(account);

            if (result.IsValid) 
            {
                return Redirect("~/");
            }

            var errors = new List<string>();

            foreach (var error in result.Errors) 
            {
                errors.Add(error.ErrorMessage);
            }

            return View("ApplicationError", new ApplicationErrorModel() { Errors = errors });
        }
    }
}
