using BusinessLayer.Models;
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


        public GeneralService(DataLayer.DataBase.Context_Project context, Teacher.ITeacherManager teacherManager
            , SignInManager<Users> signinmanager, UserManager<Users> usermanager)
        {
            _context = context;
            _teacherManager = teacherManager;
            _signinManager = signinmanager;
            _userManager = usermanager;
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

        public Task<ErrorsVM> ChangeDissertationStatus(long DissertationID, Dissertation_Status DissertationStatus)
        {
            throw new NotImplementedException();
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

        public async Task<List<Baslookup>?> GetAllDissertationStatus()
        {
            var model = new List<Baslookup>();
            try
            {
                model = await _context.Baslookups.Where(o => o.Type == DataLayer.Tools.BASLookupType.DissertationStatus.ToString()).ToListAsync();
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

        public Task<ErrorsVM> SendComment(long UserId, long DissertationId, long CommentId = 0)
        {
            throw new NotImplementedException();
        }

        public Task<ResultUploadFile> UploadFile(IFormFile MainFile)
        {
            throw new NotImplementedException();
        }
    }
}
