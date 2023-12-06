using BusinessLayer.Services.GeneralService;
using BusinessLayer.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace Dissertation_Project.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralController : ControllerBase
    {
        private IGeneralService _generalService;
        private IWebHostEnvironment _webHostEnvironment;
        public GeneralController(IGeneralService generalService, IWebHostEnvironment webHostEnvironment)
        {
            _generalService = generalService;
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            return Ok(await _generalService.GetAllRoles());
        }

        [HttpGet("GetAllColleges")]
        public async Task<IActionResult> GetAllColleg()
        {
            return Ok(await _generalService.GetAllCollegesUni());
        }


        [HttpGet("GetAllTeacher")]
        public async Task<IActionResult> GetAllTeacher()
        {
            return Ok(await _generalService.GetAllTeacher());
        }


        [HttpGet("GetAllStatusDissertation")]
        public async Task<IActionResult> GetAllStatusDissertation()
        {
            return Ok(await _generalService.GetAllDissertationStatus());
        }

        [HttpPost("DownloadFile")]
        public IActionResult DownloadFileFormRoot([FromBody] string FileAddress)
        {
            if (System.IO.File.Exists(FileAddress))
            {
                var ResponseFileDownload = _generalService.DownloadFileFormRoot(FileAddress);
                
                if (ResponseFileDownload != null && ResponseFileDownload.FileStream != null && !ResponseFileDownload.FileDownloadName.IsNullOrEmpty())
                    return File(ResponseFileDownload.FileStream, ResponseFileDownload.ContentType, ResponseFileDownload.FileDownloadName);

                else
                    return StatusCode((int)System.Net.HttpStatusCode.NoContent, "درخواست شما قابل انجام نمی‌باشد");
            }
            else
                return NotFound();
        }

    }
}
