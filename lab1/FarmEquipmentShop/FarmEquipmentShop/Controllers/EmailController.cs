using FarmEquipmentShop.DTOs;
using FarmEquipmentShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace FarmEquipmentShop.Controllers
{
    public class EmailController : Controller
    {
        private readonly IEmailSenderService _emailSenderService;

        public EmailController(IEmailSenderService emailSenderService)
        {
            _emailSenderService = emailSenderService;
        }

        [HttpPost("email/send")]
        public IActionResult Send(EmailDTO emailDTO)
        {
            _emailSenderService.SendEmail(emailDTO.Email, "Test subject", "Test content");

            return Redirect("~/");
        }
    }
}
