using BusinessLayer.Models;
using BusinessLayer.Models.INPUT.Dissertation;
using BusinessLayer.Utilities;
using DataLayer.DataBase;
using DataLayer.Entities;
using DataLayer.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Dissertation
{
    public class DissertationBL
    {
        private Context_Project _context;
        private BusinessLayer.Services.UploadFile.IUploadFile _uploadFile;
        private IHttpContextAccessor _contectAccessor;
        private BusinessLayer.Services.Log.IHistoryManager _historyManager;
        private UserManager<Users> _userManager;

        public DissertationBL(Context_Project contex, UploadFile.IUploadFile uploadFile, IHttpContextAccessor contectAccessor
            , Log.IHistoryManager historyManager, UserManager<Users> usermanager)
        {
            _context = contex;
            _uploadFile = uploadFile;
            _contectAccessor = contectAccessor;
            _historyManager = historyManager;
            _userManager = usermanager;
        }

        public async Task<ErrorsVM> CheckPreRegister(long UserID = 0)
        {
            var res = new ErrorsVM();
            try
            {
                var Dissertation = await _context.Dissertations.Where(o => o.StudentId == UserID
                && o.StatusDissertation >= (int)DataLayer.Tools.Dissertation_Status.Register).ToListAsync();

                if (Dissertation != null && Dissertation.Count > 0)
                    res.Message = "کاربر پایان نامه در جريان دارد";
                else
                {
                    res.Message = "کاربر پایان نامه در جريان ندارد";
                    res.IsValid = true;
                }
            }
            catch (Exception ex)
            {
                res.Title = "خطا در اجرای برنامه";
                res.Message = ex.Message;
            }
            return res;
        }

        public async Task<ErrorsVM> PreRegister(long UserID, IFormFile Dissertation_File, IFormFile Pro_File, PreRegisterDataDTO data)
        {
            var res = new ErrorsVM();
            if (UserID == 0)
            {
                res.Message = "کاربري يافت نشد";
                return res;
            }
            try
            {
                if (Dissertation_File == null)
                {
                    res.Message = "فایل پایان نامه بارگزاری نشده است";
                    return res;
                }
                else if (Pro_File == null)
                {
                    res.Message = "فایل نتیجه ارسال نشده است";
                    return res;
                }

                res = await CheckPreRegister(UserID);
                if (!res.IsValid)
                    return res;

                var user = await _context.Users.Where(o => o.Id == UserID)
                    .Include(o => o.Teachers)
                    .FirstOrDefaultAsync();
                if (user == null)
                {
                    res.Message = "کاربری یافت نشد";
                    return res;
                }

                // Update User
                user.FirstName = data.FirstName;
                user.LastName = data.LastName;
                user.College = data.College;
                if (data.Teacher_1 != null && data.Teacher_1 != 0)
                {
                    var teacher1 = await _context.Users.Where(o => o.Id == data.Teacher_1.Value).FirstOrDefaultAsync();
                    if (teacher1 != null)
                    {
                        user.Teachers.Add(new DataLayer.Entities.Teachers()
                        {
                            StudentId = user.Id,
                            TeacherId = teacher1.Id,
                            StudentNavigation = user,
                            TeacherNavigation = teacher1
                        });
                    }
                }
                if (data.Teacher_2 != null && data.Teacher_2 != 0)
                {
                    var teacher2 = await _context.Users.Where(o => o.Id == data.Teacher_2.Value).FirstOrDefaultAsync();
                    if (teacher2 != null)
                    {
                        user.Teachers.Add(new DataLayer.Entities.Teachers()
                        {
                            StudentId = user.Id,
                            TeacherId = teacher2.Id,
                            StudentNavigation = user,
                            TeacherNavigation = teacher2
                        });
                    }
                }
                if (data.Teacher_3 != null && data.Teacher_3.Value != 0)
                {
                    var teacher3 = await _context.Users.Where(o => o.Id == data.Teacher_3.Value).FirstOrDefaultAsync();
                    if (teacher3 != null)
                    {
                        user.Teachers.Add(new DataLayer.Entities.Teachers()
                        {
                            StudentId = user.Id,
                            TeacherId = teacher3.Id,
                            StudentNavigation = user,
                            TeacherNavigation = teacher3
                        });
                    }
                }

                // Create New Dissertation
                var NewDissertation = new Dissertations
                {
                    TitlePersian = data.Title_Persian,
                    TitleEnglish = data.Title_English,
                    TermNumber = data.Term_Number,
                    Abstract = data.Abstract
                };
                if (data.KeyWords != null && data.KeyWords.Count > 0)
                {
                    foreach (var item in data.KeyWords)
                    {
                        NewDissertation.KeyWords.Add(new KeyWord
                        {
                            Word = item
                        });
                    }
                }

                // Upload File
                var DissertationFile = await _uploadFile.UploadFileAysnc(Dissertation_File);
                var ProFile = await _uploadFile.UploadFileAysnc(Pro_File);
                if (DissertationFile != null)
                {
                    NewDissertation.DissertationFileName = DissertationFile.FileName;
                    NewDissertation.DissertationFileAddress = DissertationFile.FileAddress;
                }
                if (Pro_File != null)
                {
                    NewDissertation.ProceedingsFileName = Pro_File.FileName;
                    NewDissertation.ProceedingsFileAddress = ProFile.FileAddress;
                }
                NewDissertation.StatusDissertation = (int)DataLayer.Tools.Dissertation_Status.Register;

                // Add Dissertation In user
                user.Dissertations.Add(NewDissertation);

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                #region log For Update User
                await _historyManager.InsertHistory(DateTime.Now.ToPersianDateTime()
                    , _contectAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                        "SignUp/Register", BusinessLayer.Utilities.Utility.Level_log.Informational.ToString(),
                        _contectAccessor?.HttpContext?.Request?.Headers["sec-ch-ua"].ToString(),
                        $"اطلاعات کاربر {user.UserName} بروز شده است و پایان نامه با موفقیت بارگزاري شد");
                #endregion
                res.Message = "عمليات موفقيت آمیز بود";
                res.IsValid = true;
            }
            catch (Exception ex)
            {
                res.Title = "خطا در اجراي برنامه";
                res.Message = ex.Message;
            }
            return res;
        }

        public string DisplayStatusDissertation(int? status)
        {
            if (status == null || !status.HasValue)
                return "";

            if (status == 1)
                return "تاییدیه استاد راهنمای اول";
            else if (status == 2)
                return "تاییدیه استاد راهنمای دوم";
            else if (status == 3)
                return "تاییدیه استاد راهنمای سوم";
            else if (status == 4)
                return "تاییدیه کارشناس آموزش";
            else if (status == 5)
                return "تاییدیه کارشناس تحصیلات تکمیلی";
            else if (status == 6)
                return "تاییدیه کارشناس امور پایان نامه";
            else if (status == -3333)
                return "رد پایان نامه";
            else
                return "وضعیت نامشخص";
        }

        // Get Dissertation => گرفتن پایان نامه ثبت شده بدون تاییدیه
        public async Task<Models.OUTPUT.Dissertation.DissertationModelOutPut?> GetCurrentDissertation(long User_Id)
        {
            try
            {
                if (User_Id == 0)
                    return null;

                var dissertation = await _context.Dissertations.Where(o => o.StudentId == User_Id
                && o.StatusDissertation == (int)DataLayer.Tools.Dissertation_Status.Register)
                    .FirstOrDefaultAsync();

                if (dissertation == null)
                    return null;

                var model = new Models.OUTPUT.Dissertation.DissertationModelOutPut();
                model.StudentId = dissertation.StudentId;
                model.Abstract = dissertation.Abstract;
                model.DissertationFileAddress = dissertation.DissertationFileAddress;
                model.ProceedingsFileAddress = dissertation.DissertationFileAddress;
                model.TitlePersian = dissertation.TitlePersian;
                model.TitleEnglish = dissertation.TitleEnglish;
                model.TermNumber = dissertation.TermNumber;
                model.StatusDissertation = dissertation.StatusDissertation;
                model.DissertationId = dissertation.DissertationId;
                if (dissertation.DateTime != null && dissertation.DateTime.HasValue)
                {
                    model.DateStr = $"{dissertation.DateTime.Value.Year}/{dissertation.DateTime.Value.Month}/{dissertation.DateTime.Value.Day}";
                    model.TimeStr = $"{dissertation.DateTime.Value.Hour}:{dissertation.DateTime.Value.Minute}:{dissertation.DateTime.Value.Second}";
                }
                model.DisplayStatusDissertation = DisplayStatusDissertation(dissertation.StatusDissertation);
                return model;
            }
            catch
            {
                return null;
            }
        }

        public async Task<ErrorsVM?> UpdateDissertation(IFormFile DissertationFile, IFormFile ProFile, UpdateDissertationDTO UDissertation)
        {
            var res = new ErrorsVM();
            try
            {
                if (UDissertation == null)
                {
                    res.Message = "اطلاعاتی ارسال نشده است";
                    return res;
                }
                if (!UDissertation.StudentId.HasValue || UDissertation.StudentId == null || UDissertation.StudentId == 0)
                {
                    res.Message = "مشخصه‌ای از کاربر در دسترس نیست";
                    return res;
                }
                if (!UDissertation.Dissertation_Id.HasValue || UDissertation.Dissertation_Id == null || UDissertation.Dissertation_Id == 0)
                {
                    res.Message = "مشخصه‌ای از پایان نامه در دسترس نیست";
                    return res;
                }

                //////////////////////////////////////// update user ////////////////////////////////////////////////

                var user = await _context.Users.Where(o => o.Id == UDissertation.StudentId).FirstOrDefaultAsync();
                if (user == null)
                {
                    res.Message = "کاربری یافت نشد";
                    return res;
                }
                // Teachers
                if (UDissertation.Teacher1 != null && UDissertation.Teacher1 > 0)
                {
                    var teacher = await _context.Users.Where(o => o.Id == UDissertation.Teacher1)
                        .FirstOrDefaultAsync();
                    if (teacher != null)
                    {
                        user.Teachers = new List<Teachers>();
                        user.Teachers.Add(new Teachers
                        {
                            StudentId = user.Id,
                            StudentNavigation = user,
                            TeacherId = teacher.Id,
                            TeacherNavigation = teacher
                        });
                    }
                    else
                        res.Message = "استاد راهنمای1 یافت نشد" + Environment.NewLine;
                }
                if (UDissertation.Teacher2 != null && UDissertation.Teacher2 > 0)
                {
                    var teacher = await _context.Users.Where(o => o.Id == UDissertation.Teacher2)
                        .FirstOrDefaultAsync();
                    if (teacher != null)
                    {
                        user.Teachers.Add(new Teachers
                        {
                            StudentId = user.Id,
                            StudentNavigation = user,
                            TeacherId = teacher.Id,
                            TeacherNavigation = teacher
                        });
                    }
                    else
                        res.Message = "استاد راهنمای2 یافت نشد" + Environment.NewLine;
                }
                if (UDissertation.Teacher3 != null && UDissertation.Teacher3 > 0)
                {
                    var teacher = await _context.Users.Where(o => o.Id == UDissertation.Teacher3)
                        .FirstOrDefaultAsync();
                    if (teacher != null)
                    {
                        user.Teachers.Add(new Teachers
                        {
                            StudentId = user.Id,
                            StudentNavigation = user,
                            TeacherId = teacher.Id,
                            TeacherNavigation = teacher
                        });
                    }
                    else
                        res.Message = "استاد راهنمای3 یافت نشد" + Environment.NewLine;
                }
                if (!UDissertation.FirstName.IsNullOrEmpty())
                    user.FirstName = UDissertation.FirstName;

                if (!UDissertation.LastName.IsNullOrEmpty())
                    user.LastName = UDissertation.LastName;

                if (!UDissertation.College.IsNullOrEmpty())
                    user.College = UDissertation.College;

                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                var Dissertation = await _context.Dissertations
                    .Where(o => o.DissertationId == UDissertation.Dissertation_Id
                            && o.StatusDissertation == (int)DataLayer.Tools.Dissertation_Status.Register)
                    .FirstOrDefaultAsync();
                if (Dissertation != null)
                {
                    // Update Dissertation 
                    if (DissertationFile != null)
                    {
                        var disFile = await _uploadFile.UploadFileAysnc(DissertationFile);
                        if (disFile != null)
                        {
                            Dissertation.DissertationFileAddress = disFile.FileAddress;
                            Dissertation.DissertationFileName = disFile.FileName;
                        }
                    }
                    if (ProFile != null)
                    {
                        var Pro__File = await _uploadFile.UploadFileAysnc(ProFile);
                        if (Pro__File != null)
                        {
                            Dissertation.ProceedingsFileAddress = Pro__File.FileAddress;
                            Dissertation.ProceedingsFileName = Pro__File.FileName;
                        }
                    }
                    if (UDissertation.TitleEnglish.IsNullOrEmpty())
                        Dissertation.TitleEnglish = UDissertation.TitleEnglish;

                    if (UDissertation.TitlePersian.IsNullOrEmpty())
                        Dissertation.TitlePersian = UDissertation.TitlePersian;

                    if (UDissertation.TermNumber.IsNullOrEmpty())
                        Dissertation.TermNumber = UDissertation.TermNumber;

                    if (UDissertation.Abstract.IsNullOrEmpty())
                        Dissertation.Abstract = UDissertation.Abstract;

                    if (UDissertation.TermNumber.IsNullOrEmpty())
                        Dissertation.TermNumber = UDissertation.TermNumber;

                    Dissertation.DateTime = DateTime.Now.ToPersianDateTime();
                    if (UDissertation.KeyWords != null && UDissertation.KeyWords.Count > 0)
                    {
                        Dissertation.KeyWords = new List<KeyWord>();
                        foreach (var itm in UDissertation.KeyWords)
                        {
                            Dissertation.KeyWords.Add(new KeyWord
                            {
                                Word = itm
                            });
                        }
                    }

                    _context.Dissertations.Update(Dissertation);
                    await _context.SaveChangesAsync();
                    res.Message = "بروزرسانی با موفقیت انجام شد";
                    res.IsValid = true;
                }
                else
                    res.Message = "برای کاربر پایان نامه جاری وجود ندارد";
            }
            catch (Exception ex)
            {
                res.Title = "خطا در اجرای برنامه";
                res.Message = ex.Message;
            }
            return res;
        }

        public async Task<ErrorsVM?> ChangeStatusDissertation(long Dissertation_Id, long User_Id, Dissertation_Status XStatusDissertation)
        {
            var Err = new ErrorsVM();
            try
            {
                // همخوانی تاییدیه با نقش
                var res = await ConfirmXStatus_UserRole(User_Id, XStatusDissertation);
                if (res != null && res.IsValid)
                {
                    var Dissertation = await _context.Dissertations.Where(o => o.DissertationId == Dissertation_Id).FirstOrDefaultAsync();
                    if (Dissertation != null)
                    {
                        Dissertation.StatusDissertation = (int)XStatusDissertation;
                        _context.Dissertations.Update(Dissertation);
                        await _context.SaveChangesAsync();
                    }
                    else
                        Err.Message = "پایان نامه‌ای وجود ندارد";
                }
                else
                    Err = res;
            }
            catch (Exception ex)
            {
                Err.Title = "خطا در اجرای برنامه";
                Err.Message = ex.InnerException == null ? ex.Message : $"Message: {ex.Message}{Environment.NewLine} InnerMessage:{ex.InnerException.Message}";
            }
            return Err;
        }
        public async Task<ErrorsVM?> ConfirmXStatus_UserRole(long User_Id, Dissertation_Status XStatus)
        {
            var res = new ErrorsVM();
            try
            {
                if (User_Id != 0)
                {
                    var user = await _context.Users.Where(o => o.Id == User_Id).FirstOrDefaultAsync();
                    if (user == null)
                        res.Message = "کاربر موجود نیست";
                    else
                    {
                        var Roles = await _userManager.GetRolesAsync(user);
                        if (Roles != null && Roles.Count > 0)
                        {
                            if ((XStatus == Dissertation_Status.ConfirmationGuideMaster || XStatus == Dissertation_Status.ConfirmationGuideMaster2 || XStatus == Dissertation_Status.ConfirmationGuideMaster3)
                                && (Roles.Any(o => o.ToLower() == DataLayer.Tools.RoleName_enum.Administrator.ToString().ToLower()) || Roles.Any(o => o.ToLower() == RoleName_enum.GuideMaster.ToString().ToLower())))
                                res.IsValid = true;
                            else if (XStatus == Dissertation_Status.ConfirmationEducationExpert
                                && (Roles.Any(o => o.ToLower() == DataLayer.Tools.RoleName_enum.Administrator.ToString().ToLower()) || Roles.Any(o => o.ToLower() == RoleName_enum.EducationExpert.ToString().ToLower())))
                                res.IsValid = true;
                            else if (XStatus == Dissertation_Status.ConfirmationPostgraduateEducationExpert
                                && (Roles.Any(o => o.ToLower() == DataLayer.Tools.RoleName_enum.Administrator.ToString().ToLower()) || Roles.Any(o => o.ToLower() == RoleName_enum.PostgraduateEducationExpert.ToString().ToLower())))
                                res.IsValid = true;
                            else if (XStatus == Dissertation_Status.ConfirmationDissertationExpert
                                && (Roles.Any(o => o.ToLower() == DataLayer.Tools.RoleName_enum.Administrator.ToString().ToLower()) || Roles.Any(o => o.ToLower() == RoleName_enum.DissertationExpert.ToString().ToLower())))
                                res.IsValid = true;
                            else if (XStatus == Dissertation_Status.ExpirDissertation)
                                res.IsValid = true;
                            else
                                res.Message = "کاربر در نقش مناسب برای تغییر وضعیت نیست";
                        }
                        else
                            res.Message = "کاربر در سیستم فاقد نقش مناسب است";
                    }
                }
                else
                {
                    res.Message = "کاربر موجود نیست";
                }
            }
            catch (Exception ex)
            {
                res.Title = "خطا در اجرای برنامه";
                res.Message = ex.InnerException == null ? ex.Message : $"InnerMessage: {ex.InnerException.Message}{Environment.NewLine}Message: {ex.Message}";
            }
            return res;
        }

    }
}
