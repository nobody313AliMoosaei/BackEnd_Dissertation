using BusinessLayer.Services.GeneralService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dissertation_Project.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeneralController : ControllerBase
    {
        private IGeneralService _generalService;
        public GeneralController(IGeneralService generalService)
        {
            _generalService = generalService;
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
    }
}
