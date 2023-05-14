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
        private DataLayer.DataBase.Context_Project _context;

        public Dissertation_StatusController(DataLayer.DataBase.Context_Project context)
        {
            _context = context;
        }
        #endregion


        [Authorize(Roles = "Student")]
        [HttpPost]
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
            .ThenInclude(t => t.Confirmation)
            .Where(t => t.Student.Id == ulong.Parse(User_Id))
            .FirstOrDefaultAsync();

            if (Dissertation == null)
            {
                return Ok(new Model.DTO.OUTPUT.Dissertation_Status.StatusCodeDissertation()
                {
                    StatusCode = -1,
                    Discription = "برای این کاربر پایان نامه‌ای آپلود نکرده است."
                });
            }

            if (Dissertation.ConfirmationsDissertations == null
            || Dissertation?.ConfirmationsDissertations?.Count <= 0)
            {
                return Ok(new Model.DTO.OUTPUT.Dissertation_Status.StatusCodeDissertation
                {
                    StatusCode = 0,
                    Discription = "این پایان نامه هنوز هیچ تاییدیه‌ای را دریافت نکرده است"
                });
            }
            else
            {
                /*
                 تمام تاییدیه های پایان نامه را داریم.
                 پس می توانیم چک کنیم که چه تاییدیه هایی این پایان نامه دارد و استاتوس کد مد نظر را برگردانیم
                 */
                float Counter = 0;
                foreach (var item in Dissertation.ConfirmationsDissertations.ToList())
                {
                    if (item.Confirmation.Code_Dissertation_Confirmation == DataLayer.Tools.Dissertation_Confirmations.ConfirmationGuideMaster
                        && item.IsConfirm)
                    {
                        Counter = 1;
                    }
                    else if (item.Confirmation.Code_Dissertation_Confirmation
                        == DataLayer.Tools.Dissertation_Confirmations.ConfirmationGuideMaster2
                        && item.IsConfirm)
                    {
                        Counter = 1.2f;
                    }
                    else if (item.Confirmation.Code_Dissertation_Confirmation
                        == DataLayer.Tools.Dissertation_Confirmations.ConfirmationGuideMaster3
                        && item.IsConfirm)
                    {
                        Counter = 1.3f;
                    }
                    else if (item.Confirmation.Code_Dissertation_Confirmation ==
                        DataLayer.Tools.Dissertation_Confirmations.ConfirmationEducationExpert
                        && item.IsConfirm)
                    {
                        Counter = 2;
                    }
                    else if (item.Confirmation.Code_Dissertation_Confirmation
                        == DataLayer.Tools.Dissertation_Confirmations.ConfirmationPostgraduateEducationExpert
                        && item.IsConfirm)
                    {
                        Counter = 3;
                    }
                    else if (item.Confirmation.Code_Dissertation_Confirmation ==
                        DataLayer.Tools.Dissertation_Confirmations.ConfirmationDissertationExpert
                        && item.IsConfirm)
                    {
                        Counter = 4;
                    }
                    else
                        Counter = 0;
                }
                if (Counter == 0)
                {
                    return Ok(new Model.DTO.OUTPUT.Dissertation_Status.StatusCodeDissertation
                    {
                        StatusCode = 0,
                        Discription = "این پایان نامه هنوز هیچ تاییدیه‌ای را دریافت نکرده است"
                    });
                }else if(Counter==1)
                {
                    return Ok(new Model.DTO.OUTPUT.Dissertation_Status.StatusCodeDissertation
                    {
                        StatusCode = 1,
                        Discription = "این پایان نامه تاییدیه استاد راهنما را دارد"
                    });
                }else if(Counter==1.2)
                {
                    return Ok(new Model.DTO.OUTPUT.Dissertation_Status.StatusCodeDissertation
                    {
                        StatusCode = 2,
                        Discription = "این پایان نامه تاییدیه استاد راهنمای اول و دوم را دارد"
                    });
                }
                else if(Counter==1.3)
                {
                    return Ok(new Model.DTO.OUTPUT.Dissertation_Status.StatusCodeDissertation
                    {
                        StatusCode = 3,
                        Discription = "این پایان نامه تاییدیه استاد راهنمای اول، دوم و سوم را دارد"
                    });
                }
                else if(Counter==2)
                {
                    return Ok(new Model.DTO.OUTPUT.Dissertation_Status.StatusCodeDissertation
                    {
                        StatusCode = 4,
                        Discription = "این پایان نامه تاییدیه کارشناس آموزش را دارد"
                    });
                }else if(Counter==3)
                {
                    return Ok(new Model.DTO.OUTPUT.Dissertation_Status.StatusCodeDissertation
                    {
                        StatusCode = 5,
                        Discription = "این پایان نامه تاییدیه کارشناس تحصیلات تکمیلی را دارد"
                    });
                }else if(Counter==4)
                {
                    return Ok(new Model.DTO.OUTPUT.Dissertation_Status.StatusCodeDissertation
                    {
                        StatusCode = 6,
                        Discription = "این پایان نامه تاییدیه کارشناس امور پایان نامه ها را دارد"
                    });
                }else
                {
                    return Content("خطا در شمارش تاییدیه ها");
                }
            }
        }
    }
}

