using FarmEquipmentShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace FarmEquipmentShop.Controllers
{
    public class FileController : Controller
    {
        private readonly IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;
        }

        public IActionResult Index()
        {
            var fileNameList = _fileService.ReadList();

            if (fileNameList == null)
            {
                return View("Error", Request);
            }

            return View(fileNameList);
        }

        [HttpGet]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(IFormFile file)
        {
            _fileService.Upload(file);

            return View(file);
        }

        public IActionResult Preview([FromQuery] string fileName) 
        {
            var preview = _fileService.Read(fileName, 1024);

            if (preview == null)
            {
                return View("Error", Request);
            }

            return View(preview);
        }
    }
}
