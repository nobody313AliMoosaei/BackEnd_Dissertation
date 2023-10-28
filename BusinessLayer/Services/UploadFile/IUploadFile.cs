using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.UploadFile
{
    public interface IUploadFile
    {
        Task<ResultUploadFile> UploadFileAysnc(IFormFile MainFile);
    }
    public class ResultUploadFile
    {
        public string? FileName { get; set; }
        public string? FileAddress { get; set; }
    }
}
