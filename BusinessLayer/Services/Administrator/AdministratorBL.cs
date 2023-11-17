﻿using BusinessLayer.Models;
using BusinessLayer.Models.INPUT.SignUp;
using BusinessLayer.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Administrator
{
    public class AdministratorBL
    {
        private DataLayer.DataBase.Context_Project _context;
        private Teacher.ITeacherManager _teacherManager;
        private GeneralService.IGeneralService _generalService;
        private UserManager<DataLayer.Entities.Users> _userManager;

        public AdministratorBL(DataLayer.DataBase.Context_Project context, Teacher.ITeacherManager teacherManager, GeneralService.IGeneralService generalService
            , UserManager<DataLayer.Entities.Users> usermanager)
        {
            _context = context;
            _teacherManager = teacherManager;
            _generalService = generalService;
            _userManager = usermanager;
        }

        // مشاهده تمام پایان نامه ها با وضعیت
        public async Task<List<Models.OUTPUT.Dissertation.DissertationModelOutPut>> GetAllDissertation(int PageNumber, int PageSize)
        {
            var model = new List<Models.OUTPUT.Dissertation.DissertationModelOutPut>();
            try
            {
                model = await _context.Dissertations.Where(o => o.DissertationId > 0)
                    .Skip((PageNumber - 1) * PageSize)
                    .Take(PageSize)
                    .Select(o => new Models.OUTPUT.Dissertation.DissertationModelOutPut
                    {
                        DissertationId = o.DissertationId,
                        Abstract = o.Abstract,
                        DateStr = o.DateTime.Value.ToString(),
                        TitlePersian = o.TitlePersian,
                        TitleEnglish = o.TitleEnglish,
                        TimeStr = o.DateTime.Value.ToShortTimeString(),
                        TermNumber = o.TermNumber,
                        StudentId = o.StudentId,
                        StatusDissertation = o.StatusDissertation,
                    }).ToListAsync();


                var disStatus = await _generalService.GetAllDissertationStatus();
                if (disStatus.Any())
                {
                    model = model.Select(o =>
                    {
                        o.DisplayStatusDissertation = disStatus.Where(t => t.Code == o.StatusDissertation).FirstOrDefault()?.Title;
                        return o;
                    }).ToList();
                }
            }
            catch (Exception ex)
            {

            }
            return model;
        }

        // تغییر وضعیت پایان نامه
        public async Task<ErrorsVM> ChangeDissertationStatus(long dissertationId, DataLayer.Tools.Dissertation_Status XDisStatus)
        {
            return await _generalService.ChangeDissertationStatus(dissertationId, XDisStatus);
        }

        // مشاهده تمام کاربران
        public async Task<List<Models.OUTPUT.Administrator.UserModelDTO>> GetAllUsers()
        {
            var model = new List<Models.OUTPUT.Administrator.UserModelDTO>();
            try
            {
                model = (await _userManager.GetUsersInRoleAsync(DataLayer.Tools.RoleName_enum.Student.ToString()))
                    .Where(o => o != null && o.Active == true)
                    .Select(o => new Models.OUTPUT.Administrator.UserModelDTO
                    {
                        UserId = o.Id,
                        FirsName = o.FirstName,
                        LastName = o.LastName,
                        PhoneNumber = o.PhoneNumber,
                        NationalCode = o.NationalCode,
                        CollegeName = o.College,
                        CollegeRef = o.CollegeRef != null ? o.CollegeRef.Value : 0,
                    }).ToList();
                if (model.Count > 0)
                {
                    var allTeacher = await _context.Teachers.ToListAsync();
                    model.ForEach(o =>
                    {
                        o.HasDissertation = _context.Dissertations.Where(t => t.StudentId == o.UserId).Count() > 0;
                        o.TeachersName = allTeacher.Join(model, x => x.StudentId, y => y.UserId, (x, y) =>
                        {

                            return x.TeacherNavigation != null ? (x.TeacherNavigation.FirstName + " " + x.TeacherNavigation.LastName)
                            : "";
                        }).ToList();
                    });
                }
            }
            catch
            {

            }
            return model;
        }

        // ویرایش کاربر
        public async Task<ErrorsVM> UpdateUser(Models.INPUT.Administrator.EditUserDTO NewUser)
        {
            var Err = new ErrorsVM();
            try
            {
                if (NewUser == null || NewUser.UserId == 0)
                    return Err;

                // load User
                var user = await _context.Users.Where(o => o.Id == NewUser.UserId).FirstOrDefaultAsync();
                if (user == null)
                {
                    Err.Message = "کاربر یافت نشد";
                    return Err;
                }

                // Edit
                if (!NewUser.FirstName.IsNullOrEmpty())
                    user.FirstName = NewUser.FirstName;

                if (!NewUser.LastName.IsNullOrEmpty())
                    user.LastName = NewUser.LastName;

                if (!NewUser.NationalCode.IsNullOrEmpty())
                    user.NationalCode = NewUser.NationalCode;

                if (!NewUser.PhoneNumber.IsNullOrEmpty())
                    user.PhoneNumber = NewUser.PhoneNumber;

                if (!NewUser.UserName.IsNullOrEmpty())
                    user.UserName = NewUser.UserName;

                if (NewUser.CollegeRef.HasValue && NewUser.CollegeRef != 0)
                {
                    var college = await _context.Baslookups.Where(o => o.Id == NewUser.CollegeRef.Value).FirstOrDefaultAsync();
                    if (college != null)
                    {
                        user.College = college.Title;
                        user.CollegeRef = college.Id;
                        user.CollegeRefNavigation = college;
                    }
                }
                if (NewUser.Teacher1_Ref.HasValue && NewUser.Teacher1_Ref != 0)
                {
                    var teacher = await _teacherManager.GetTeacher(NewUser.Teacher1_Ref.Value);
                    if (teacher != null)
                    {
                        user.Teachers = new List<DataLayer.Entities.Teachers>();
                        DataLayer.Entities.Teachers _teacher = new DataLayer.Entities.Teachers()
                        {
                            Id = teacher.Id,
                            StudentId = user.Id,
                            StudentNavigation = user,
                            TeacherId = teacher.Id
                        };
                        user.Teachers.Add(_teacher);
                    }
                }
                if (NewUser.Teacher2_Ref.HasValue && NewUser.Teacher2_Ref != 0)
                {
                    var teacher = await _teacherManager.GetTeacher(NewUser.Teacher2_Ref.Value);
                    if (teacher != null)
                    {
                        DataLayer.Entities.Teachers _teacher = new DataLayer.Entities.Teachers()
                        {
                            Id = teacher.Id,
                            StudentId = user.Id,
                            StudentNavigation = user,
                            TeacherId = teacher.Id
                        };
                        user.Teachers.Add(_teacher);
                    }
                }
                if (NewUser.Teacher3_Ref.HasValue && NewUser.Teacher3_Ref != 0)
                {
                    var teacher = await _teacherManager.GetTeacher(NewUser.Teacher3_Ref.Value);
                    if (teacher != null)
                    {
                        DataLayer.Entities.Teachers _teacher = new DataLayer.Entities.Teachers()
                        {
                            Id = teacher.Id,
                            StudentId = user.Id,
                            StudentNavigation = user,
                            TeacherId = teacher.Id
                        };
                        user.Teachers.Add(_teacher);
                    }
                }
                Err.Message = "کاربر با موفقیت بروز شد";
                Err.IsValid = true;
            }
            catch (Exception ex)
            {
                Err.ErrorList.Add(ex.Message);
                if (ex.InnerException != null)
                    Err.ErrorList.Add(ex.InnerException.Message);
            }
            return Err;
        }

        // غیر فعال کردن کاربر
        public async Task<ErrorsVM> DeActiveUser(long UserId)
        {
            var Err = new ErrorsVM();
            try
            {
                var user = await _context.Users.Where(o => o.Id == UserId).FirstOrDefaultAsync();
                if (user != null)
                {
                    user.Active = false;
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();

                    Err.Message = "کاربر به روز شد";
                    Err.IsValid = true;
                }
                else
                    Err.Message = "کاربری وجود ندارد";
            }
            catch (Exception ex)
            {
                Err.ErrorList.Add(ex.Message);
                if (ex.InnerException != null)
                    Err.ErrorList.Add(ex.InnerException.Message);
            }
            return Err;
        }

        // افزودن نقش جدید به کاربر
        public async Task<Dictionary<string, string>> GetAllRoles()
        {
            Dictionary<string, string> Roles = new Dictionary<string, string>();
            try
            {
                (await _context.Roles.Where(o => !o.PersianName.IsNullOrEmpty() && !o.Name.IsNullOrEmpty()).ToListAsync()).ForEach(o =>
                    {
                        Roles.Add(o.Name ?? "", o.PersianName ?? "");
                    });
            }
            catch
            {

            }
            return Roles;
        }
        public async Task<ErrorsVM> AddNewRoleToUser(long UserId, string NewRole)
        {
            var Err = new ErrorsVM();
            try
            {
                if (NewRole.IsNullOrEmpty())
                    Err.Message = "نقشی انتخاب نشده است";
                else
                {
                    var user = await _context.Users.Where(o => o.Id == UserId).FirstOrDefaultAsync();
                    if (user != null)
                    {
                        var result = await _userManager.AddToRoleAsync(user, NewRole);
                        if (result.Succeeded)
                        {
                            Err.Message = "نقش اضافه شد";
                            Err.IsValid = true;
                        }
                        else
                            Err.Message = "نقش اضافه نشد";
                    }
                    else
                        Err.Message = "کاربر پیدا نشد";
                }
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

        // مشاهده تمام اساتید راهنماها
        public async Task GetAllTeachers()
        {
            //return await _teacherManager.GetAllTeachers();
        }

        // ویرایش استاد راهنما 
        public async Task updateTEacher()
        {
            //_teacherManager.UpdateTeacher()
        }

        // افزودن کاربر با نقش دلخواه
        public async Task<ErrorsVM> AddNewUser(Models.INPUT.Administrator.EditUserDTO NewUser, string NewRole)
        {
            var Err = new ErrorsVM();
            try
            {
                if (NewRole.IsNullOrEmpty())
                {
                    Err.ErrorList.Add("انتخاب نقش الزامی می‌باشد");
                    return Err;
                }
                if (NewUser.FirstName.IsNullOrEmpty() || NewUser.LastName.IsNullOrEmpty()
                    || NewUser.NationalCode.IsNullOrEmpty() || NewUser.PhoneNumber.IsNullOrEmpty()
                    || NewUser.UserName.IsNullOrEmpty())
                {
                    Err.ErrorList.Add("اطلاعات شخصی کاربر ناقص است");
                    Err.Message = "اطلاعات کاربر ناقص است";
                    return Err;
                }
                var user = new DataLayer.Entities.Users()
                {
                    FirstName = NewUser.FirstName,
                    LastName = NewUser.LastName,
                    NationalCode = NewUser.NationalCode,
                    PhoneNumber = NewUser.PhoneNumber,
                    UserName = NewUser.UserName
                };
                var ResultAddUser = await _userManager.CreateAsync(user, user.NationalCode ?? "123456");
                if (ResultAddUser.Succeeded)
                {
                    user = await _userManager.FindByNameAsync(NewUser.UserName ?? "");
                    if (user != null)
                    {
                        var ResultAddRole = await _generalService.AddRoleToUser(user, NewRole);
                        if (!ResultAddRole.IsValid)
                            Err.ErrorList.Add("نقش مد نظر به کاربر تخصیص داده نشد");
                        else
                        {
                            Err.Message = "کاربر ایجاد شد";
                            Err.IsValid = true;
                        }
                    }
                    else
                        Err.ErrorList.Add("کاربر یافت نشد");
                }
                else
                {
                    if (ResultAddUser.Errors.Any(o => o.Code == "DuplicateUserName"))
                        Err.ErrorList.Add("کاربر تکراری می‌باشد");
                    else
                        Err.ErrorList.Add("ایجاد چنین کاربری امکان پذیر نیست");
                }
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

        // پیدا کردن کاربر مدنظر
        public async Task<Models.OUTPUT.Administrator.UserModelDTO> GetUserBySearch(string Value) // Email , UserName, NationalCode
        {
            var model = new Models.OUTPUT.Administrator.UserModelDTO();
            try
            {
                if (Value.IsNullOrEmpty())
                {
                    return model;
                }

                var user = await _context.Users.Where(o => (o.Email.IsNullOrEmpty() ? "" : o.Email.ToLower()) == Value.ToLower()
                || o.NationalCode == Value || o.UserName == Value)
                    .Include(o => o.CollegeRefNavigation)
                    .Include(o => o.Teachers)
                    .FirstOrDefaultAsync();
                if (user != null)
                {
                    model.FirsName = user.FirstName;
                    model.LastName = user.LastName;
                    model.PhoneNumber = user.PhoneNumber;
                    model.NationalCode = user.NationalCode;
                    model.UserName = user.UserName;
                    model.CollegeName = user.College;
                    model.CollegeRef = user.CollegeRef.HasValue ? user.CollegeRef.Value : 0;
                    model.UserId = user.Id;

                    // HasDissertation
                    model.HasDissertation = _context.Dissertations.Where(o => o.StudentId == user.Id
                    && o.StatusDissertation >= (int)DataLayer.Tools.Dissertation_Status.Register).Count() > 0;

                    // Teacher s Name
                    var Teachers = await _teacherManager.GetAllTeachers();

                    model.TeachersName =
                        Teachers.Join(user.Teachers, x => x.Id, y => y.TeacherId, (x, y) =>
                        {
                            return x.FirstName + " " + x.LastName;
                        }).ToList();
                }

            }
            catch
            {

            }
            return model;
        }

        // آپلود پایان نامه برای کاربر
        public async Task<ErrorsVM> UploadDissertationForUser(long UserId, Models.INPUT.Administrator.NewDissertationDTO newDissertation, IFormFile Dis_File, IFormFile Pre_File)
        {
            var Err = new ErrorsVM();
            try
            {
                var user = await _context.Users.Where(o => o.Id == UserId && o.Active == true).FirstOrDefaultAsync();
                if (user == null)
                {
                    Err.Message = "کاربری یافت نشد";
                    return Err;
                }

                if (Dis_File == null)
                {
                    Err.Message = "فایل پایان نامه ارسال نشده است";
                    return Err;
                }
                var dis = await _context.Dissertations.Where(o => o.StudentId == UserId
                && o.StatusDissertation >= (int)DataLayer.Tools.Dissertation_Status.Register).FirstOrDefaultAsync();
                if (dis != null)
                {
                    Err.Message = "کاربر پایان نامه در جریان دارد";
                }
                else
                {
                    DataLayer.Entities.Dissertations dissertation = new DataLayer.Entities.Dissertations()
                    {
                        TitlePersian = newDissertation.Title_Persian,
                        TitleEnglish = newDissertation.Title_English,
                        Abstract = newDissertation.Abstract,
                        TermNumber = newDissertation.Term_Number
                    };
                    if (newDissertation.KeyWords.Count > 0)
                    {
                        dissertation.KeyWords = newDissertation.KeyWords.Select(o => new DataLayer.Entities.KeyWord
                        {
                            Word = o
                        }).ToList();
                    }

                    // Upload File

                    var Result_Dis_File = await _generalService.UploadFile(Dis_File);
                    dissertation.DissertationFileName = Result_Dis_File.FileName;
                    dissertation.DissertationFileAddress = Result_Dis_File.FileAddress;
                    if (Pre_File != null)
                    {
                        var Result_Pre_File = await _generalService.UploadFile(Pre_File);
                        dissertation.ProceedingsFileName = Result_Pre_File.FileName;
                        dissertation.ProceedingsFileAddress = Result_Pre_File.FileAddress;
                    }

                    user.Dissertations.Add(dissertation);
                    await _context.SaveChangesAsync();
                    Err.Message = "پایان نامه بارگذاری شد";
                    Err.IsValid = true;
                }
            }
            catch(Exception ex)
            {
                Err.ErrorList.Add(ex.Message);
                if (ex.InnerException != null)
                    Err.ErrorList.Add(ex.InnerException.Message);
            }
            return Err;
        }


    }
}
