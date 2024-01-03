﻿using BusinessLayer.Models;
using BusinessLayer.Models.OUTPUT;
using BusinessLayer.Models.OUTPUT.Administrator;
using BusinessLayer.Models.OUTPUT.General;
using BusinessLayer.Models.OUTPUT.Teacher;
using BusinessLayer.Services.UploadFile;
using BusinessLayer.Utilities;
using DataLayer.Entities;
using DataLayer.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.GeneralService
{
    public class GeneralService : IGeneralService
    {
        private DataLayer.DataBase.Context_Project _context;
        private UserManager<Users> _userManager;
        private SignInManager<Users> _signinManager;
        private Services.Teacher.ITeacherManager _teacherManager;
        private Services.UploadFile.IUploadFile _uploadFile;
        private BusinessLayer.Services.Log.IHistoryManager _historyManager;
        private IHttpContextAccessor _contextAccessor;
        private BusinessLayer.Services.Email.IEmailSender _emailService;

        public GeneralService(DataLayer.DataBase.Context_Project context, Teacher.ITeacherManager teacherManager
            , SignInManager<Users> signinmanager, UserManager<Users> usermanager, UploadFile.IUploadFile uploadFile
            , Log.IHistoryManager historyManager, IHttpContextAccessor contextAccessor, Email.IEmailSender emailService)
        {
            _context = context;
            _teacherManager = teacherManager;
            _signinManager = signinmanager;
            _userManager = usermanager;
            _uploadFile = uploadFile;
            _historyManager = historyManager;
            _contextAccessor = contextAccessor;
            _emailService = emailService;
        }

        public async Task<ErrorsVM> AddRoleToUser(Users? user, string RoleUser)
        {
            var Err = new ErrorsVM();
            try
            {
                if (user == null || RoleUser.IsNullOrEmpty())
                {
                    Err.Message = "اطلاعات مورد نیاز ارسال نشده";
                    return Err;
                }
                else
                {
                    var result = await _userManager.AddToRoleAsync(user, RoleUser);
                    if (result.Succeeded)
                    {
                        Err.ErrorList.Add("نقش با موفقيت افزوده شد");
                        Err.IsValid = true;
                    }
                    else
                        Err.ErrorList.Add("اضافه کردن نقش انجام نشد");
                }
            }
            catch (Exception ex)
            {
                Err.Title = "خطا در اجراي برنامه";
                Err.ErrorList.Add(ex.Message);
                if (ex.InnerException != null)
                    Err.ErrorList.Add(ex.InnerException.Message);
            }
            return Err;
        }

        public async Task<ErrorsVM> ChangeDissertationStatus(long DissertationID, string DissertationStatusId)
        {
            var Err = new ErrorsVM();
            try
            {
                if (DissertationStatusId.IsNullOrEmpty())
                {
                    Err.Message = "وضعیت نامعلوم است";
                    return Err;
                }

                var Dis = await _context.Dissertations.Include(o => o.Student).Where(o => o.DissertationId == DissertationID
                && o.StatusDissertation >= (int)DataLayer.Tools.Dissertation_Status.Register).FirstOrDefaultAsync();

                if (Dis != null)
                {
                    var Status = (await GetAllDissertationStatus()).Where(o => o.Id == DissertationStatusId.Val32()).FirstOrDefault();
                    if (Status != null)
                    {
                        Dis.StatusDissertation = Status.Code;
                        Dis.UpdateCnt++;
                        Dis.EditDateTime = DateTime.Now.ToPersianDateTime();
                        _context.Dissertations.Update(Dis);
                        await _context.SaveChangesAsync();

                        // send Email
                        var ResultEmail = await _emailService.SendEmailAsync
                            (Dis.Student.Email, "تغییر وضعیت پایان نامه", $"وضعیت پایان نامه شما به {Status.Title} تغییر پیدا کرده است.");

                        if (ResultEmail)
                            Err.Message = "تغییر وضعیت انجام شد و ایمیل ارسال شد";
                        else
                            Err.Message = "تغییر وضعیت انجام شد";
                        Err.IsValid = true;

                        #region Set Log
                        await _historyManager.InsertHistory(DateTime.Now.ToPersianDateTime(),
                                this._contextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                                this._contextAccessor.HttpContext.Request.Path, BusinessLayer.Utilities.Utility.Level_log.Informational.ToString(),
                                _contextAccessor?.HttpContext?.Request?.Headers["sec-ch-ua"].ToString(),
                                $"پایان نامه {Dis.DissertationId} به وضعیت {Status.Title} تغییر کرد");
                        #endregion


                    }
                    else
                        Err.Message = "وضعیت مناسب نمی‌باشد";
                }
                else
                    Err.Message = "پایان نامه وجود ندارد";

            }
            catch (Exception ex)
            {
                Err.Message = "خطا در اجرای برنامه";
                Err.ErrorList.Add(ex.Message);
                if (ex.InnerException != null)
                    Err.ErrorList.Add(ex.InnerException.Message);

                #region Set Log
                await _historyManager.InsertHistory(DateTime.Now.ToPersianDateTime(),
                        this._contextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                       this._contextAccessor.HttpContext.Request.Path, BusinessLayer.Utilities.Utility.Level_log.Error.ToString(),
                        _contextAccessor?.HttpContext?.Request?.Headers["sec-ch-ua"].ToString(),
                        $"Error Message : {ex.Message}");
                #endregion

            }
            return Err;
        }

        public async Task<string?> DisplayCollegeUni(int code)
        {
            try
            {
                return (await _context.Baslookups.Where(o => o.Type == DataLayer.Tools.BASLookupType.CollegesUni.ToString()
                && o.Code == code).FirstOrDefaultAsync())?.Title;
            }
            catch { }
            return null;
        }

        public async Task<string?> DisplayDissertationstatus(int code)
        {
            try
            {
                return (await _context.Baslookups.Where(o => o.Type == DataLayer.Tools.BASLookupType.DissertationStatus.ToString()
                && o.Code == code).FirstOrDefaultAsync())?.Title;
            }
            catch { }
            return null;
        }

        public async Task<List<Baslookup>?> GetAllCollegeUni()
        {
            var model = new List<Baslookup>();
            try
            {
                model = await _context.Baslookups.Where(o => o.Type == DataLayer.Tools.BASLookupType.CollegesUni.ToString()).ToListAsync();
            }
            catch { }
            return model;
        }

        public async Task<List<Models.OUTPUT.Administrator.StatusModelDTO>> GetAllDissertationStatus()
        {
            var model = new List<Models.OUTPUT.Administrator.StatusModelDTO>();
            try
            {
                model = await _context.Baslookups.Where(o => o.Type.ToLower()
                == DataLayer.Tools.BASLookupType.DissertationStatus.ToString().ToLower())
                    .Select(o => new Models.OUTPUT.Administrator.StatusModelDTO
                    {
                        Id = o.Id,
                        Code = o.Code.HasValue ? o.Code.Value : -1,
                        Title = o.Title
                    })
                    .ToListAsync();
            }
            catch { }
            return model;
        }

        public async Task<List<TeacherOutModelDTO>> GetAllTeacher(string Value = "")
        {
            return await _teacherManager.GetAllTeachers(Value);
        }

        public async Task<List<TeacherOutModelDTO>> GetCollegeTeacher(long CollegeId)
        {
            return await _teacherManager.GetTeachersCollege(CollegeId);
        }

        public async Task<ErrorsVM> LogOut()
        {
            var Err = new ErrorsVM();
            try
            {
                await _signinManager.SignOutAsync();
                Err.Message = "خارج شد";
                Err.IsValid = true;
            }
            catch (Exception ex)
            {
                Err.Title = "خطا در اجرای برنامه";
                Err.ErrorList.Add(ex.Message);
                if (ex.InnerException != null)
                    Err.ErrorList.Add(ex.InnerException.Message);
            }
            return Err;
        }

        public async Task<ResultUploadFile?> UploadFile(IFormFile MainFile)
        {
            return await _uploadFile.UploadFileAysnc(MainFile);
        }

        public async Task<List<StatusModelDTO>> GetStatus(string StatusType)
        {
            var model = new List<StatusModelDTO>();
            try
            {
                if (StatusType.IsNullOrEmpty())
                    return model;
                model = await _context.Baslookups.Where(o => o.Type.ToLower().Trim() == StatusType.ToLower().Trim())
                    .Select(o => new StatusModelDTO
                    {
                        Id = o.Id,
                        Code = o.Code.HasValue ? o.Code.Value : -1,
                        Title = o.Title
                    }).ToListAsync();
            }
            catch
            {

            }
            return model;
        }

        public async Task<List<StatusModelDTO>> GetAllRoles()
        {
            var model = new List<StatusModelDTO>();
            try
            {
                model = await _context.Roles
                    .Select(o => new StatusModelDTO
                    {
                        Id = o.Id,
                        Title = o.PersianName,
                    }).ToListAsync();
            }
            catch
            {

            }
            return model;
        }

        public async Task<List<StatusModelDTO>> GetAllCollegesUni()
        {
            var model = new List<StatusModelDTO>();
            try
            {
                model = await _context.Baslookups
                    .Where(o => o.Type.ToLower() == DataLayer.Tools.BASLookupType.CollegesUni.ToString().ToLower())
                    .Select(o => new StatusModelDTO
                    {
                        Id = o.Id,
                        Code = o.Code.HasValue ? o.Code.Value : 0,
                        Title = o.Title
                    }).ToListAsync();
            }
            catch
            {

            }
            return model;
        }

        public async Task<ErrorsVM> SendComment(long UserId, long DissertationId, string Title, string Dsr, long CommentId = 0)
        {
            var Err = new ErrorsVM();
            try
            {
                if (Title.IsNullOrEmpty() || Dsr.IsNullOrEmpty())
                {
                    Err.Message = "تیتر یا توضحات کامنت نمی‌تواند خالی باشد";
                    return Err;
                }

                var Dissertation = await _context.Dissertations.Where(o => o.DissertationId == DissertationId
                 && o.StatusDissertation >= (int)DataLayer.Tools.Dissertation_Status.Register)
                    .Include(o => o.Comments)
                    .FirstOrDefaultAsync();
                if (Dissertation == null)
                {
                    Err.Message = "پایان نامه‌ای یافت نشد";
                }
                else
                {
                    var comment = new Comments
                    {
                        Title = Title,
                        Description = Dsr,
                        InsertDateTime = DateTime.Now.ToPersianDateTime(),
                        UserRef = UserId,
                    };

                    if (CommentId == 0)//add Comment
                    {
                        Dissertation.Comments.Add(comment);
                    }
                    else // Replay
                    {
                        //Dissertation.Comments.Where(o => o.Id == CommentId).FirstOrDefault()?
                        //    .InverseInversCommentRefNavigation.Add(new Comments
                        //    {
                        //        Title = Title,
                        //        Description = Dsr,
                        //    });
                    }
                    _context.Dissertations.Update(Dissertation);
                    await _context.SaveChangesAsync();
                    Err.Message = "کامنت ثبت شد";
                    Err.IsValid = true;

                    #region Set Log
                    await _historyManager.InsertHistory(DateTime.Now.ToPersianDateTime(),
                            this._contextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                            this._contextAccessor.HttpContext.Request.Path, BusinessLayer.Utilities.Utility.Level_log.Informational.ToString(),
                            _contextAccessor?.HttpContext?.Request?.Headers["sec-ch-ua"].ToString(),
                            $"کامنت برای پایان نامه {Dissertation.DissertationId} توسط کاربر {UserId} ثبت شد");
                    #endregion


                }
            }
            catch (Exception ex)
            {
                Err.ErrorList.Add(ex.Message);
                if (ex.InnerException != null)
                    Err.ErrorList.Add(ex.InnerException.Message);
                #region Set Log
                await _historyManager.InsertHistory(DateTime.Now.ToPersianDateTime(),
                        this._contextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                        this._contextAccessor.HttpContext.Request.Path, BusinessLayer.Utilities.Utility.Level_log.Error.ToString(),
                        _contextAccessor?.HttpContext?.Request?.Headers["sec-ch-ua"].ToString(),
                        $"Error Message : {ex.Message}");
                #endregion

            }
            return Err;
        }

        public DownloadOutModelDTO? DownloadFileFormRoot(string FileAddress)
        {
            var model = new DownloadOutModelDTO();
            try
            {
                if (!FileAddress.IsNullOrEmpty())
                {
                    var File_Info = new FileInfo(FileAddress);
                    model.FileStream = System.IO.File.OpenRead(FileAddress);
                    //string contentType = "application/octet-stream";

                    model.FileDownloadName = ("__" + File_Info.Name).Trim();
                }
            }
            catch
            {
                model = null;
            }
            return model;
        }

        public async Task<UserModelDTO> GetUserById(long UserId)
        {
            var model = new UserModelDTO();
            try
            {
                var AllTeacher = await _teacherManager.GetAllTeachers();

                var user = await _context.Users.Where(o => o.Active == true && o.Id == UserId)
                    .Include(o => o.CollegeRefNavigation)
                    .Include(o => o.Teachers)
                    .FirstOrDefaultAsync();

                if (user.Teachers.Count > 0)
                {
                    model.TeachersName = (await _teacherManager.GetAllTeachers())
                        .Join(user.Teachers, x => x.Id, y => y.TeacherId, (x, y) => { return x; }).Select(o => o.FirstName + " " + o.LastName).ToList();
                }
                model.FirsName = user.FirstName;
                model.LastName = user.LastName;
                model.CollegeName = user.College;
                model.CollegeRef = user.CollegeRef.HasValue ? user.CollegeRef.Value : -1;
                model.NationalCode = user.NationalCode;
                model.UserId = user.Id;
                model.UserName = user.UserName;
                model.PhoneNumber = user.PhoneNumber;
                model.Email = user.Email;
            }
            catch
            {
            }
            return model;
        }

        public async Task<List<CommentOutPutModelDTO>> GetAllDissertationComments(long DissertationId, int PageNumber, int PageSize)
        {
            var lstComments = new List<CommentOutPutModelDTO>();
            try
            {
                lstComments = await _context.Comments.Where(o => o.DissertationRef == DissertationId)
                    .Include(o => o.UserRefNavigation)
                    .Skip((PageNumber - 1) * PageSize)
                    .Take(PageSize)
                    .Select(o => new CommentOutPutModelDTO
                    {
                        Description = o.Description,
                        Title = o.Title,
                        Id = o.Id,
                        InsertDateTime = DateTime.Now,
                        User_FullName = o.UserRefNavigation.FirstName + " " + o.UserRefNavigation.LastName,
                        User_UserName = o.UserRefNavigation.UserName,
                        UserRef = o.UserRef
                    }).ToListAsync();

                // Join UserRole with Roles
                var RoleInfo = await _context.Roles.Join(_context.UserRoles, x => x.Id, y => y.RoleId, (x, y) => new { Role = x, UserRole = y })
                    .Select(o => new
                    {
                        o.UserRole.RoleId,
                        o.UserRole.UserId,
                        o.Role.Name
                    }).ToListAsync();

                lstComments.ForEach(o =>
                {
                    o.User_Roles = RoleInfo.Where(t => t.UserId == o.UserRef).Select(p => p.Name).ToList();
                });


                if (lstComments.Count == 0)
                {
                    var aaaa = await _context.Dissertations.Where(o => o.DissertationId == DissertationId)
                        .Include(o => o.Comments)
                        .FirstOrDefaultAsync();

                    lstComments = aaaa.Comments
                        .Skip((PageNumber - 1) * PageSize)
                        .Take(PageSize)
                        .Select(o => new CommentOutPutModelDTO
                        {
                            Description = o.Description,
                            Title = o.Title,
                            Id = o.Id,
                            InsertDateTime = DateTime.Now,
                            User_FullName = o.UserRefNavigation.FirstName + " " + o.UserRefNavigation.LastName,
                            User_UserName = o.UserRefNavigation.UserName
                        }).ToList();
                    lstComments.ForEach(o =>
                    {
                        o.User_Roles = RoleInfo.Where(t => t.UserId == o.UserRef).Select(p => p.Name).ToList();
                    });
                }
            }
            catch
            {

            }
            return lstComments;
        }

        public async Task<List<StatusModelDTO>> GetApp_Tables()
        {
            var model = new List<StatusModelDTO>();
            try
            {
                model = await _context.Baslookups.Where(o => o.Type.ToLower() == DataLayer.Tools.BASLookupType.App_Table.ToString().ToLower())
                    .Select(o => new StatusModelDTO
                    {
                        Id = o.Id,
                        Code = 0,
                        Title = o.Description
                    }).ToListAsync();
            }
            catch
            { }
            return model;
        }

        #region Replay Comment
        //public async Task<List<CommentOutPutModelDTO>> GetAllReplayCommentsByCommentId(long DissertationId, long CommentId, int PageNumber, int PageSize)
        //{
        //    var lstComments = new List<CommentOutPutModelDTO>();
        //    try
        //    {
        //        lstComments = _context.Comments.Where(o => o.Id == CommentId)
        //            .Skip((PageNumber - 1) * PageSize)
        //            .Take(PageSize)
        //            //.Include(o=>o.InversCommentRefNavigation)
        //            //.ThenInclude(o=>o.UserRefNavigation)
        //            .Include(o => o.UserRefNavigation)
        //            .Select(o => new CommentOutPutModelDTO
        //            {
        //                Description = o.InversCommentRefNavigation.Description,
        //                Title = o.InversCommentRefNavigation.Title,
        //                Id = o.InversCommentRefNavigation.Id,
        //                InsertDateTime = o.InversCommentRefNavigation.InsertDateTime,

        //                User_FullName = o.InversCommentRefNavigation.UserRefNavigation.FirstName + " "
        //                + o.InversCommentRefNavigation.UserRefNavigation.LastName,

        //                User_UserName = o.InversCommentRefNavigation.UserRefNavigation.UserName
        //            })
        //            .ToList();

        //        //if (lstComments.Count == 0)
        //        //{
        //        //    var aaaa = await _context.Dissertations.Where(o => o.DissertationId == DissertationId)
        //        //        .Include(o => o.Comments)
        //        //        .FirstOrDefaultAsync();

        //        //    lstComments = aaaa.Comments.Select(o => new CommentOutPutModelDTO
        //        //    {
        //        //        Description = o.Description,
        //        //        Title = o.Title,
        //        //        Id = o.Id,
        //        //        InsertDateTime = DateTime.Now,
        //        //        User_FullName = o.UserRefNavigation.FirstName + " " + o.UserRefNavigation.LastName,
        //        //        User_UserName = o.UserRefNavigation.UserName
        //        //    }).ToList();
        //        //}

        //    }
        //    catch
        //    {

        //    }
        //    return lstComments;
        //}

        //public async Task<object> GetExcel()
        //{
        //    _context.Database.ExecuteSqlAsync()
        //}
        #endregion

        public async Task<UserModelDTO> GetDataFromAPI(string NationalCode, DateTime BirthDate)
        {
            try
            {
                DateTime Birthdate = new DateTime(BirthDate.Year, BirthDate.Month, BirthDate.Day, new System.Globalization.PersianCalendar());
                var baseAddress = new System.Uri("https://api.sandbox.faraboom.co/v1/");
                var client = new System.Net.Http.HttpClient { BaseAddress = baseAddress };
                client.DefaultRequestHeaders.AcceptLanguage.Clear();
                client.DefaultRequestHeaders.Add("App-Key", "14290");
                client.DefaultRequestHeaders.Add("Accept-Language", "fa");
                client.DefaultRequestHeaders.Add("Device-Id", "192.168.1.1");
                client.DefaultRequestHeaders.Add("Token-Id", "8wOAgcUOEQPGYuj6CV1bM7E7RePOrrIbtUlGB5B2ZjnYJWJN4rX2NB5GH2ukRt5bYM7z4eGM1n4TTeQJZHChrv4");
                client.DefaultRequestHeaders.Add("CLIENT-DEVICE-ID", "192.168.1.1");
                client.DefaultRequestHeaders.Add("CLIENT-IP-ADDRESS", "192.168.1.1");
                client.DefaultRequestHeaders.Add("CLIENT-USER-AGENT", "User Agent");
                client.DefaultRequestHeaders.Add("CLIENT-USER-ID", "0912212981");
                client.DefaultRequestHeaders.Add("CLIENT-PLATFORM-TYPE", "WEB");
                client.DefaultRequestHeaders.Add("Bank-Id", "LTFRIR");

                var data = new
                {
                    national_code = NationalCode,
                    birth_date = Birthdate
                };
                var ttt = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                var response = await client.PostAsJsonAsync("identity/inquiry/birthDate", data);
                var result = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {

            }
            return new UserModelDTO();
        }

        public async Task<Models.OUTPUT.General.ReportCountSystemDTO?> ReportCountSystem()
        {
            var obj = new ReportCountSystemDTO();
            try
            {
                // تعداد استاد راهنما 
                obj.TeachersCount = (await _userManager.GetUsersInRoleAsync(DataLayer.Tools.RoleName_enum.GuideMaster.ToString())).Count;

                //تعداد دانشجو 
                obj.StudentCount = (await _userManager.GetUsersInRoleAsync(RoleName_enum.Student.ToString())).Count;

                //تداد دانشکده 
                obj.CollegeCount = await _context.Baslookups
                    .Where(o => o.Type.ToLower() == DataLayer.Tools.BASLookupType.CollegesUni.ToString().ToLower()).CountAsync();

                //تعداد پایان نامه ها 
                obj.DissertationCount = await _context.Dissertations.CountAsync();
            }
            catch
            {
                obj = null;
            }
            return obj;
        }

        public async Task<List<Models.OUTPUT.Dissertation.DissertationModelOutPut>> GetAllDissertationOfUesr(long UserId)
        {
            var model = new List<Models.OUTPUT.Dissertation.DissertationModelOutPut>();
            try
            {
                var AllDissertationStatus = await GetAllDissertationStatus();

                model = await _context.Dissertations
                    .Where(o => o.StudentId == UserId)
                    .Select(o => new Models.OUTPUT.Dissertation.DissertationModelOutPut
                    {
                        Abstract = o.Abstract,
                        StudentId = o.StudentId,
                        DateStr = o.RegisterDateTime.HasValue ? o.RegisterDateTime.Value.ToShortDateString() : "",
                        DissertationId = o.DissertationId,
                        DissertationFileAddress = o.DissertationFileAddress,
                        ProceedingsFileAddress = o.ProceedingsFileAddress,
                        StatusDissertation = o.StatusDissertation,
                        TermNumber = o.TermNumber,
                        TimeStr = o.RegisterDateTime.HasValue ? o.RegisterDateTime.Value.ToShortTimeString() : "",
                        TitleEnglish = o.TitleEnglish,
                        TitlePersian = o.TitlePersian,
                    }).ToListAsync();
                model.ForEach(o =>
                {
                    o.DisplayStatusDissertation = (AllDissertationStatus.Where(t => t.Code == o.StatusDissertation.Value).FirstOrDefault() == null) ?
                        "" : AllDissertationStatus.Where(t => t.Code == o.StatusDissertation.Value).FirstOrDefault().Title;
                });
            }
            catch
            {

            }
            return model;
        }


        public async Task<bool> UserIsEmployee(long UserId)
        {
            try
            {
                var user = await _context.Users.Where(o => o.Id == UserId).FirstOrDefaultAsync();
                if (user == null)
                    return false;

                var lstRole = new List<string>()
                {
                    "Administrator","GuideMaster","Adviser","EducationExpert","PostgraduateEducationExpert","DissertationExpert"
                };
                return (await _userManager.GetRolesAsync(user)).Where(o => o != null)
                    .Where(o => lstRole.Any(t => t.ToLower() == o.ToLower())).Count() > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UserIsAdmin(long UserId)
        {
            try
            {
                var user = await _context.Users.Where(o => o.Id == UserId).FirstOrDefaultAsync();
                if (user == null)
                    return false;
                return (await _userManager.GetRolesAsync(user)).Where(o => o != null)
                    .Where(o => o.ToLower() == RoleName_enum.Administrator.ToString().ToLower()).Count() > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UserIsStudent(long UserId)
        {
            try
            {
                var user = await _context.Users.Where(o => o.Id == UserId).FirstOrDefaultAsync();
                if (user == null) return false;
                return (await _userManager.GetRolesAsync(user)).Where(o => o != null)
                    .Where(o => o.ToLower() == RoleName_enum.Student.ToString().ToLower()).Count() > 0;
            }
            catch
            {
                return false;
            }
        }


        //public async Task<ErrorsVM> UpdateUser(long UserId, BusinessLayer.Models.INPUT.SignUp.UpdateUserDTO UpdateUser)
        //{
        //    var res = new ErrorsVM();
        //    try
        //    {
        //        var user = await _context.Users.Include(o=>o.Teachers)
        //            .Where(o=>o.Id==UserId && o.Active== true).FirstOrDefaultAsync();
        //        if(user == null)
        //        {
        //            res.Message = "کاربر وجود ندارد";
        //        }
        //        else
        //        {
        //            user.FirstName = UpdateUser.FirstName;
        //            user.LastName = UpdateUser.LastName;
        //            user.NationalCode = UpdateUser.NationalCode;
        //            user.Email = UpdateUser.EmailAddress;
        //            var resultChangePassword = await _userManager.ChangePasswordAsync(user, user.PasswordHash, UpdateUser.NationalCode);
        //            if(!resultChangePassword.Succeeded)
        //            {
        //                res.Message = "تغییر رمز برای کاربر انجام نشده است";
        //                return res;
        //            }

        //            // college
        //            var college = await _context.Baslookups.Where(o => o.Type.ToLower() == DataLayer.Tools.BASLookupType.CollegesUni.ToString().ToLower()
        //            && o.Id == UpdateUser.CollegeRef).FirstOrDefaultAsync();
        //            if(college == null)
        //            {
        //                res.Message = "دانشکده انتخاب نشده است";
        //                return res;
        //            }
        //            user.CollegeRef = college.Id;
        //            user.College = college.Title;
        //            user.CollegeRefNavigation = college;

        //            // teacher1
        //            var allTeachers = await GetAllTeacher();
        //            if(allTeachers.Count==0)
        //                res.ErrorList.Add("استاد راهنمایی تعریف شده وجود ندارد");

        //            if (UpdateUser.Teacher_1.HasValue && UpdateUser.Teacher_1 != 0)
        //            {
        //                var teacher = allTeachers.Where(o => o.Id == UpdateUser.Teacher_1.Value).FirstOrDefault();
        //                user.Teachers = new List<DataLayer.Entities.Teachers>();
        //                user.Teachers.Add(new Teachers
        //                {
        //                    StudentId = user.Id,
        //                    TeacherId = teacher.
        //                })
        //            }
        //        }
        //    }
        //    catch
        //    {

        //    }
        //    return res;
        //}



    }
}
