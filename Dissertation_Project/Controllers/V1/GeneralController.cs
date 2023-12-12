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
        public IActionResult DownloadFileFormRoot([FromBody] BusinessLayer.Models.INPUT.FileInfoDownloadDTO FileInfo)
        {
            if (System.IO.File.Exists(FileInfo.FileAddress))
            {
                var ResponseFileDownload = _generalService.DownloadFileFormRoot(FileInfo.FileAddress);
                
                if (ResponseFileDownload != null && ResponseFileDownload.FileStream != null && !ResponseFileDownload.FileDownloadName.IsNullOrEmpty())
                    return File(ResponseFileDownload.FileStream, ResponseFileDownload.ContentType, ResponseFileDownload.FileDownloadName);

                else
                    return StatusCode((int)System.Net.HttpStatusCode.NoContent, "درخواست شما قابل انجام نمی‌باشد");
            }
            else
                return NotFound();
        }

        [HttpPost("GetUserById")]
        public async Task<IActionResult> GetUserById(long UserId)
        {
            return Ok(await _generalService.GetUserById(UserId));
        }

        [HttpGet("GetAllCommentsOfDissertationById")]
        public async Task<IActionResult> GetAllDissertationComments(long DissertationId, int PageNumber, int PageSize)
        {
            return Ok(_generalService.GetAllDissertationComments(DissertationId, PageNumber, PageSize));
        }

        [HttpGet("GetAllReplayCommentsByCommentId")]
        public async Task<IActionResult> GetAllReplayCommentsByCommentId(long DissertationId, long CommentId, int PageNumber, int PageSize)
        {
            return Ok(_generalService.GetAllReplayCommentsByCommentId(DissertationId,CommentId,PageNumber, PageSize));
        }

    }
}
