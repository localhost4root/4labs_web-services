using FarmEquipmentShop.Services.Models;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace FarmEquipmentShop.Services
{
    public class FileService : IFileService
    {
        private readonly string _path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files");

        public FileNameListModel? ReadList()
        {
            var fileNamesList = new FileNameListModel();

            try
            {
                foreach (var file in Directory.GetFiles(_path))
                {
                    fileNamesList.Names.Add(Path.GetFileName(file));
                }

                return fileNamesList;
            }
            catch 
            {
                return null;
            }
        }

        public void Upload(IFormFile file)
        {
            var filePath = Path.Combine(_path, file.FileName);

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    if (stream != null)
                    {
                        file.CopyTo(stream);

                    }
                }
            }
            catch 
            {
                return;
            }
        }

        public FilePreviewModel? Read(string fileName, int offset)
        {
            var filePath = Path.Combine(_path, fileName);

            byte[] readBytes = new byte[offset];

            try
            {
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    var readSize = stream.Read(readBytes, 0, readBytes.Length);

                    var preview = new FilePreviewModel()
                    {
                        Content = Encoding.Default.GetString(readBytes, 0, readSize)
                    };

                    return preview;
                }
            } 
            catch
            {
                return null;
            }
        }
    }
}
