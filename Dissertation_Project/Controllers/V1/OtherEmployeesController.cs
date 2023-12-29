using BusinessLayer.Models;
using BusinessLayer.Services.GeneralService;
using BusinessLayer.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dissertation_Project.Controllers.V1
{
    //[Authorize(Roles = "Administrator,GuideMaster,Adviser,EducationExpert,PostgraduateEducationExpert,DissertationExpert")]
    [ApiVersion("1.0")]
    [Route("API/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class OtherEmployeesController : ControllerBase
    {
        private IGeneralService _generalService;
        private BusinessLayer.Services.EmployeeService.EmployeeService _employeeService;
        private BusinessLayer.Services.Administrator.AdministratorBL _administratorBL;

        public OtherEmployeesController(IGeneralService generalService, BusinessLayer.Services.EmployeeService.EmployeeService employeeService
            , BusinessLayer.Services.Administrator.AdministratorBL AdministratorBL)
        {
            _generalService = generalService;
            _employeeService = employeeService;
            _administratorBL = AdministratorBL;
        }

        [HttpGet("GetSelfDissertation")]
        public async Task<IActionResult> GetSelfDissertationOfTeacher(long TeacherId, int PageNumber, int PageSize)
        {
            if (TeacherId == 0)
            {
                var UserId = User.Claims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value;
                TeacherId = (UserId.IsNullOrEmpty()) ? 0 : UserId.Val64();
            }
            return Ok(await _employeeService.GetAllDissertationOfTeacher(TeacherId, PageNumber, PageSize));
        }

        [HttpGet("GetSelfDissertationAutomatic")]
        public async Task<IActionResult> GetSelfDissertationOfTeacherAutomaticly(int PageNumber, int PageSize)
        {
            var UserId = this.User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            if (UserId.IsNullOrEmpty())
                return Unauthorized("کاربر لاگین نکرده");
            return Ok(await _employeeService.GetAllDissertationOfTeacher(UserId.Val64(), PageNumber, PageSize));
        }

        [HttpPost("GetAllDissertations")]
        public async Task<IActionResult> GetAllDissertation([FromBody] BusinessLayer.Models.INPUT.Administrator.FilterDissertationDTO _filterModel, int PageNumber, int PageSize)
        {
            return Ok(await _administratorBL.GetAllDissertation(PageNumber, PageSize, _filterModel));
        }

        [HttpPost("ChangeDissertationStatus")]
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


    }
}
