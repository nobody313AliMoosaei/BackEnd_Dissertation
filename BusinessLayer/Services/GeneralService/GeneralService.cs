using BusinessLayer.Models;
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

        public GeneralService(DataLayer.DataBase.Context_Project context, Teacher.ITeacherManager teacherManager
            , SignInManager<Users> signinmanager, UserManager<Users> usermanager, UploadFile.IUploadFile uploadFile
            , Log.IHistoryManager historyManager, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _teacherManager = teacherManager;
            _signinManager = signinmanager;
            _userManager = usermanager;
            _uploadFile = uploadFile;
            _historyManager = historyManager;
            _contextAccessor = contextAccessor;
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

                var Dis = await _context.Dissertations.Where(o => o.DissertationId == DissertationID
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
                var tttt = await _context.Comments.FromSqlRaw("select * from comments")
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
                    }).ToListAsync();

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
                        User_UserName = o.UserRefNavigation.UserName
                    }).ToListAsync();

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

    }
}
