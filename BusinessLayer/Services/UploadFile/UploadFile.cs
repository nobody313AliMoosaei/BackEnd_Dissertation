using BusinessLayer.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.UploadFile
{
    public class UploadFile : IUploadFile
    {

        private IWebHostEnvironment _webHostEnvironment;
        public UploadFile(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment= webHostEnvironment;
        }

        public async Task<ResultUploadFile?> UploadFileAysnc(IFormFile MainFile)
        {
            try
            {
                if (MainFile == null)
                {
                    return null;
                }

                FileInfo fileinfo = new FileInfo(MainFile.FileName);

                if (fileinfo.Extension.ToLower() == ".pdf"
                    || fileinfo.Extension.ToLower() == ".docx"
                    || fileinfo.Extension.ToLower() == ".docm"
                    || fileinfo.Extension.ToLower() == ".jpeg"
                    || fileinfo.Extension.ToLower() == ".jpg"
                    || fileinfo.Extension.ToLower() == ".png"
                    || fileinfo.Extension.ToLower()==".rar"
                    || fileinfo.Extension.ToLower() == ".zip")
                {
                    string Filename = "", FileAddress = "";
                    var DateNow = DateTime.Now.ToPersianDateTime();
                    Filename = $"{Guid.NewGuid().ToString()}{fileinfo.Extension}";

                    string Folder_Env = $"{_webHostEnvironment.WebRootPath}\\AllFiles\\{DateNow.Year}";
                    FileAddress = Path.Combine(_webHostEnvironment.WebRootPath, $"AllFiles\\{DateNow.Year}", Filename);

                    if (!Directory.Exists(Folder_Env))
                    {
                        Directory.CreateDirectory(Folder_Env);
                    }
                    var MemoryStream = new MemoryStream();
                    await MainFile.OpenReadStream().CopyToAsync(MemoryStream);

                    using (var fa = new FileStream(FileAddress, FileMode.Create, FileAccess.ReadWrite))
                    {
                        MemoryStream.WriteTo(fa);
                    }

                    var model = new ResultUploadFile()
                    {
                        FileAddress = FileAddress,
                        FileName = Filename
                    };
                    return model;

                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
