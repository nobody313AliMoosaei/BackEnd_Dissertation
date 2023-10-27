using BusinessLayer.Models;
using BusinessLayer.Services.SignUp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            _signUpBL= signupbl;
        }


        [HttpPost]
        public async Task<IActionResult> LoginUser(CancellationToken cancellToken, [FromBody] BusinessLayer.Models.INPUT.SignUp.LoginUserDTO model)
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

    }
}
