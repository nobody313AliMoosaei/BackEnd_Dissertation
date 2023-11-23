using BusinessLayer.Models;
using BusinessLayer.Models.OUTPUT.Administrator;
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


        public GeneralService(DataLayer.DataBase.Context_Project context, Teacher.ITeacherManager teacherManager
            , SignInManager<Users> signinmanager, UserManager<Users> usermanager, UploadFile.IUploadFile uploadFile)
        {
            _context = context;
            _teacherManager = teacherManager;
            _signinManager = signinmanager;
            _userManager = usermanager;
            _uploadFile = uploadFile;
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

        public async Task<ErrorsVM> ChangeDissertationStatus(long DissertationID, string DissertationStatus)
        {
            var Err = new ErrorsVM();
            try
            {
                if (DissertationStatus.IsNullOrEmpty())
                {
                    Err.Message = "وضعیت نامعلوم است";
                    return Err;
                }

                var Dis = await _context.Dissertations.Where(o => o.DissertationId == DissertationID
                && o.StatusDissertation >= (int)DataLayer.Tools.Dissertation_Status.Register).FirstOrDefaultAsync();

                if (Dis != null)
                {
                    var Status = (await GetAllDissertationStatus()).Where(o => o.Code == DissertationStatus.Val32()).FirstOrDefault();
                    if (Status != null)
                    {
                        Dis.StatusDissertation = Status.Code;
                        _context.Dissertations.Update(Dis);
                        await _context.SaveChangesAsync();
                        Err.Message = "تغییر وضعیت انجام شد";
                        Err.IsValid = true;
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

        public async Task<List<TeacherOutModelDTO>> GetAllTeacher()
        {
            return await _teacherManager.GetAllTeachers();
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
                        Title = o.PersianName
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
                if(Title.IsNullOrEmpty() || Dsr.IsNullOrEmpty())
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
                    };

                    if (CommentId == 0)//add Comment
                    {
                        Dissertation.Comments.Add(comment);
                    }
                    else // Replay
                    {
                        Dissertation.Comments.Where(o => o.Id == CommentId).FirstOrDefault()?
                            .InverseInversCommentRefNavigation.Add(new Comments
                            {
                                Title= Title,
                                Description= Dsr,
                            });
                    }
                    _context.Dissertations.Update(Dissertation);
                    await _context.SaveChangesAsync();
                    Err.Message = "کامنت ثبت شد";
                    Err.IsValid = true;
                }
            }
            catch (Exception ex)
            {
                Err.ErrorList.Add(ex.Message);
                if (ex.InnerException != null)
                    Err.ErrorList.Add(ex.InnerException.Message);
            }
            return Err;
        }

    }
}
