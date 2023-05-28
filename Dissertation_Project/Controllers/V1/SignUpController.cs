using DataLayer.Entities;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using AspNetCore.ReCaptcha;


namespace Dissertation_Project.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("API/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class SignUpController : ControllerBase
    {
        #region IOC

        private const string Controller_Name = "SignUp";
        private UserManager<Users> _userManager;
        private SignInManager<Users> _signInManager;
        private Core.Utlities.JWT.IJWTBearer _jwtBearer;
        private DataLayer.DataBase.Context_Project _Context;
        private Model.Infra.Interfaces.IEmailSender _emailSender;
        private Model.Infra.Interfaces.IGoogle_Recaptcha _Google_Recaptcha;


        public SignUpController(
              UserManager<Users> userManager
            , SignInManager<Users> signInManager
            , Core.Utlities.JWT.IJWTBearer jWTBearer
            , DataLayer.DataBase.Context_Project context
            , Model.Infra.Interfaces.IEmailSender emailSender
            , Model.Infra.Interfaces.IGoogle_Recaptcha google_Recaptcha
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtBearer = jWTBearer;
            _Context = context;
            _emailSender = emailSender;
            _Google_Recaptcha = google_Recaptcha;
            _Google_Recaptcha = google_Recaptcha;
        }

        #endregion

        /// <summary>
        /// کدملی، شماره دانشجویی و ایمیل در بادی دریافت می شود و برای فعال سازی ایمیل برای کاربر ایمیل ارسال میشود
        /// 
        /// </summary>
        /// <param name="registeruser"></param>
        /// <returns>Status Code </returns>
        [HttpPost("Register")]
        public async Task<IActionResult> Register_User(CancellationToken cancellationToken, [FromBody] Model.DTO.INPUT
            .SignUp.RegisterUserDTO registeruser)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return BadRequest("فرایند توسط کاربر متوقف شده است");
            }
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("اطلاعات به درستی وارد نشده اند");
                }

                // ساخت کاربر با مشخصات ارسال شده
                var newuser = new Users()
                {
                    //FirstName = registeruser.FirstName,
                    //LastName = registeruser.LastName,
                    Email = registeruser.Email,
                    //College = registeruser.College,
                    UserName = registeruser.UserName,
                    NationalCode = registeruser.NationalCode,
                    //Teachers = new List<Users>(),
                };


                if (string.IsNullOrWhiteSpace(registeruser.NationalCode))
                {
                    return BadRequest("کد ملی وارد نشده است");
                }
                // ساخت کاربر
                var Resualt = await _userManager.CreateAsync(newuser, registeruser.NationalCode);
                if (Resualt != null && Resualt.Succeeded)
                {
                    string Token_Confirm_User = await _userManager.GenerateEmailConfirmationTokenAsync(newuser);
                    if (Token_Confirm_User == null
                        || string.IsNullOrEmpty(Token_Confirm_User)
                        || string.IsNullOrWhiteSpace(Token_Confirm_User))
                    {
                        return BadRequest("توانایی فعال سازی ایمیل برای کاربران غیر فعال شده است");
                    }

                    string Callback_Url = Url.Action(nameof(ConfirmEmail), Controller_Name,
                        new { email = newuser.Email, token = Token_Confirm_User }
                    , Request.Scheme);


                    string title = "سامانه پایان‌نامه دانشگاه تربیت دبیر شهید رجایی تهران";
                    string body = "";
                    body += "<strong>فعال سازی حساب کاربری : </strong>";
                    body += $"<a href={Callback_Url} >برای فعالسازی کلیک کنید</a>";

                    // Send Email In Background Application
                    BackgroundJob.Enqueue<Model.Infra.Interfaces.IEmailSender>(p =>
                    p.SendEmailAsync(newuser.Email, title.ToString(), body.ToString()));

                    // اضافه کردن نقش پیش فرض برای کاربر
                    if (!await Add_Defualt_Role(newuser))
                    {
                        return BadRequest("نقش مدنظر برای کاربر اضافه نشده است");
                    }

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
                            Url = Url.Action(nameof(Register_User), Controller_Name, new { }, Request.Scheme),
                            Message = $"کاربر {newuser.FirstName} {newuser.LastName} ثبت نام کرده است"
                        }));
                    #endregion

                    return Ok(All_API_Links());
                }

                if (Resualt == null)
                {
                    return BadRequest("خطا در ثبت کاربر");
                }

                var Errors = Resualt.Errors.ToList();
                var ErrorList = new List<Model.DTO.OUTPUT.SignUp.ErrorList_CreateUserDTO>();
                foreach (var error in Errors)
                {
                    ErrorList.Add(new Model.DTO.OUTPUT.SignUp.ErrorList_CreateUserDTO()
                    {
                        Code = error.Code,
                        Description = error.Description
                    });
                }
                return BadRequest(ErrorList);
            }
            catch (Exception ex)
            {
                // Fatal Log
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
                            Url = Url.Action(nameof(Register_User), Controller_Name, new { }, Request.Scheme),
                            Message = $"Error Message : {ex.Message}"
                        }));
                return BadRequest("Fatal : عملیات متوقف شد");
            }
        }


        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            try
            {
                if (string.IsNullOrEmpty(email)
                    || string.IsNullOrWhiteSpace(email)
                    || string.IsNullOrWhiteSpace(token)
                    || string.IsNullOrEmpty(token))
                {
                    return BadRequest("در دریافت اطلاعات از ایمیل مشکلی وجود دارد");
                }
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return BadRequest("کاربری با این مشخصات وجود ندارد");
                }
                var Resualt_Confirm_Email_User
                    = await _userManager.ConfirmEmailAsync(user, token);

                if (Resualt_Confirm_Email_User != null
                    && Resualt_Confirm_Email_User.Succeeded)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("حساب با موفقیت فعال شده است.");
                    sb.AppendLine("می‌توانید وارد حساب کاربری خود شوید");
                    Model.DTO.OUTPUT.SignUp.ConfirmEmail_UserDTO ConfirmEmailOUTPUT = new Model.DTO.OUTPUT.SignUp.ConfirmEmail_UserDTO()
                    {
                        Message = sb.ToString(),
                        Links = All_API_Links()
                    };
                    return Ok(ConfirmEmailOUTPUT);
                }
                else if (Resualt_Confirm_Email_User == null)
                {
                    return BadRequest("خطایی رخ داده است");
                }

                List<Model.DTO.OUTPUT.SignUp.ErrorList_CreateUserDTO> errors = new List<Model.DTO.OUTPUT.SignUp.ErrorList_CreateUserDTO>();
                foreach (var item in Resualt_Confirm_Email_User.Errors.ToList())
                {
                    errors.Add(new Model.DTO.OUTPUT.SignUp.ErrorList_CreateUserDTO()
                    {
                        Code = item.Code,
                        Description = item.Description
                    });
                }
                return BadRequest(errors);
            }
            catch (Exception ex)
            {
                // Fatal Log
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
                            Url = Url.Action(nameof(ConfirmEmail), Controller_Name, new { }, Request.Scheme),
                            Message = $"Error MEssage : {ex.Message}"
                        }));
                return BadRequest("Fatal : عملیات متوقف شد");
            }
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login_User(CancellationToken cancellationToken, [FromBody] Model.DTO.INPUT.SignUp
            .LoginUserDTO LoginUser)
        {
            #region google recaptcha
            //g-Recaptcha-Response
            /*
            string? Token_google_recaptcha = this.HttpContext.Request.Form["g-Recaptcha-Response"];
            if(string.IsNullOrEmpty(Token_google_recaptcha))
            {
                this.HttpContext.Response.Headers.Add("g-Google-Recaptcha:", "Is Null");
            }
            bool Verify_Google_recaptcha  = await _Google_Recaptcha.Verify(Token_google_recaptcha);
            if(Verify_Google_recaptcha)
            {
                this.HttpContext.Response.Headers.Add("g-google-response", "Google Recaptcha Is Confirm");
            }else
            {
                this.HttpContext.Response.Headers.Add("g-google-response", "Google Recaptcha Is Not Confirm");
            }
            */
            #endregion

            if (cancellationToken.IsCancellationRequested)
            {
                return BadRequest("فرایند از طرف کاربر متوقف شده است");
            }
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("اطلاعات وارد شده درست نیست");
                }
                if (LoginUser == null
                    || string.IsNullOrWhiteSpace(LoginUser.UserName)
                    || string.IsNullOrWhiteSpace(LoginUser.Password))
                {
                    return BadRequest("اطلاعاتی ورودی تشخیص داده نشده است");
                }

                var user = await _userManager.FindByNameAsync(LoginUser.UserName);
                if (user == null)
                {
                    return BadRequest("کاربر یافت نشد");
                }
                var RoleUser = await _userManager.GetRolesAsync(user);
                if (RoleUser == null || RoleUser.Count < 0)
                {
                    return BadRequest("کاربر نقشی ندارد");
                }

                var Resualt = await _signInManager.
                    PasswordSignInAsync(user, LoginUser.Password, LoginUser.IsRememberMe, true);

                if (Resualt != null && Resualt.Succeeded)
                {
                    // Generate Token For User

                    var Token = _jwtBearer.GetUserToken(user.Id, user.UserName, RoleUser.ToList());
                    if (Token == null)
                    {
                        return BadRequest("ساختن مجوز برای کاربر انجام نشد");
                    }

                    Model.DTO.OUTPUT.SignUp.UserInfo_LoginDTO userinfo;
                    if (string.IsNullOrWhiteSpace(user.FirstName)
                        || string.IsNullOrWhiteSpace(user.LastName))
                    {
                        userinfo = new Model.DTO.OUTPUT.SignUp.UserInfo_LoginDTO()
                        {
                            UserName = user.UserName,
                            Role = RoleUser[0],
                            Token = Token
                        };

                        return Ok(userinfo);
                    }

                    userinfo = new Model.DTO.OUTPUT.SignUp.UserInfo_LoginDTO()
                    {
                        FullName = user.FirstName + " " + user.LastName,
                        UserName = user.UserName,
                        Role = RoleUser[0],
                        Token = Token
                    };

                    return Ok(userinfo);
                }

                return Content("کاربری یافت نشد");
            }
            catch (Exception ex)
            {
                // Fatal Log
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
                            Url = Url.Action(nameof(Register_User), Controller_Name, new { }, Request.Scheme),
                            Message = $"Error Message : {ex.Message}"
                        }));
                return BadRequest("Fatal : عملیات متوقف شد");
            }
        }


        [HttpPost("ForgetPassword/{Value}")]
        public async Task<IActionResult> ForgetPassword([FromRoute] string Value)
        {
            try
            {
                // Get Email Or UserName Or FirstName Or LastName
                // Send Email For User If : User != null => existed
                var user = await _Context.Users.Where(t => t.UserName == Value
                || t.FirstName == Value
                || t.LastName == Value
                || t.Email == Value).FirstOrDefaultAsync();

                if (user == null
                    || string.IsNullOrWhiteSpace(user.Email)
                    || string.IsNullOrEmpty(user.Email))
                    return NotFound("کاربر با مشخصات ارسال شده یافت نشد");

                // Send Email For user And Confirm Email
                string title = "";
                string body = "";
                // عنوان رایانامه
                title += $"سامانه پایان نامه دانشگاه شهید رجایی تهران";
                title += $"فراموشی رمز عبور : ";

                // بدنه رایانامه
                string token = await _userManager.GeneratePasswordResetTokenAsync(user);

                string Callback_Url = Url.Action(nameof(NewPassword)
                    , Controller_Name, new { User_Id = user.Id, Token = token }, Request.Scheme);

                body += $"برای تغییر رمز عبور روی لینک پایین کلیک کنید : ";
                body += $"<a> href={Callback_Url}>تغییر رمز عبور</a>";

                bool Resualt_Send_Email = await _emailSender.SendEmailAsync(user.Email, title.ToString(), body.ToString());
                if (Resualt_Send_Email)
                {
                    return Ok(new Model.DTO.OUTPUT.SignUp.ConfirmEmail_UserDTO()
                    {
                        Message = "ایمیل خود را چک کنید",
                        Links = All_API_Links()
                    });
                }

                return BadRequest("ایمیل ارسال نشد");
            }
            catch (Exception ex)
            {
                // Fatal Log
                PersianCalendar persianCalendar = new PersianCalendar();
                BackgroundJob.Enqueue<Model.Infra.Interfaces.ILogManager>(t =>
                t.InsertLogInDatabase(new Model.DTO.INPUT.Temp.InsertLogDTO()
                {
                    Client = this.HttpContext.Request.Headers["sec-ch-ua"].ToString(),
                    Date = $"{persianCalendar.GetYear(DateTime.Now)}/{persianCalendar.GetMonth(DateTime.Now)}/{persianCalendar.GetDayOfMonth(DateTime.Now)}",
                    Level = "Fatal",
                    Ip = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                    Url = this.HttpContext.Request.Headers["Origin"].ToString() + this.HttpContext.Request.Path.Value,
                    Time = $"{DateTime.Now.ToLongTimeString()}",
                    Method = "POST",
                    System = this.HttpContext.Request.Headers["sec-ch-ua-platform"].ToString(),
                    Message = $"Message : {ex.Message} | Controller : SignUp & Action : ForgetPassword"
                }));
                return BadRequest("Fatal : عملیات متوقف شد");
            }
        }


        [HttpGet("NewPassword/{User_Id}/{Token}")]
        public async Task<IActionResult> NewPassword([FromRoute] ulong? User_Id, [FromRoute] string? Token)
        {
            try
            {
                if (User_Id == null ||
                    User_Id == 0 ||
                    string.IsNullOrEmpty(Token) ||
                    string.IsNullOrWhiteSpace(Token))
                {
                    return BadRequest("شناسه کاربر درست نمی‌باشد");
                }

                return Ok(new Model.DTO.OUTPUT.SignUp.IdToken_NewPassword()
                {
                    Token = Token,
                    User_Id = User_Id
                });
            }
            catch (Exception ex)
            {
                // Fatal Log
                PersianCalendar persianCalendar = new PersianCalendar();
                BackgroundJob.Enqueue<Model.Infra.Interfaces.ILogManager>(t =>
                t.InsertLogInDatabase(new Model.DTO.INPUT.Temp.InsertLogDTO()
                {
                    Client = this.HttpContext.Request.Headers["sec-ch-ua"].ToString(),
                    Date = $"{persianCalendar.GetYear(DateTime.Now)}/{persianCalendar.GetMonth(DateTime.Now)}/{persianCalendar.GetDayOfMonth(DateTime.Now)}",
                    Level = "Fatal",
                    Ip = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                    Url = this.HttpContext.Request.Headers["Origin"].ToString() + this.HttpContext.Request.Path.Value,
                    Time = $"{DateTime.Now.ToLongTimeString()}",
                    Method = "GET",
                    System = this.HttpContext.Request.Headers["sec-ch-ua-platform"].ToString(),
                    Message = $"Message : {ex.Message} | Controller : SignUp & ACtion : NewPassword"
                }));
                return BadRequest("Fatal : عملیات متوقف شد");
            }
        }


        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePasswordUser(
            [FromBody] Model.DTO.INPUT.SignUp.ChangePassword_UserDTO passwordData)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest("در ارسال اطلاعات مشکلی وجود دارد");
                }
                if (passwordData == null)
                {
                    return BadRequest("اطلاعاتی ارسال نشده است");
                }

                var user = await _Context.Users.FirstOrDefaultAsync(t => t.Id == passwordData.User_Id);
                if (user == null)
                {
                    return BadRequest("کاربر با شناسه ارسال شده یافت نشد");
                }

                var Resualt_Reset_Password =
                    await _userManager.ResetPasswordAsync(user, passwordData.Token, passwordData.Password);
                if (Resualt_Reset_Password == null
                    || Resualt_Reset_Password.Succeeded == false)
                {
                    return BadRequest("تغییر رمز برای کاربر انجام نشد");
                }

                if (Resualt_Reset_Password.Succeeded)
                {
                    return Ok(new Model.DTO.OUTPUT.SignUp.ConfirmEmail_UserDTO()
                    {
                        Message = "تغییر رمز با موفقیت انجام شد",
                        Links = All_API_Links()
                    });
                }
                return BadRequest("در انجام عملیات مشکلی رخ داده است");
            }
            catch (Exception ex)
            {
                // Fatal Log
                PersianCalendar persianCalendar = new PersianCalendar();
                BackgroundJob.Enqueue<Model.Infra.Interfaces.ILogManager>(t =>
                t.InsertLogInDatabase(new Model.DTO.INPUT.Temp.InsertLogDTO()
                {
                    Client = this.HttpContext.Request.Headers["sec-ch-ua"].ToString(),
                    Date = $"{persianCalendar.GetYear(DateTime.Now)}/{persianCalendar.GetMonth(DateTime.Now)}/{persianCalendar.GetDayOfMonth(DateTime.Now)}",
                    Level = "Fatal",
                    Ip = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                    Url = this.HttpContext.Request.Headers["Origin"].ToString() + this.HttpContext.Request.Path.Value,
                    Time = $"{DateTime.Now.ToLongTimeString()}",
                    Method = "POST",
                    System = this.HttpContext.Request.Headers["sec-ch-ua-platform"].ToString(),
                    Message = $"Message : {ex.Message} | Controller : SignUp & Action : ChangePassword"
                }));
                return BadRequest("Fatal : عملیات متوقف شد");
            }
        }


        [HttpPost("LogOut")]
        public async Task<IActionResult> LogOut()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return Ok(All_API_Links());

            }
            catch (Exception ex)
            {
                // Fatal Log
                PersianCalendar persianCalendar = new PersianCalendar();
                BackgroundJob.Enqueue<Model.Infra.Interfaces.ILogManager>(t =>
                t.InsertLogInDatabase(new Model.DTO.INPUT.Temp.InsertLogDTO()
                {
                    Client = this.HttpContext.Request.Headers["sec-ch-ua"].ToString(),
                    Date = $"{persianCalendar.GetYear(DateTime.Now)}/{persianCalendar.GetMonth(DateTime.Now)}/{persianCalendar.GetDayOfMonth(DateTime.Now)}",
                    Level = "Fatal",
                    Ip = this.HttpContext.Connection.RemoteIpAddress.ToString(),
                    Url = this.HttpContext.Request.Headers["Origin"].ToString() + this.HttpContext.Request.Path.Value,
                    Time = $"{DateTime.Now.ToLongTimeString()}",
                    Method = "POST",
                    System = this.HttpContext.Request.Headers["sec-ch-ua-platform"].ToString(),
                    Message = $"Message : {ex.Message} | Controller : SignUp & Action : LOgOut"
                }));
                return BadRequest("Fatal : عملیات متوقف شد");
            }
        }

        [HttpGet("TestAction")]
        public IActionResult Test()
        {
            return Ok(All_API_Links());
        }

        #region Tools Methods
        private async Task<bool> Add_Defualt_Role(Users? user)
        {
            if (user == null)
            {
                return false;
            }
            var Resualt_Add_Role = await _userManager
                .AddToRoleAsync(user, DataLayer.Tools.RoleName_enum.Student.ToString());
            if (Resualt_Add_Role != null && Resualt_Add_Role.Succeeded)
            {
                return true;
            }

            return false;
        }

        private List<Model.DTO.API_Links> All_API_Links()
        {
            List<Model.DTO.API_Links> Links = new List<Model.DTO.API_Links>()
            {
                new Model.DTO.API_Links()
                    {
                        Method="POST",
                        Link = Url.Action(nameof(Login_User),Controller_Name,new{ },protocol:Request.Scheme)
                    },
                    new Model.DTO.API_Links()
                    {
                        Method="POST",
                        Link = Url.Action(nameof(Register_User),Controller_Name,new{ },protocol:Request.Scheme)
                    },
                    new Model.DTO.API_Links()
                    {
                        Method="POST",
                        Link=Url.Action(nameof(ForgetPassword),Controller_Name,new{ },protocol:Request.Scheme)
                    },
                    new Model.DTO.API_Links()
                    {
                        Method="POST",
                        Link=Url.Action(nameof(LogOut),Controller_Name,new{ },protocol:Request.Scheme)
                    }
            };

            return Links;
        }
        #endregion
    }
}
