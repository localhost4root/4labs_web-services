using FarmEquipmentShop.Services.Models;
using Microsoft.AspNetCore.Http;

namespace FarmEquipmentShop.Services
{
    public interface IFileService
    {
        FileNameListModel? ReadList();
        void Upload(IFormFile file);
        FilePreviewModel? Read(string fileName, int offset);
    }
}