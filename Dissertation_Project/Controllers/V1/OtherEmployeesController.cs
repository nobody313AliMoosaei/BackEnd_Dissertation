using BusinessLayer.Models;
using BusinessLayer.Services.GeneralService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dissertation_Project.Controllers.V1
{
    //[Authorize(Roles = "Administrator,GuideMaster,Adviser,EducationExpert,PostgraduateEducationExpert,DissertationExpert")]
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
    }
}
