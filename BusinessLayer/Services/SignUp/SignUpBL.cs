using Azure.Core;
using BusinessLayer.Models;
using BusinessLayer.Models.OUTPUT.SignUp;
using BusinessLayer.Utilities;
using DataLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.SignUp
{
    public class SignUpBL
    {
        private UserManager<Users>? _userManager;
        private SignInManager<Users>? _signInManager;
        private Log.IHistoryManager _historyManager;
        private IHttpContextAccessor _contextAccessor;
        private JWT.IJWTTokenManager _jwtTokenManager;
        private Email.IEmailSender _emailSender;
        private DataLayer.DataBase.Context_Project _context;

        public SignUpBL(UserManager<Users> usermanager, Log.IHistoryManager historyManager
            , IHttpContextAccessor httpContextAccessor, SignInManager<Users>? signInManager, JWT.IJWTTokenManager jWT,
            DataLayer.DataBase.Context_Project context, Email.IEmailSender emailSender)
        {
            _userManager = usermanager;
            _historyManager = historyManager;
            _contextAccessor = httpContextAccessor;
            _signInManager = signInManager;
            _jwtTokenManager = jWT;
            _context = context;
            _emailSender = emailSender;
        }

        public async Task<ErrorsVM> Register(Models.INPUT.SignUp.RegisterUserDTO _newUser)
        {
            ErrorsVM err = new ErrorsVM();
            try
            {
                if (_newUser.NationalCode.IsNullOrEmpty())
                {
                    err.ErrorList.Add("کد ملی وارد نشده است");
                }
                if (!_newUser.NationalCode.IsValidNationalCode())
                    err.ErrorList.Add("کد ملی درست نیست");

                var newuser = new Users()
                {
                    Email = _newUser.Email,
                    UserName = _newUser.UserName,
                    NationalCode = _newUser.NationalCode,
                };

                // ساخت کاربر
                var Resualt = await _userManager.CreateAsync(newuser, _newUser.NationalCode ?? "123456");
                if (Resualt != null && Resualt.Succeeded)
                {
                    // اضافه کردن نقش پیش فرض برای کاربر
                    if (!await Add_Defualt_Role(newuser))
                    {
                        err.ErrorList.Add("نقش مدنظر برای کاربر اضافه نشده است" + Environment.NewLine);
                    }

                    #region Background For Log 
                    var UsrName = _newUser.UserName.IsNullOrEmpty() ? _newUser.Email : _newUser.UserName;
                    await _historyManager.InsertHistory(DateTime.Now.ToPersianDateTime(),
                        this._contextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                        this._contextAccessor.HttpContext.Request.Path, BusinessLayer.Utilities.Utility.Level_log.Informational.ToString(),
                        Newtonsoft.Json.JsonConvert.SerializeObject(_contextAccessor?.HttpContext?.Request?.Headers.ToList()),
                        $"کاربر {UsrName} ثبت نام کرده است");
                    #endregion

                    //await _emailSender.SendEmailAsync(_newUser.Email, "سامانه آپلود پایان نامه دانشگاه شهید رجایی",
                    //   "شما با موفقت در سامانه ثبت نام شدید");

                    #region Send Email IN BackGround
                    _historyManager.SendEmail(_newUser.Email, "سامانه آپلود پایان نامه دانشگاه شهید رجایی",
                       "شما با موفقت در سامانه ثبت نام شدید");
                    #endregion


                    err.IsValid = true;
                    err.Message = "کاربر با موفقيت ثبت نام کرده است";

                }
                else
                {
                    if (Resualt != null && Resualt.Errors.Any())
                    {
                        foreach (var itm in Resualt.Errors.ToList())
                        {
                            if (itm.Code == "DuplicateUserName")
                                err.ErrorList.Add("کاربر تکراری مي‌باشد");
                            else
                                err.ErrorList.Add($"Title : {itm.Code} Message : {itm.Description}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                err.Title = "خطا در اجراي برنامه";
                err.Message = ex.Message;
            }
            return err;
        }

        private async Task<bool> Add_Defualt_Role(Users? user)
        {
            if (user == null)
            {
                return false;
            }
            var Resualt_Add_Role = await _userManager.AddToRoleAsync(user, DataLayer.Tools.RoleName_enum.Student.ToString());
            if (Resualt_Add_Role != null && Resualt_Add_Role.Succeeded)
                return true;

            return false;
        }

        public async Task<LoginUserInfoDTO> Login(Models.INPUT.SignUp.LoginUserDTO LoginInfo)
        {
            LoginUserInfoDTO model = null;
            StringBuilder sb = new StringBuilder();
            try
            {
                if (LoginInfo == null
                    || LoginInfo.UserName.IsNullOrEmpty()
                    || LoginInfo.Password.IsNullOrEmpty())
                    sb.AppendLine("اطلاعاتی ورودی تشخیص داده نشده است" + Environment.NewLine);

                var user = await _userManager.FindByNameAsync(LoginInfo.UserName);

                if (user == null)
                {
                    user = await _context.Users.Where(o => o.UserName == LoginInfo.UserName || o.Email == LoginInfo.UserName).FirstOrDefaultAsync();

                    if (user.Active == false)
                    {
                        model.Errors.Message = "کاربر غیر فعال است";
                        return model;
                    }

                    if (user == null)
                    {
                        sb.AppendLine("کاربر یافت نشد" + Environment.NewLine);
                        model = new LoginUserInfoDTO();
                        model.Errors.Message = sb.ToString();
                        return model;
                    }
                }

                if (user.Active == false)
                {
                    model.Errors.Message = "کاربر غیر فعال است";
                    return model;
                }

                var RoleUser = await _userManager.GetRolesAsync(user);

                if (RoleUser == null || RoleUser.Count == 0)
                    sb.AppendLine("کاربر نقشی ندارد" + Environment.NewLine);

                var Resualt = await _signInManager.
                    PasswordSignInAsync(user, LoginInfo.Password, LoginInfo.IsRememberMe, true);

                if (Resualt != null && Resualt.Succeeded)
                {
                    // Generate Token For User

                    var Token = _jwtTokenManager.GetUserToken(user.Id, user.UserName, RoleUser.ToList());
                    if (Token == null)
                        sb.AppendLine("ساختن مجوز برای کاربر انجام نشد" + Environment.NewLine);


                    model = new LoginUserInfoDTO
                    {
                        FullName = ((user.FirstName.IsNullOrEmpty() ? "" : user.FirstName)
                        + " " + (user.LastName.IsNullOrEmpty() ? "" : user.LastName)).Trim(),
                        UserName = user.UserName,
                        Role = RoleUser.FirstOrDefault(),
                        Token = Token
                    };

                    #region Send Email
                    //await _emailSender.SendEmailAsync(user.Email, "ورود به سامانه", "شما با موفقیت به سامانه آپلود فایل دانشگاه شهید رجایی تهران وارد شدید");
                    _historyManager.SendEmail(user.Email, "ورود به سامانه", "شما با موفقیت به سامانه آپلود فایل دانشگاه شهید رجایی تهران وارد شدید");
                    #endregion
                }
                else
                {
                    sb.AppendLine("کاربری یافت نشد" + Environment.NewLine);
                }
                if (sb.Length > 0 || model == null)
                {
                    model = new LoginUserInfoDTO();
                    model.Errors.IsValid = false;
                    model.Errors.Message = sb.ToString();
                }
                else
                    model.Errors.IsValid = true;
            }
            catch (Exception ex)
            {
                if (model == null)
                    model = new LoginUserInfoDTO();
                model.Errors.Message = ex.Message;
                model.Errors.Title = "خطا در اجراي برنامه";
            }
            return model;
        }

        public async Task<ErrorsVM> ForgetPassword(string Info, string Rout)
        {
            var res = new ErrorsVM();
            try
            {
                var user = await _context.Users.Where(t => t.UserName == Info
                || t.FirstName == Info
                || t.LastName == Info
                || t.Email == Info).FirstOrDefaultAsync();

                if (user == null)
                    res.Message = "کاربر با مشخصات ارسال شده یافت نشد" + Environment.NewLine;
                else
                {
                    // Send Email For user And Confirm Email
                    string title = "";
                    string body = "";
                    // عنوان رایانامه
                    title += $"فراموشی رمز عبور : ";
                    title += $"(سامانه پایان نامه دانشگاه شهید رجایی تهران)";

                    // بدنه رایانامه
                    string token = await _userManager.GeneratePasswordResetTokenAsync(user);

                    string Callback_Url = $"{Rout}?User_Id={user.Id}&Token={token}";

                    body += $"برای تغییر رمز عبور روی لینک پایین کلیک کنید : ";
                    body += $"<a href={Callback_Url}>تغییر رمز عبور</a>";

                    bool Resualt_Send_Email = await _emailSender.SendEmailAsync(user.Email, title.ToString(), body.ToString());
                    if (Resualt_Send_Email)
                    {
                        res.IsValid = true;
                        res.Message = "ايميل خود را چک کنيد";
                    }
                    else
                    {
                        res.Message = "ایمیل ارسال نشد" + Environment.NewLine;
                    }
                }
            }
            catch (Exception ex)
            {
                res.Title = "خطا در اجراي برنامه";
                res.Message = ex.Message;
            }
            return res;
        }

        public async Task<ErrorsVM> ChangePassword(Models.INPUT.SignUp.ChangePassword_UserDTO _newPassword)
        {
            var res = new ErrorsVM();
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(t => t.Id == _newPassword.User_Id);
                if (user == null)
                {
                    res.Message = "کاربر با شناسه ارسال شده یافت نشد" + Environment.NewLine;
                }

                var Resualt_Reset_Password =
                    await _userManager.ResetPasswordAsync(user, _newPassword.Token, _newPassword.Password);
                if (Resualt_Reset_Password == null
                    || Resualt_Reset_Password.Succeeded == false)
                {
                    res.Message = "تغییر رمز برای کاربر انجام نشد" + Environment.NewLine;
                }

                if (Resualt_Reset_Password.Succeeded)
                {
                    res.IsValid = true;
                    res.Message = "تغییر رمز با موفقیت انجام شد" + Environment.NewLine;
                }
                else
                    res.Message = "در انجام عملیات مشکلی رخ داده است" + Environment.NewLine;
            }
            catch (Exception ex)
            {
                res.Title = "خطا در اجراي برنامه";
                res.Message = ex.Message;
            }
            return res;
        }

        public async Task<ErrorsVM> LogOut()
        {
            var res = new ErrorsVM();
            try
            {
                await _signInManager.SignOutAsync();
                res.IsValid = true;
                res.Message = "کاربر از حساب کاربري خارج شد";
            }
            catch (Exception ex)
            {
                res.Title = "خطا در اجراي برنامه";
                res.Message = ex.Message;
            }
            return res;
        }

        public async Task<string> RefreshToken(long UserId)
        {
            try
            {
                var user = await _context.Users.Where(o => o.Id == UserId).FirstOrDefaultAsync();
                if (user != null)
                {
                    var Roles = await _userManager.GetRolesAsync(user);
                    string Token = _jwtTokenManager.GetUserToken(user.Id, user.UserName, Roles.ToList());
                    return Token;
                }
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }

    }
}
