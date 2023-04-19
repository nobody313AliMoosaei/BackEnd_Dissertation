using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Dissertation_Project.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("API/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(Roles = "Student")]
    public class Pre_RegistrationController : ControllerBase
    {
        private const string CONTROLLER_NAME = "Pre_Registration";
        #region IOC
        private DataLayer.DataBase.Context_Project _context;
        private UserManager<DataLayer.Entities.Users> _userManager;

        public Pre_RegistrationController(DataLayer.DataBase.Context_Project context
            , UserManager<DataLayer.Entities.Users> usermanager)
        {
            _context = context;
            _userManager = usermanager;
        }
        #endregion



        // Step_1
        #region Personal Information
        [HttpPost("Personal_Info")]
        public async Task<IActionResult> PersonalInformation(Model.DTO.INPUT.Pre_Registration.Personal_Info_DTO PersonalInfo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("اطلاعات به درستی ارسال نشده است");
                }

                var userId = User.Claims.FirstOrDefault(t => t.Type == ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrWhiteSpace(userId))
                {
                    return BadRequest("استخراج شناسه کاربر انجام نشده است");
                }
                // Find User From Token
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return BadRequest("کاربر یافت نشد");
                }
                // Find Role And Check
                var RoleUser = await _userManager.GetRolesAsync(user);
                if (RoleUser.Count <= 0)
                {
                    return BadRequest("کاربر نقش ندارد و نمی توان ادامه فرایند را پیش برد");
                }
                if (!Is_Have_Role(RoleUser, DataLayer.Tools.RoleName_enum.Student.ToString()))
                {
                    return BadRequest("کاربر دارای نقش دانشجو نیست");
                }

                user.FirstName = PersonalInfo.FirstName;
                user.LastName = PersonalInfo.LastName;
                user.College = PersonalInfo.College;

                // Add Teacher_1
                // باید چک کنیم ک آیا استاد، نقش استاد راهنما را دارد یا خیر ؟
                var Teacher_1 = await _userManager.Users.FirstOrDefaultAsync(t => t.Id == PersonalInfo.Teacher_1);
                if (Teacher_1 == null)
                    return BadRequest("استاد راهنمای اول پیدا نشد !!");

                var Role_Teacher_1 = await _userManager.GetRolesAsync(user);
                if (!Is_Have_Role(Role_Teacher_1, DataLayer.Tools.RoleName_enum.GuideMaster.ToString()))
                {
                    return BadRequest("استاد راهنمای اول دارای نقش استاد راهنما نمی باشد");
                }

                // Add Teacher_2
                if (PersonalInfo.Teacher_2 > 0)
                {
                    var Teacher_2 = await _userManager.FindByIdAsync(PersonalInfo.Teacher_2.ToString());
                    if (Teacher_2 != null)
                    {
                        var Role_Teacher_2 = await _userManager.GetRolesAsync(Teacher_2);
                        if (Role_Teacher_2 != null
                            && Role_Teacher_2.Count > 0)
                        {
                            if (Is_Have_Role(Role_Teacher_2, DataLayer.Tools.RoleName_enum.GuideMaster.ToString()))
                            {
                                user.Teachers.Add(Teacher_2);
                            }
                        }
                    }
                }

                // Add Teacher_3
                if (PersonalInfo.Teacher_3 > 0)
                {
                    var Teacher_3 = await _userManager.FindByIdAsync(PersonalInfo.Teacher_2.ToString());
                    if (Teacher_3 != null)
                    {
                        var Role_Teacher_3 = await _userManager.GetRolesAsync(Teacher_3);
                        if (Role_Teacher_3 != null
                            && Role_Teacher_3.Count > 0)
                        {
                            if (Is_Have_Role(Role_Teacher_3, DataLayer.Tools.RoleName_enum.GuideMaster.ToString()))
                            {
                                user.Teachers.Add(Teacher_3);
                            }
                        }
                    }
                }

                // Update user
                var Resualt_Update_User = await _userManager.UpdateAsync(user);
                if (Resualt_Update_User == null)
                {
                    return BadRequest("بروزرسانی کاربر انجام نشد");
                }
                if (Resualt_Update_User.Succeeded)
                {
                    return Ok("بروزرسانی انجام شده");
                }
                return BadRequest("اطلاعات ثبت نشد");
            }
            catch
            {
                return BadRequest("Fatal : در اجرای برنامه مشکلی به وجود آمده است");
            }
        }
        private bool Is_Have_Role(IList<string> Roles, string Role)
        {
            foreach (var item in Roles)
            {
                if (item == Role) return true;
            }
            return false;
        }
        #endregion

        // Step_2
        #region Dissertaion Information

        [HttpPost("Dissertaion_Info")]
        public async Task<IActionResult> Dissertation_Info(CancellationToken cancellationToken,[FromBody]Model.DTO.INPUT.Pre_Registration.Dissertation_Info Dissertation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("اطلاعات به درستی وارد نشده است");
            }
            // Fine User
            var User_Id = User.Claims.FirstOrDefault(t => t.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(User_Id))
            {
                return BadRequest("شناسه کاربر از توکن ارسالی دریافت نشده است");
            }
            var user = await _userManager.FindByIdAsync(User_Id);
            if (user == null)
            {
                return BadRequest("کاربری با چنین مشخصاتی یافت نشد");
            }
            // add Dissertation
            var New_Dissertation = new DataLayer.Entities.Dissertations()
            {
                Title_English = Dissertation.Title_English,
                Title_Persian = Dissertation.Title_Persian,
                Abstract = Dissertation.Abstract,
                Allow_Edit = true,
                Insert_DateTime = DateTime.Now,
                Term_Number = Dissertation.Term_Number,
                Student=user
            };

            // Save Dissertation in database
            await _context.Dissertations.AddAsync(New_Dissertation, cancellationToken);

            // استخراج Id
            var Id_Dissertation = await _context.Dissertations
                .Where(t => t.Title_Persian == Dissertation.Title_Persian)
                .Select(t => t.Dissertation_Id).FirstOrDefaultAsync(cancellationToken);

            if(Id_Dissertation<=0)
            {
                return BadRequest("پایان نامه دریافت نشد");
            }

            return Created(Url.Action(nameof(Upload_Dissertation), CONTROLLER_NAME, new { },Request.Scheme),Id_Dissertation);
        }
        #endregion

        // Step_3

        [HttpPost("Upload_Dissertation")]
        public async Task<IActionResult>Upload_Dissertation(CancellationToken cancellationToken
            ,Model.DTO.INPUT.Pre_Registration.Upload_Dissertation_DTO Dissertation_File)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("فایل ها به درستی ارسال نشده است");
            }
            // File Info

            return Ok();
        }

        // Step_4


        




        // 
    }
}
