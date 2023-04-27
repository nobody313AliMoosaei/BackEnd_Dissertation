using System.Linq;
using Microsoft.AspNetCore.Mvc;
using DataLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Dissertation_Project.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("API/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class Dissertation_StatusController : ControllerBase
    {
        #region IOC
        private UserManager<Users> _Usermanager;
        private DataLayer.DataBase.Context_Project _context;

        public Dissertation_StatusController(UserManager<Users> userManager
            , DataLayer.DataBase.Context_Project context)
        {
            _Usermanager = userManager;
            _context = context;
        }
        #endregion


        [Authorize(Roles = "Student")]
        [HttpPost("Status_Dissertation")]
        public async Task<IActionResult> GetStatus_Dissertation()
        {
            string User_Id = this.User?.Claims?.FirstOrDefault(t => t.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(User_Id))
            {
                return Unauthorized("کاربر در سیستم لاگین نکرده است");
            }
            var Dissertation = await _context.Dissertations
            .Include(t => t.Student)
            .Include(t => t.ConfirmationsDissertations)
            .ThenInclude(t=>t.Confirmation_List)
            .Where(t => t.Student.Id == ulong.Parse(User_Id))
            .FirstOrDefaultAsync();

            if (Dissertation == null)
            {
                return Ok(new Model.DTO.OUTPUT.Dissertation_Status.StatusCodeDissertation()
                {
                    StatusCode = 0,
                    Discription = "برای این کاربر پایان نامه‌ای آپلود نکرده است."
                });
            }

            if (Dissertation.ConfirmationsDissertations == null
            && Dissertation?.ConfirmationsDissertations?.Count <= 0)
            {
                return Ok(new Model.DTO.OUTPUT.Dissertation_Status.StatusCodeDissertation
                {
                    StatusCode = 1,
                    Discription = "این پایان نامه هنوز هیچ تاییدیه‌ای را دریافت نکرده است"
                });
            }
            else
            {
                /*
                لیست تاییدیه ها را چک می کنیم
                */

                foreach (var item in Dissertation.ConfirmationsDissertations.ToList())
                {

                }
            }

            return Ok();
        }
    }
}

