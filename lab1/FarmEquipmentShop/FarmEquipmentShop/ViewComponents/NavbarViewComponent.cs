using Microsoft.AspNetCore.Mvc;

namespace FarmEquipmentShop.ViewComponents
{
    public class NavbarViewComponent: ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
