using BusinessLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dissertation_Project.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("API/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        private BusinessLayer.Services.SignUp.SignUpBL _signUpBL;
        
        public SignUpController(BusinessLayer.Services.SignUp.SignUpBL signUpBL, BusinessLayer.Services.Session.ISessionManager sessionManager)
        {
            _signUpBL = signUpBL;
        }


        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody] BusinessLayer.Models.INPUT.SignUp.RegisterUserDTO model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("اطلاعات به درستي ارسال نشده است");
                var Result = await _signUpBL.Register(model);
                if (!Result.IsValid)
                    return BadRequest(Result);

                return Ok(Result);

            }catch(Exception ex)
            {
                ErrorsVM res = new ErrorsVM();
                res.Title = "خطا در اجراي برنامه";
                res.Message = ex.Message;
                return BadRequest(res);
            }
        }
    }
}
