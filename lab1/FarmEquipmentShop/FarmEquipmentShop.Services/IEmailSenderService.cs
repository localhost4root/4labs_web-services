namespace FarmEquipmentShop.Services
{
    public interface IEmailSenderService
    {
        void SendEmail(string to, string subject, string content);
    }
}
