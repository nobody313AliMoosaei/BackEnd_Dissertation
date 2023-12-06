using BusinessLayer.Models;
using DataLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System.Net;

namespace Dissertation_Project.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("API/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        private BusinessLayer.Services.SignUp.SignUpBL _signUpBL;

        public SignUpController(BusinessLayer.Services.SignUp.SignUpBL signUpBL)
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

            }
            catch (Exception ex)
            {
                ErrorsVM res = new ErrorsVM();
                res.Title = "خطا در اجراي برنامه";
                res.Message = ex.Message;
                return BadRequest(res);
            }
        }

        [HttpPost("LogOut")]
        public async Task<IActionResult> LogOutUser()
        {
            try
            {
                var resp = await _signUpBL.LogOut();
                if (!resp.IsValid)
                    return BadRequest(resp);

                return Ok(resp);
            }
            catch (Exception ex)
            {
                var Err = new ErrorsVM();
                Err.ErrorList.Add(ex.Message);
                if (ex.InnerException != null)
                    Err.ErrorList.Add(ex.InnerException.Message);

                return BadRequest(Err);
            }
        }
    }
}
