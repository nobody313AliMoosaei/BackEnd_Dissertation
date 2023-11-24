using BusinessLayer.Models;
using BusinessLayer.Services.GeneralService;
using BusinessLayer.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dissertation_Project.Controllers.V1
{
    [Authorize(Roles = "Administrator,GuideMaster,Adviser,EducationExpert,PostgraduateEducationExpert,DissertationExpert")]
    [ApiVersion("1.0")]
    [Route("API/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class OtherEmployeesController : ControllerBase
    {
        private IGeneralService _generalService;

        public OtherEmployeesController(IGeneralService generalService)
        {
            _generalService = generalService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllDissertation([FromBody]string Value = "")
        {
            return Ok(await _generalService.GetAllDissertationStatus());
        }

        [HttpPost]
        public async Task<IActionResult> ChangeDissertationStatus([FromBody] BusinessLayer.Models.INPUT.OtherEmployees.ChangeStatusDTO model)
        {
            if (ModelState.IsValid)
            {
                return Ok(await _generalService.ChangeDissertationStatus(model.DissertationId.Value, model.StatusId.ToString()));
            }
            else
            {
                return BadRequest(new ErrorsVM
                {
                    Message = "اطلاعات به درستی وارد نشده است"
                });
            }
        }

        [HttpPost("SendComment")]
        public async Task<IActionResult> SendComment([FromBody] BusinessLayer.Models.INPUT.CommentInputDTO Comment)
        {
            var result = await _generalService.SendComment(Comment.UserId, Comment.DissertationId, Comment.Title, Comment.Dsr, Comment.CommentId);
            if (result.IsValid)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("Download")]
        public async Task<IActionResult> DonloadFile([FromBody] string FileAddress)
        {
            try
            {
                var UserID = User.Claims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value;
                if (UserID.IsNullOrEmpty())
                    return Unauthorized("کاربر لاگین نکرده است");

                if (System.IO.File.Exists(FileAddress))
                {
                    var File_Info = new FileInfo(FileAddress);
                    var Filestream = System.IO.File.OpenRead(FileAddress);
                    string contentType = "application/octet-stream";

                    var fileDownloadName = UserID + "__" + File_Info.Name;

                    return File(Filestream, contentType, fileDownloadName);
                }
                return BadRequest();
            }
            catch
            {
                return NotFound();
            }
        }

    }
}
