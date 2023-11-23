using BusinessLayer.Services.Dissertation;
using BusinessLayer.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Dissertation_Project.Controllers.V1
{
    [Authorize(Roles = "Student")]
    [ApiVersion("1.0")]
    [Route("API/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class DissertationController : ControllerBase
    {
        private DissertationBL _dissertationBL;

        public DissertationController(DissertationBL dissertationBL)
        {
            _dissertationBL = dissertationBL;
        }

        [HttpPost]
        public async Task<IActionResult> PreRegister(IFormFile DissertationFile,IFormFile ProFile, 
            [FromForm]BusinessLayer.Models.INPUT.Dissertation.PreRegisterDataDTO PreRegisterData)
        {
            try
            {
                if (ModelState.IsValid)
                    return BadRequest("اطلاعات ارسال شده ناقص مي‌باشد");

                var userId = User.Claims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value;
                if (userId.IsNullOrEmpty())
                    return Unauthorized("کاربر لاگین نکرده است");

                var Result = await _dissertationBL.PreRegister(userId.Val64(), DissertationFile, ProFile, PreRegisterData);

                if (Result.IsValid)
                    return Ok(Result);
                return BadRequest(Result);
            }catch (Exception ex)
            {
                return BadRequest($"خطا در اجرای برنامه : {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentDissertation()
        {
            try
            {
                var UserID = User.Claims.FirstOrDefault(o=>o.Type == ClaimTypes.NameIdentifier)?.Value;
                if (UserID.IsNullOrEmpty())
                    return Unauthorized("کاربر لاگین نکرده است");
                var dissertation = await _dissertationBL.GetCurrentDissertation(UserID.Val64());
                if(dissertation==null)
                    return NotFound("پایان نامه در جريانی وجود ندارد");
                return Ok(dissertation);
            }
            catch
            {
                return BadRequest("خطا در اجراي برنامه");
            }
        }


    }
}
