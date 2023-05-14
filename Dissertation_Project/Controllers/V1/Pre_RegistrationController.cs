using DataLayer.Entities;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
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
        private Model.Infra.Interfaces.IUpload_File _Upload_file;
        private Model.Infra.Interfaces.ILogManager _LogManager;

        public Pre_RegistrationController(DataLayer.DataBase.Context_Project context
            , UserManager<DataLayer.Entities.Users> usermanager
            , Model.Infra.Interfaces.IUpload_File upload_file
            , Model.Infra.Interfaces.ILogManager logManager)
        {
            _context = context;
            _userManager = usermanager;
            _Upload_file = upload_file;
            _LogManager = logManager;
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
                else
                {
                    user.Teachers.Add(new User_User_Relation()
                    {
                        Teacher_Id = Teacher_1.Id
                    });
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
                                 user.Teachers?.Add(new User_User_Relation()
                                 {
                                     Teacher_Id = Teacher_2.Id
                                 });
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
                                 user.Teachers?.Add(new User_User_Relation() { Teacher_Id = Teacher_3.Id });
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
        public async Task<IActionResult> Dissertation_Info(CancellationToken cancellationToken, [FromBody] Model.DTO.INPUT.Pre_Registration.Dissertation_Info Dissertation)
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
                Term_Number = Dissertation.Term_Number,
                Student = user
            };

            // Save Dissertation in database
            await _context.Dissertations.AddAsync(New_Dissertation, cancellationToken);

            // استخراج Id
            var Id_Dissertation = await _context.Dissertations
                .Where(t => t.Title_Persian == Dissertation.Title_Persian)
                .Select(t => t.Dissertation_Id).FirstOrDefaultAsync(cancellationToken);

            if (Id_Dissertation <= 0)
            {
                return BadRequest("پایان نامه دریافت نشد");
            }

            return Created(Url.Action(nameof(Upload_Dissertation), CONTROLLER_NAME, new { }, Request.Scheme), Id_Dissertation);
        }
        #endregion

        // Step_3
        #region Upload Dissertation
        //[AllowAnonymous]
        [HttpPost("Upload_Dissertation")]
        public async Task<IActionResult> Upload_Dissertation(CancellationToken cancellationToken
            , IFormFile Dissertation_File, IFormFile Pro_File, [FromQuery] ulong Dissertation_Id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("فایل ها به درستی ارسال نشده است");
                }
                if (cancellationToken.IsCancellationRequested)
                {
                    return BadRequest("عملیات توسط کاربر متوقف شده است");
                }

                // File Info

                var Resualt_UploadFile_Dissertation = await _Upload_file.UploadFileAsync(Dissertation_File);
                var Resualt_UploadFile_Pro = await _Upload_file.UploadFileAsync(Pro_File);

                if (Resualt_UploadFile_Dissertation == null)
                {
                    return BadRequest("فایل پایان نامه ذخیره نشد");
                }

                if (Resualt_UploadFile_Pro == null)
                {
                    return BadRequest("فایل صورت جلسه ذخیره نشد");
                }

                await _context.Dissertations.AddAsync(new DataLayer.Entities.Dissertations()
                {
                    Dissertation_FileAddress = Resualt_UploadFile_Dissertation?.FileAddress,
                    Dissertation_FileName = Resualt_UploadFile_Dissertation?.FileName,
                    Proceedings_FileAddress = Resualt_UploadFile_Pro?.FileAddress,
                    Proceedings_FileName = Resualt_UploadFile_Pro?.FileName,
                });

                var t = await _context.SaveChangesAsync();
                if (t > 0)
                {
                    return Ok("فایل ها ذخیره شد");
                }
                return BadRequest("فایل ذخیره نشده");
            }
            catch
            {
                return BadRequest("در ذخیره سازی فایل مشکلی به وجود آمده است");
            }
        }
        #endregion

        // Main Step
        #region Send All Data For Action
        // Convert Three Step to One Step
        [HttpPost]
        public async Task<IActionResult> SendAllDataForConfirm(CancellationToken cancellationToken, IFormFile Dissertation_File
            , IFormFile Pro_File
           , [FromForm] Model.DTO.INPUT.Pre_Registration.AllData_DTO Data)
        {
            try
            {
                string User_Id = User.Claims.FirstOrDefault(t => t.Type == ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrWhiteSpace(User_Id))
                {
                    return BadRequest("شناسه کاربر ارسال نشده است");
                }

                var user = await _context.Users
                    .Include(t => t.Teachers)
                    .FirstOrDefaultAsync(t => t.Id == ulong.Parse(User_Id));
                
                if (user == null)
                {
                    return BadRequest("کاربر پیدا نشده است");
                }
                if (!string.IsNullOrWhiteSpace(user.FirstName)
                    || !string.IsNullOrWhiteSpace(user.LastName))
                {
                    return BadRequest("کاربر قبلا مشخصات خود را وارد کرده است");
                }
                if(!Is_Have_Role(await _userManager.GetRolesAsync(user)
                    , DataLayer.Tools.RoleName_enum.Student.ToString()))
                {
                    return Unauthorized("کاربر دانشجو نیست");
                }

                // Set Information For User
                user.FirstName = Data.FirstName;
                user.LastName = Data.LastName;
                user.College = Data.College;

                var Teacher_1 = await _context.Users.
                    Where(t => t.Id == Data.Teacher_1)
                    .Select(x => new
                    {
                        Teacher_id = x.Id
                    }).FirstOrDefaultAsync();

                if (Teacher_1 == null)
                {
                    return BadRequest("استاد راهنمایی با مشخصات ارسال شده وجود ندارد");
                }
                user.Teachers.Add(new User_User_Relation()
                {
                    Teacher_Id = Teacher_1.Teacher_id
                });

                var Teacher_2 = await _context.Users
                    .Where(t => t.Id == Data.Teacher_2)
                    .Select(t => new
                    {
                        Teacher_id = t.Id
                    })
                    .FirstOrDefaultAsync();

                if (Teacher_2 != null)
                {
                    user.Teachers.Add(new User_User_Relation()
                    {
                        Teacher_Id = Teacher_2.Teacher_id
                    });
                }
                var Teacher_3 = await _context.Users.Where(t => t.Id == Data.Teacher_3)
                    .Select(x => new
                    {
                        Teacher_id = x.Id
                    }).FirstOrDefaultAsync();
                if (Teacher_3 != null)
                {
                    user.Teachers.Add(new User_User_Relation()
                    {
                        Teacher_Id = Teacher_3.Teacher_id
                    });
                }

                // Update User
                //var Resualt_Update_User = await _context.Users.Where(t => t.Id == user.Id)
                //    .ExecuteUpdateAsync(update =>
                //    update.SetProperty(x => x.FirstName, user.FirstName)
                //    .SetProperty(x => x.LastName, user.LastName)
                //    .SetProperty(x => x.College, user.College)
                //    .SetProperty(x => x.Teachers, user.Teachers), cancellationToken);

                _context.Users.Update(user);

                // Log

                #region Log
                BackgroundJob.Enqueue<Model.Infra.Interfaces.ILogManager>
                        (t => t.InsertLogInDatabase(new Model.DTO.INPUT.Temp.InsertLogDTO()
                        {
                            Client = this.HttpContext.Request.Headers["sec-ch-ua"].ToString(),
                            System = HttpContext.Request.Headers["sec-ch-ua-platform"].ToString(),
                            Date = Core.Utlities.Persian_Calender.Shamsi_Calender.GetDate_Shamsi(),
                            Ip = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            Level = Core.Utlities.Level_Log.Level_log.Informational.ToString(),
                            Method = "POST",
                            Time = DateTime.Now.ToLongTimeString(),
                            Url = Url.Action(nameof(SendAllDataForConfirm), CONTROLLER_NAME, new { }, Request.Scheme),
                            Message = $"کاربر {user.FirstName} {user.LastName} بروزرسانی شد"
                        }));
                #endregion

                //if (Resualt_Update_User <= 0)
                //{
                //    return NoContent();
                //}
                var Dissertation_Exist = await _context.Dissertations
                    .Include(t => t.Student)
                    .Where(t => t.Student.Id == user.Id)
                    .Select(t => new
                    {
                        Id = t.Dissertation_Id,
                        Persian_Title = t.Title_Persian,
                        English_Title = t.Title_English
                    })
                    .FirstOrDefaultAsync();
                if (Dissertation_Exist != null)
                {
                    return BadRequest($"پایان نامه با مشخصات : عنوان فارسی {Dissertation_Exist.Persian_Title} و عنوان انگلیسی : {Dissertation_Exist.English_Title} قبلا برای این کاربر ثبت شده است.");
                }
                // Create Dissertaion 
                var Dissertation = new Dissertations()
                {
                    Title_English = Data.Title_English,
                    Title_Persian = Data.Title_Persian,
                    Abstract = Data.Abstract,
                    Term_Number = Data.Term_Number,
                    Allow_Edit = true,
                    Student = user,
                    Date = Core.Utlities.Persian_Calender.Shamsi_Calender.GetDate_Shamsi(),
                    Status_Dissertation = DataLayer.Tools.Status_Dissertation.During,
                };
                if (Data.KeyWords_Persian != null
                    && Data.KeyWords_Persian.Count > 0)
                {
                    List<KeyWord> _list_Persian_KeyWord = new List<KeyWord>();
                    foreach (var item in Data.KeyWords_Persian)
                    {
                        _list_Persian_KeyWord.Add(new KeyWord()
                        {
                            Word = item
                        });
                    }
                    Dissertation.Persian_KeyWords = _list_Persian_KeyWord;
                }
                if (Data.KeyWords_English != null
                    && Data.KeyWords_English.Count > 0)
                {
                    List<KeyWord> _list_KeyWord_English = new List<KeyWord>();
                    foreach (var item in Data.KeyWords_English)
                    {
                        _list_KeyWord_English.Add(new KeyWord()
                        {
                            Word = item
                        });
                    }
                    Dissertation.English_KeyWords = _list_KeyWord_English;
                }

                // upload Files
                var Resualt_Upload_Dissertaion = await
                    _Upload_file.UploadFileAsync(Dissertation_File);

                Dissertation.Dissertation_FileAddress = Resualt_Upload_Dissertaion.FileAddress;
                Dissertation.Dissertation_FileName = Resualt_Upload_Dissertaion.FileName;

                var Resualt_Upload_Proceeding = await
                    _Upload_file.UploadFileAsync(Pro_File);

                Dissertation.Proceedings_FileAddress = Resualt_Upload_Proceeding.FileAddress;
                Dissertation.Proceedings_FileName = Resualt_Upload_Proceeding.FileName;

                await _context.Dissertations.AddAsync(Dissertation, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                #region Background For Log
                BackgroundJob.Enqueue<Model.Infra.Interfaces.ILogManager>
                        (t => t.InsertLogInDatabase(new Model.DTO.INPUT.Temp.InsertLogDTO()
                        {
                            Client = this.HttpContext.Request.Headers["sec-ch-ua"].ToString(),
                            System = HttpContext.Request.Headers["sec-ch-ua-platform"].ToString(),
                            Date = Core.Utlities.Persian_Calender.Shamsi_Calender.GetDate_Shamsi(),
                            Ip = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            Level = Core.Utlities.Level_Log.Level_log.Informational.ToString(),
                            Method = "POST",
                            Time = DateTime.Now.ToLongTimeString(),
                            Url = Url.Action(nameof(SendAllDataForConfirm), CONTROLLER_NAME, new { }, Request.Scheme),
                            Message = $"پایان نامه برای کاربر {user.FirstName} {user.LastName} به ثبت رسیده است"
                        }));
                #endregion

                return Ok("عملیات با موفقیت انجام شد");
            }
            catch (Exception ex)
            {
                #region Background For Log
                BackgroundJob.Enqueue<Model.Infra.Interfaces.ILogManager>
                        (t => t.InsertLogInDatabase(new Model.DTO.INPUT.Temp.InsertLogDTO()
                        {
                            Client = this.HttpContext.Request.Headers["sec-ch-ua"].ToString(),
                            System = HttpContext.Request.Headers["sec-ch-ua-platform"].ToString(),
                            Date = Core.Utlities.Persian_Calender.Shamsi_Calender.GetDate_Shamsi(),
                            Ip = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                            Level = Core.Utlities.Level_Log.Level_log.Emergency.ToString(),
                            Method = "POST",
                            Time = DateTime.Now.ToLongTimeString(),
                            Url = Url.Action(nameof(SendAllDataForConfirm), CONTROLLER_NAME, new { }, Request.Scheme),
                            Message = $"Error Message : {ex.Message}"
                        }));
                #endregion

                return BadRequest("اجرای برنامه با مشکل مواجه شده است");
            }
        }
        #endregion
    }
}
