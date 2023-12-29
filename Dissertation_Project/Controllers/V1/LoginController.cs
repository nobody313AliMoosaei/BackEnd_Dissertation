using BusinessLayer.Models;
using BusinessLayer.Models.INPUT.SignUp;
using BusinessLayer.Services.SignUp;
using BusinessLayer.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Security.Claims;

namespace Dissertation_Project.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("API/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private BusinessLayer.Services.SignUp.SignUpBL _signUpBL;
        public LoginController(SignUpBL signupbl)
        {
            _signUpBL = signupbl;
        }


        [HttpPost]
        public async Task<IActionResult> LoginUser([FromBody] BusinessLayer.Models.INPUT.SignUp.LoginUserDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("اطلاعات ارسال شده ناقص مي‌باشد");
                var Result = await _signUpBL.Login(model);
                if (Result.Errors != null && !Result.Errors.IsValid)
                    return BadRequest(Result);
                return Ok(Result);
            }
            catch (Exception ex)
            {
                var res = new ErrorsVM();
                res.Title = "خطا در اجراي برنامه";
                res.Message = ex.Message;
                return BadRequest(res);
            }
        }


        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPasswordUser([FromBody] string Info)
        {
            try
            {
                if (Info.IsNullOrEmpty())
                    return BadRequest("مشخصه‌ای ارسال نشده است");

                var Result = await _signUpBL.ForgetPassword(Info, Url.Action(nameof(ChangePassword), "Login", new { }, Request.Scheme));

                if (!Result.IsValid)
                    return BadRequest(Result);
                return Ok(Result);
            }
            catch (Exception ex)
            {
                var res = new ErrorsVM();
                res.Title = "خطا در اجراي برنامه";
                res.Message = ex.Message;
                return BadRequest(res);
            }
        }

        [HttpGet(nameof(ChangePassword))]
        public async Task<IActionResult> ChangePassword(string User_Id, string Token)
        {
            return Ok();
        }

        [HttpPost("ChangePasswordUser")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePassword_UserDTO model)
        {
            try
            {
                var Result = await _signUpBL.ChangePassword(model);
                if (!Result.IsValid) return BadRequest(Result);
                return Ok(Result);
            }
            catch (Exception ex)
            {
                ErrorsVM res = new ErrorsVM();
                res.Title = "خطا در اجرای برنامه";
                res.Message = ex.Message;
                return BadRequest(res);
            }
        }

        [Authorize]
        [HttpGet("RefreshTokenAutomatic")]
        public async Task<IActionResult> RefreshTokenAutomatic()
        {
            var userid = User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            if (userid.IsNullOrEmpty())
                return BadRequest("کاربر لاگین نکرده و امکان استفاده از دریافت توکن اتوماتیک را ندارد");
            return Ok(await _signUpBL.RefreshToken(userid.Val64()));
        }

        [HttpGet("RefreshToken")]
        public async Task<IActionResult> RefreshToken(long UserId)
        {
            return Ok(await _signUpBL.RefreshToken(UserId));
        }
    }
}
