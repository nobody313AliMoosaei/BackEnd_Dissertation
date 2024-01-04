﻿using BusinessLayer.Models;
using BusinessLayer.Models.INPUT.Dissertation;
using BusinessLayer.Services.GeneralService;
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
        private UserManager<Users> _userManager;
        private IGeneralService _generalService;
        private BusinessLayer.Services.Log.IHistoryManager _historyManager;
        private IHttpContextAccessor _contextAccessor;
        private Email.IEmailSender _emailService;

        public DissertationBL(Context_Project contex, UploadFile.IUploadFile uploadFile, IHttpContextAccessor contectAccessor
            , Log.IHistoryManager historyManager, UserManager<Users> usermanager, IGeneralService generalService
            , Email.IEmailSender emailSender)
        {
            _context = contex;
            _uploadFile = uploadFile;
            _contextAccessor = contectAccessor;
            _historyManager = historyManager;
            _userManager = usermanager;
            _generalService = generalService;
            _emailService = emailSender;
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
                //user.College = data.College;
                var College = (await _context.Baslookups.Where(o => o.Type.ToLower() == DataLayer.Tools.BASLookupType.CollegesUni.ToString().ToLower()
                && o.Id == data.CollegeRef.Value).FirstOrDefaultAsync());
                user.CollegeRef = College?.Id;
                user.CollegeRefNavigation = College;
                user.College = College?.Title;

                var Teachers = await _userManager.GetUsersInRoleAsync(DataLayer.Tools.RoleName_enum.GuideMaster.ToString());

                if (data.Teacher_1 != null && data.Teacher_1 != 0)
                {
                    var teacher1 = Teachers.Where(o => o.Id == data.Teacher_1.Value).FirstOrDefault();
                    if (teacher1 != null)
                    {
                        // Delete All Teachers
                        var teacherOfUser = await _context.Teachers.Where(o => o.StudentId == user.Id).ToListAsync();
                        _context.Teachers.RemoveRange(teacherOfUser);
                        await _context.SaveChangesAsync();

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
                    var teacher2 = Teachers.Where(o => o.Id == data.Teacher_2.Value).FirstOrDefault();
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
                    var teacher3 = Teachers.Where(o => o.Id == data.Teacher_3.Value).FirstOrDefault();
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
                    Abstract = data.Abstract,
                    RegisterDateTime = DateTime.Now.ToPersianDateTime()
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

                // Valid Files

                // Upload File
                if (IsValidFile(Dissertation_File.FileName, true))
                {
                    var DissertationFile = await _uploadFile.UploadFileAysnc(Dissertation_File);
                    if (DissertationFile != null)
                    {
                        NewDissertation.DissertationFileName = DissertationFile.FileName;
                        NewDissertation.DissertationFileAddress = DissertationFile.FileAddress;
                    }
                    else
                        res.ErrorList.Add("فایل پایان نامه ثبت نشد");
                }
                else
                    res.ErrorList.Add("فایل پایان نامه ثبت نشد");
                if (IsValidFile(Dissertation_File.FileName, true))
                {
                    var ProFile = await _uploadFile.UploadFileAysnc(Pro_File);
                    if (ProFile != null)
                    {
                        NewDissertation.ProceedingsFileName = ProFile.FileName;
                        NewDissertation.ProceedingsFileAddress = ProFile.FileAddress;
                    }
                    else
                        res.ErrorList.Add("فایل تاییدیه ثبت نشد");
                }
                else
                    res.ErrorList.Add("فایل پایان نامه ثبت نشد");

                NewDissertation.StatusDissertation = (int)DataLayer.Tools.Dissertation_Status.Register;

                // Add Dissertation In user
                if (res.ErrorList.Count == 0)
                {
                    user.Dissertations.Add(NewDissertation);
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                    res.Message = "عمليات موفقيت آمیز بود";
                    res.IsValid = true;
                }
                #region log For Update User
                await _historyManager.InsertHistory(DateTime.Now.ToPersianDateTime()
                    , _contextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                        this._contextAccessor.HttpContext.Request.Path, BusinessLayer.Utilities.Utility.Level_log.Informational.ToString(),
                        _contextAccessor?.HttpContext?.Request?.Headers["sec-ch-ua"].ToString(),
                        $"اطلاعات کاربر {user.UserName} بروز شده است و پایان نامه با موفقیت بارگزاري شد");
                #endregion

                #region Send Email
                //var resultEmail = await _emailService.SendEmailAsync(user.Email,
                //    "پیش ثبت نام", "پیش ثبت نام شما در سامانه پایان نامه دانشگاه تربیت دبیر شهید رجایی با موفقیت انجام شد.");
                _historyManager.SendEmail(user.Email,
                    "پیش ثبت نام", "پیش ثبت نام شما در سامانه پایان نامه دانشگاه تربیت دبیر شهید رجایی با موفقیت انجام شد.");
                #endregion

            }
            catch (Exception ex)
            {
                res.Title = "خطا در اجراي برنامه";
                res.Message = ex.Message;
            }
            return res;
        }

        public bool IsValidFile(string FileName, bool IsMainDissertation = false)
        {
            FileInfo _fileInfo = new FileInfo(FileName);
            if (IsMainDissertation)
            {
                List<string> ValidExtentionOfDissertation = new List<string>
                {
                    ".zip",".rar",".rar4",".doc",".docm",".docx",".dot",".dotx"
                };

                if (ValidExtentionOfDissertation.Any(o => o.ToLower() == _fileInfo.Extension.ToLower()))
                    return true;
                return false;
            }
            else
            {
                List<string> ValidExtentionOfPro = new List<string>
                {
                    ".jpg" ,".jpeg" , ".jfif" , ".pjpeg" , ".pjp",".png",".webp",".svg"
                };
                if (ValidExtentionOfPro.Any(o => o.ToLower() == _fileInfo.Extension.ToLower()))
                    return true;
                return false;
            }
        }



        // Get Dissertation => گرفتن پایان نامه ثبت شده بدون تاییدیه
        public async Task<List<Models.OUTPUT.Dissertation.DissertationModelOutPut>?> GetCurrentDissertation(long User_Id)
        {
            var lstDissertations = new List<Models.OUTPUT.Dissertation.DissertationModelOutPut>();
            try
            {
                if (User_Id == 0)
                    return null;

                var user = await _context.Users.Where(o => o.Id == User_Id).FirstOrDefaultAsync();
                if (user == null)
                    return null;

                if (!(await _userManager.IsInRoleAsync(user, DataLayer.Tools.RoleName_enum.Student.ToString())))
                    return null;

                lstDissertations = await _context.Dissertations.Where(o => o.StudentId == User_Id)
                    .Select(o => new Models.OUTPUT.Dissertation.DissertationModelOutPut
                    {
                        Abstract = o.Abstract,
                        DateStr = o.RegisterDateTime.HasValue ? o.RegisterDateTime.Value.ToShortDateString() : "",
                        TimeStr = o.RegisterDateTime.HasValue ? o.RegisterDateTime.Value.ToShortTimeString() : "",
                        DissertationFileAddress = o.DissertationFileAddress,
                        ProceedingsFileAddress = o.ProceedingsFileAddress,
                        DissertationId = o.DissertationId,
                        StudentId = o.StudentId.Value,
                        StatusDissertation = o.StatusDissertation,
                        TermNumber = o.TermNumber,
                        TitleEnglish = o.TitleEnglish,
                        TitlePersian = o.TitlePersian
                    }).ToListAsync();

                var allStatus = await _generalService.GetAllDissertationStatus();
                if (allStatus != null && allStatus.Count > 0)
                {
                    lstDissertations.ForEach(o =>
                    {
                        o.DisplayStatusDissertation = allStatus.Where(t => t.Code == o.StatusDissertation).FirstOrDefault()?.Title;
                    });
                }
            }
            catch
            {
                return null;
            }
            return lstDissertations;
        }

        public async Task<ErrorsVM> UpdateDissertation(IFormFile DissertationFile, IFormFile ProFile, UpdateDissertationDTO UDissertation)
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
                var Teachers = await _userManager.GetUsersInRoleAsync(DataLayer.Tools.RoleName_enum.GuideMaster.ToString());
                if (UDissertation.Teacher1 != null && UDissertation.Teacher1 > 0)
                {
                    var teacher = Teachers.Where(o => o.Id == UDissertation.Teacher1).FirstOrDefault();
                    if (teacher != null)
                    {
                        // Delete Teachers of User
                        var TeachersOfUser = await _context.Teachers.Where(o => o.StudentId == user.Id).ToListAsync();
                        _context.Teachers.RemoveRange(TeachersOfUser);
                        await _context.SaveChangesAsync();

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
                    var teacher = Teachers.Where(o => o.Id == UDissertation.Teacher2).FirstOrDefault();
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
                    var teacher = Teachers.Where(o => o.Id == UDissertation.Teacher3).FirstOrDefault();
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

                if (!UDissertation.CollegeRef.HasValue)
                {
                    var _college = await _context.Baslookups.Where(o => o.Id == UDissertation.CollegeRef
                     && o.Type.ToLower() == DataLayer.Tools.BASLookupType.CollegesUni.ToString().ToLower()).FirstOrDefaultAsync();
                    user.CollegeRef = UDissertation.CollegeRef;
                    user.CollegeRefNavigation = _college;
                    user.College = _college?.Title;
                }

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
                    if (!UDissertation.TitleEnglish.IsNullOrEmpty())
                        Dissertation.TitleEnglish = UDissertation.TitleEnglish;

                    if (!UDissertation.TitlePersian.IsNullOrEmpty())
                        Dissertation.TitlePersian = UDissertation.TitlePersian;

                    if (!UDissertation.TermNumber.IsNullOrEmpty())
                        Dissertation.TermNumber = UDissertation.TermNumber;

                    if (!UDissertation.Abstract.IsNullOrEmpty())
                        Dissertation.Abstract = UDissertation.Abstract;

                    if (!UDissertation.TermNumber.IsNullOrEmpty())
                        Dissertation.TermNumber = UDissertation.TermNumber;

                    Dissertation.RegisterDateTime = DateTime.Now.ToPersianDateTime();
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

                    #region Set Log
                    await _historyManager.InsertHistory(DateTime.Now.ToPersianDateTime(),
                            this._contextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                            this._contextAccessor.HttpContext.Request.Path, BusinessLayer.Utilities.Utility.Level_log.Informational.ToString(),
                            Newtonsoft.Json.JsonConvert.SerializeObject(_contextAccessor?.HttpContext?.Request?.Headers.ToList()),
                            $"پایان نامه {Dissertation.DissertationId} با موفقیت برای کاربر {user.Id} به روز رسانی شد");
                    #endregion


                }
                else
                    res.Message = "برای کاربر پایان نامه جاری وجود ندارد";
            }
            catch (Exception ex)
            {
                res.Title = "خطا در اجرای برنامه";
                res.Message = ex.Message;
                #region Set Log
                await _historyManager.InsertHistory(DateTime.Now.ToPersianDateTime(),
                        this._contextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                        this._contextAccessor.HttpContext.Request.Path, BusinessLayer.Utilities.Utility.Level_log.Informational.ToString(),
                        _contextAccessor?.HttpContext?.Request?.Headers["sec-ch-ua"].ToString(),
                        $"Error Message : {ex.Message}");
                #endregion

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
                        Dissertation.EditDateTime = DateTime.Now.ToPersianDateTime();
                        Dissertation.UpdateCnt++;
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


        public async Task<List<Models.OUTPUT.Dissertation.DissertationModelOutPut>?> GetAllDissertationOfTeacher(long TeacherRef)
        {
            var lstDissertations = new List<Models.OUTPUT.Dissertation.DissertationModelOutPut>();
            try
            {
                var user = await _context.Users.Where(o => o.Id == TeacherRef).FirstOrDefaultAsync();
                if (user == null)
                    return null;

                if (!(await _userManager.IsInRoleAsync(user, DataLayer.Tools.RoleName_enum.GuideMaster.ToString())))
                    return null;

                lstDissertations = await _context.Dissertations.Include(o => o.Student).ThenInclude(x => x.Teachers)
                    .Where(o => o.Student.Teachers.Count > 0
                            && o.StatusDissertation == (int)DataLayer.Tools.Dissertation_Status.Register
                            && o.Student.Teachers.Any(t => t.TeacherId == TeacherRef))
                    .Select(o => new Models.OUTPUT.Dissertation.DissertationModelOutPut
                    {
                        Abstract = o.Abstract,
                        DateStr = o.RegisterDateTime.HasValue ? o.RegisterDateTime.Value.ToShortDateString() : "",
                        TimeStr = o.RegisterDateTime.HasValue ? o.RegisterDateTime.Value.ToShortTimeString() : "",
                        DissertationFileAddress = o.DissertationFileAddress,
                        ProceedingsFileAddress = o.ProceedingsFileAddress,
                        DissertationId = o.DissertationId,
                        StudentId = o.StudentId.Value,
                        StatusDissertation = o.StatusDissertation,
                        TermNumber = o.TermNumber,
                        TitleEnglish = o.TitleEnglish,
                        TitlePersian = o.TitlePersian
                    }).ToListAsync();

                var allStatus = await _generalService.GetAllDissertationStatus();
                if (allStatus != null && allStatus.Count > 0)
                {
                    lstDissertations.ForEach(o =>
                    {
                        o.DisplayStatusDissertation = allStatus.Where(t => t.Code == o.StatusDissertation).FirstOrDefault()?.Title;
                    });
                }
            }
            catch
            {
                return null;
            }
            return lstDissertations;
        }

    }
}
