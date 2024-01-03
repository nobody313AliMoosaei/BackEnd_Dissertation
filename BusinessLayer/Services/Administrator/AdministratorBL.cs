using BusinessLayer.Models;
using BusinessLayer.Models.INPUT.SignUp;
using BusinessLayer.Models.OUTPUT.Administrator;
using BusinessLayer.Utilities;
using DataLayer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
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
        private BusinessLayer.Services.Log.IHistoryManager _HistoryService;
        private IHttpContextAccessor _contextAccessor;
        private IConfiguration _configuration;

        public AdministratorBL(DataLayer.DataBase.Context_Project context, Teacher.ITeacherManager teacherManager, GeneralService.IGeneralService generalService
            , UserManager<DataLayer.Entities.Users> usermanager, Log.IHistoryManager historyService, IHttpContextAccessor contextAccessor, IConfiguration configuration)
        {
            _context = context;
            _teacherManager = teacherManager;
            _generalService = generalService;
            _userManager = usermanager;
            _HistoryService = historyService;
            _contextAccessor = contextAccessor;
            _configuration = configuration;

        }

        // مشاهده تمام پایان نامه ها با وضعیت
        public async Task<List<Models.OUTPUT.Dissertation.DissertationModelOutPut>> GetAllDissertation(int PageNumber, int PageSize, BusinessLayer.Models.INPUT.Administrator.FilterDissertationDTO _filterModel)
        {
            var model = new List<Models.OUTPUT.Dissertation.DissertationModelOutPut>();
            try
            {
                Expression<Func<DataLayer.Entities.Dissertations, bool>> _where = o => true;

                if (!_filterModel.Title.IsNullOrEmpty())
                    _where = _where.AndAlso(o => o.TitleEnglish.Trim().Contains(_filterModel.Title.Trim()) || o.TitlePersian.Contains(_filterModel.Title.Trim()));
                if (!_filterModel.UserName.IsNullOrEmpty())
                    _where = _where.AndAlso(o => o.Student.UserName == _filterModel.UserName.Trim());
                if (!_filterModel.FullName.IsNullOrEmpty())
                    _where = _where.AndAlso(o => (o.Student.FirstName + o.Student.LastName).Replace(" ", "").Trim()
                    == _filterModel.FullName.Replace(" ", "").Trim());

                _where = _where.AndAlso(o => o.DissertationId > 0);

                model = await _context.Dissertations
                    .Include(o => o.Student)
                    .Where(_where)
                    .Skip((PageNumber - 1) * PageSize)
                    .Take(PageSize)
                    .Select(o => new Models.OUTPUT.Dissertation.DissertationModelOutPut
                    {
                        DissertationId = o.DissertationId,
                        Abstract = o.Abstract,
                        DateStr = o.RegisterDateTime.Value.ToString(),
                        TitlePersian = o.TitlePersian,
                        TitleEnglish = o.TitleEnglish,
                        TimeStr = o.RegisterDateTime.Value.ToShortTimeString(),
                        TermNumber = o.TermNumber,
                        StudentId = o.StudentId,
                        StatusDissertation = o.StatusDissertation,
                        DissertationFileAddress = o.DissertationFileAddress,
                        ProceedingsFileAddress = o.ProceedingsFileAddress
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
        public async Task<ErrorsVM> ChangeDissertationStatus(long dissertationId, string XDisStatus)
        {
            return await _generalService.ChangeDissertationStatus(dissertationId, XDisStatus);
        }

        public async Task<List<StatusModelDTO>> GetStatusByType(string StatusType)
        {
            return await _generalService.GetStatus(StatusType);
        }

        // مشاهده تمام کاربران
        public async Task<List<Models.OUTPUT.Administrator.UserModelDTO>>
            GetAllUsers(BusinessLayer.Models.INPUT.Administrator.FilterDissertationDTO _filterModel, int PageNumber, int PageSize = 5)
        {
            var model = new List<Models.OUTPUT.Administrator.UserModelDTO>();
            try
            {
                Expression<Func<DataLayer.Entities.Users?, bool>> _where = o => true;

                if (!_filterModel.FullName.IsNullOrEmpty())
                    _where = _where.AndAlso(o => (o.FirstName + o.LastName).Trim().Replace(" ", "") == _filterModel.FullName.Trim().Replace(" ", ""));

                if (!_filterModel.UserName.IsNullOrEmpty())
                    _where = _where.AndAlso(o => o.UserName.Trim() == _filterModel.UserName.Trim());

                var Role = await _context.Roles.Where(o => o.Name.ToLower() == DataLayer.Tools.RoleName_enum.Student.ToString().ToLower()).FirstOrDefaultAsync();

                model = await _context.Users.Include(o => o.Teachers).ThenInclude(o => o.TeacherNavigation)
                    .Join(_context.UserRoles, user => user.Id, UserRole => UserRole.UserId, (x, y) => new { user = x, UserRole = y })
                    .Where(o => o.UserRole.RoleId == Role.Id)
                    .Select(o => o.user)
                    .AsQueryable().Where(_where)
                    .Skip((PageNumber - 1) * PageSize)
                    .Take(PageSize)
                    .Select(o => new Models.OUTPUT.Administrator.UserModelDTO
                    {
                        UserId = o.Id,
                        FirsName = o.FirstName,
                        LastName = o.LastName,
                        PhoneNumber = o.PhoneNumber,
                        NationalCode = o.NationalCode,
                        CollegeName = o.College,
                        CollegeRef = o.CollegeRef != null ? o.CollegeRef.Value : 0,
                        UserName = o.UserName,
                        Active = o.Active,
                        TeachersName = o.Teachers.Count > 0 ? o.Teachers.Select(t => t.TeacherNavigation.FirstName + " " + t.TeacherNavigation.LastName).ToList()
                        : new List<string>()
                    }).ToListAsync();


                //model = (await _userManager.GetUsersInRoleAsync(DataLayer.Tools.RoleName_enum.Student.ToString()))
                //    .AsQueryable().Where(_where)
                //    .Skip((PageNumber - 1) * PageSize)
                //    .Take(PageSize)
                //    .Select(o => new Models.OUTPUT.Administrator.UserModelDTO
                //    {
                //        UserId = o.Id,
                //        FirsName = o.FirstName,
                //        LastName = o.LastName,
                //        PhoneNumber = o.PhoneNumber,
                //        NationalCode = o.NationalCode,
                //        CollegeName = o.College,
                //        CollegeRef = o.CollegeRef != null ? o.CollegeRef.Value : 0,
                //        UserName = o.UserName,
                //        Active = o.Active
                //    }).ToList();

                //model = (await _userManager.GetUsersInRoleAsync(DataLayer.Tools.RoleName_enum.Student.ToString()))
                //        .Where(o => o != null && o.Active == true && (o.FirstName == Value || o.LastName == Value
                //                || o.UserName == Value || o.Email == Value || o.NationalCode == Value
                //                || (o.FirstName + o.LastName).Replace(" ", "") == Value.Replace(" ", "")))
                //        .Select(o => new Models.OUTPUT.Administrator.UserModelDTO
                //        {
                //            UserId = o.Id,
                //            FirsName = o.FirstName,
                //            LastName = o.LastName,
                //            PhoneNumber = o.PhoneNumber,
                //            NationalCode = o.NationalCode,
                //            CollegeName = o.College,
                //            CollegeRef = o.CollegeRef != null ? o.CollegeRef.Value : 0,
                //        }).ToList();


                //if (model.Count > 0)
                //{
                //    var allTeacher = await _context.Teachers
                //        .Include(o => o.StudentNavigation).Include(o => o.TeacherNavigation).ToListAsync();

                //    model.ForEach(o =>
                //    {
                //        o.HasDissertation = _context.Dissertations.Where(t => t.StudentId == o.UserId).Count() > 0;
                //        o.TeachersName = allTeacher.Join(model, x => x.StudentId, y => y.UserId, (x, y) =>
                //        {
                //            return x.TeacherNavigation != null ? (x.TeacherNavigation.FirstName + " " + x.TeacherNavigation.LastName)
                //            : "";
                //        }).ToList();
                //    });

                //}
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

                //if (!NewUser.PhoneNumber.IsNullOrEmpty())
                //    user.PhoneNumber = NewUser.PhoneNumber;

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
                    var teacher = await _context.Users.Where(o => o.Id == NewUser.Teacher1_Ref.Value).FirstOrDefaultAsync();
                    if (teacher != null && (await _userManager.IsInRoleAsync(teacher, DataLayer.Tools.RoleName_enum.GuideMaster.ToString())))
                    {
                        user.Teachers = new HashSet<DataLayer.Entities.Teachers>();
                        DataLayer.Entities.Teachers _teacher = new DataLayer.Entities.Teachers()
                        {
                            StudentId = user.Id,
                            StudentNavigation = user,
                            TeacherId = teacher.Id,
                            TeacherNavigation = teacher
                        };
                        user.Teachers.Add(_teacher);
                    }
                }
                if (NewUser.Teacher2_Ref.HasValue && NewUser.Teacher2_Ref != 0)
                {
                    var teacher = await _context.Users.Where(o => o.Id == NewUser.Teacher2_Ref.Value).FirstOrDefaultAsync();
                    if (teacher != null && (await _userManager.IsInRoleAsync(teacher, DataLayer.Tools.RoleName_enum.GuideMaster.ToString())))
                    {
                        DataLayer.Entities.Teachers _teacher = new DataLayer.Entities.Teachers()
                        {
                            StudentId = user.Id,
                            StudentNavigation = user,
                            TeacherId = teacher.Id,
                            TeacherNavigation = teacher
                        };
                        user.Teachers.Add(_teacher);
                    }
                }
                if (NewUser.Teacher3_Ref.HasValue && NewUser.Teacher3_Ref != 0)
                {
                    var teacher = await _context.Users.Where(o => o.Id == NewUser.Teacher3_Ref.Value).FirstOrDefaultAsync();
                    if (teacher != null && (await _userManager.IsInRoleAsync(teacher, DataLayer.Tools.RoleName_enum.GuideMaster.ToString())))
                    {
                        DataLayer.Entities.Teachers _teacher = new DataLayer.Entities.Teachers()
                        {
                            StudentId = user.Id,
                            StudentNavigation = user,
                            TeacherId = teacher.Id,
                            TeacherNavigation = teacher
                        };
                        user.Teachers.Add(_teacher);
                    }
                }
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                Err.Message = "کاربر با موفقیت بروز شد";
                Err.IsValid = true;

                #region Set Log
                await _HistoryService.InsertHistory(DateTime.Now.ToPersianDateTime(),
                        this._contextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                        this._contextAccessor.HttpContext.Request.Path, BusinessLayer.Utilities.Utility.Level_log.Informational.ToString(),
                        _contextAccessor?.HttpContext?.Request?.Headers["sec-ch-ua"].ToString(),
                        $"کاربر با نام کاربری {NewUser.UserName} بروز شد");
                #endregion



            }
            catch (Exception ex)
            {
                Err.ErrorList.Add(ex.Message);
                if (ex.InnerException != null)
                    Err.ErrorList.Add(ex.InnerException.Message);


                #region Set Log
                await _HistoryService.InsertHistory(DateTime.Now.ToPersianDateTime(),
                        this._contextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                        this._contextAccessor.HttpContext.Request.Path, BusinessLayer.Utilities.Utility.Level_log.Error.ToString(),
                        _contextAccessor?.HttpContext?.Request?.Headers["sec-ch-ua"].ToString(),
                        $"Message Error : {ex.Message}");
                #endregion

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
                    user.Active = !user.Active;
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();

                    Err.Message = "کاربر به روز شد";
                    Err.IsValid = true;
                }
                else
                    Err.Message = "کاربری وجود ندارد";


                #region Set Log
                await _HistoryService.InsertHistory(DateTime.Now.ToPersianDateTime(),
                        this._contextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                        this._contextAccessor.HttpContext.Request.Path, BusinessLayer.Utilities.Utility.Level_log.Informational.ToString(),
                        _contextAccessor?.HttpContext?.Request?.Headers["sec-ch-ua"].ToString(),
                        $"کاربر با شناسه {user.Id} غیر فعال شد");
                #endregion


            }
            catch (Exception ex)
            {
                Err.ErrorList.Add(ex.Message);
                if (ex.InnerException != null)
                    Err.ErrorList.Add(ex.InnerException.Message);

                #region Set Log
                await _HistoryService.InsertHistory(DateTime.Now.ToPersianDateTime(),
                        this._contextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                        this._contextAccessor.HttpContext.Request.Path, BusinessLayer.Utilities.Utility.Level_log.Error.ToString(),
                        _contextAccessor?.HttpContext?.Request?.Headers["sec-ch-ua"].ToString(),
                        $"Message Error : {ex.Message}");
                #endregion

            }
            return Err;
        }

        // افزودن نقش جدید به کاربر
        public async Task<Dictionary<string, string>> GetAllRoles()
        {
            Dictionary<string, string> Roles = new Dictionary<string, string>();
            try
            {
                (await _context.Roles.ToListAsync())
                    .ForEach(o =>
                    {
                        if (o.PersianName.IsNullOrEmpty() == false && o.Name.IsNullOrEmpty() == false)
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

                        _context.UserRoles.Add(new IdentityUserRole<long>
                        {
                            RoleId = NewRole.Val64(),
                            UserId = user.Id
                        });
                        await _context.SaveChangesAsync();
                        Err.Message = "نقش اضافه شد";
                        Err.IsValid = true;


                        #region Set Log
                        await _HistoryService.InsertHistory(DateTime.Now.ToPersianDateTime(),
                                this._contextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                                this._contextAccessor.HttpContext.Request.Path, BusinessLayer.Utilities.Utility.Level_log.Informational.ToString(),
                                _contextAccessor?.HttpContext?.Request?.Headers["sec-ch-ua"].ToString(),
                                $"به کاربر {user.Id} نقش {NewRole} افزوده شد");
                        #endregion


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
                #region Set Log
                await _HistoryService.InsertHistory(DateTime.Now.ToPersianDateTime(),
                        this._contextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                        this._contextAccessor.HttpContext.Request.Path, BusinessLayer.Utilities.Utility.Level_log.Error.ToString(),
                        _contextAccessor?.HttpContext?.Request?.Headers["sec-ch-ua"].ToString(),
                        $"Message Error : {ex.Message}");
                #endregion

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
                    || NewUser.NationalCode.IsNullOrEmpty() || NewUser.UserName.IsNullOrEmpty())
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
                    UserName = NewUser.UserName,
                };

                var CollegeNav = await _context.Baslookups.Where(o => o.Id == (NewUser.CollegeRef.HasValue ? NewUser.CollegeRef.Value : -1)).FirstOrDefaultAsync();
                if (CollegeNav != null)
                {
                    user.College = CollegeNav.Title;
                    user.CollegeRefNavigation = CollegeNav;
                    user.CollegeRef = CollegeNav.Id;
                }


                var ResultAddUser = await _userManager.CreateAsync(user, user.NationalCode ?? "123456");
                if (ResultAddUser.Succeeded)
                {
                    user = await _userManager.FindByNameAsync(NewUser.UserName ?? "");
                    if (user != null)
                    {
                        _context.UserRoles.Add(new IdentityUserRole<long>
                        {
                            RoleId = NewRole.Val64(),
                            UserId = user.Id
                        });
                        await _context.SaveChangesAsync();


                        // Teacher_1
                        if (NewUser.Teacher1_Ref.HasValue)
                        {
                            var teacher = await _context.Users.Where(o => o.Id == NewUser.Teacher1_Ref.Value).FirstOrDefaultAsync();
                            if (teacher != null)
                            {
                                user.Teachers.Add(new Teachers
                                {
                                    StudentId = user.Id,
                                    //StudentNavigation = user,
                                    TeacherId = teacher.Id,
                                    TeacherNavigation = teacher
                                });
                            }
                        }

                        // teacher_2
                        if (NewUser.Teacher2_Ref.HasValue)
                        {
                            var teacher = await _context.Users.Where(o => o.Id == NewUser.Teacher2_Ref.Value).FirstOrDefaultAsync();
                            if (teacher != null)
                            {
                                user.Teachers.Add(new Teachers
                                {
                                    StudentId = user.Id,
                                    //StudentNavigation = user,
                                    TeacherId = teacher.Id,
                                    TeacherNavigation = teacher
                                });
                            }
                        }

                        // teacher_3
                        if (NewUser.Teacher3_Ref.HasValue)
                        {
                            var teacher = await _context.Users.Where(o => o.Id == NewUser.Teacher3_Ref.Value).FirstOrDefaultAsync();
                            if (teacher != null)
                            {
                                user.Teachers.Add(new Teachers
                                {
                                    StudentId = user.Id,
                                    //StudentNavigation = user,
                                    TeacherId = teacher.Id,
                                    TeacherNavigation = teacher
                                });
                            }
                        }

                        _context.Users.Update(user);
                        await _context.SaveChangesAsync();


                        Err.Message = "کاربر ایجاد شد";
                        Err.IsValid = true;

                        #region Set Log
                        await _HistoryService.InsertHistory(DateTime.Now.ToPersianDateTime(),
                                this._contextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                                this._contextAccessor.HttpContext.Request.Path, BusinessLayer.Utilities.Utility.Level_log.Informational.ToString(),
                                _contextAccessor?.HttpContext?.Request?.Headers["sec-ch-ua"].ToString(),
                                $"کاربر با نام کاربری {user.UserName} ایجاد شد");
                        #endregion


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
                #region Set Log
                await _HistoryService.InsertHistory(DateTime.Now.ToPersianDateTime(),
                        this._contextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                        this._contextAccessor.HttpContext.Request.Path, BusinessLayer.Utilities.Utility.Level_log.Error.ToString(),
                        _contextAccessor?.HttpContext?.Request?.Headers["sec-ch-ua"].ToString(),
                        $"Error Message : {ex.Message}");
                #endregion

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

                var user = await _context.Users.Where(o => (o.Email.ToLower()) == Value.ToLower()
                || o.NationalCode == Value || o.UserName == Value
                || (o.FirstName + o.LastName).Replace(" ", "").Trim() == Value.Replace(" ", "").Trim())
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

                    #region Set Log
                    await _HistoryService.InsertHistory(DateTime.Now.ToPersianDateTime(),
                            this._contextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                            this._contextAccessor.HttpContext.Request.Path, BusinessLayer.Utilities.Utility.Level_log.Informational.ToString(),
                            _contextAccessor?.HttpContext?.Request?.Headers["sec-ch-ua"].ToString(),
                            $"پایان نامه برای کاربر {UserId} آپلود شد");
                    #endregion


                }
            }
            catch (Exception ex)
            {
                Err.ErrorList.Add(ex.Message);
                if (ex.InnerException != null)
                    Err.ErrorList.Add(ex.InnerException.Message);
                #region Set Log
                await _HistoryService.InsertHistory(DateTime.Now.ToPersianDateTime(),
                        this._contextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString(),
                        this._contextAccessor.HttpContext.Request.Path, BusinessLayer.Utilities.Utility.Level_log.Informational.ToString(),
                        _contextAccessor?.HttpContext?.Request?.Headers["sec-ch-ua"].ToString(),
                        $"Error Message : {ex.Message}");
                #endregion

            }
            return Err;
        }

        // مشاهده پایان نامه های در جریان و رد شده
        public async Task<List<Models.OUTPUT.Dissertation.DissertationModelOutPut>> GetDissertationsByStatus(int StatusId = 1)
        {
            var model = new List<Models.OUTPUT.Dissertation.DissertationModelOutPut>();
            try
            {
                var DisplayStautsDissertation = await _generalService.GetStatus(DataLayer.Tools.BASLookupType.DissertationStatus.ToString());

                if (StatusId == 1) // پایان نامه های فعال
                {
                    model = await _context.Dissertations
                        .Where(o => o.StatusDissertation >= (int)DataLayer.Tools.Dissertation_Status.Register)
                        .Select(o => new Models.OUTPUT.Dissertation.DissertationModelOutPut
                        {
                            Abstract = o.Abstract,
                            DateStr = o.RegisterDateTime.HasValue ? o.RegisterDateTime.Value.ToShortDateString() : "",
                            DissertationId = o.DissertationId,
                            DissertationFileAddress = o.DissertationFileAddress,
                            ProceedingsFileAddress = o.ProceedingsFileAddress,
                            StudentId = o.StudentId,
                            StatusDissertation = o.StatusDissertation,
                            TermNumber = o.TermNumber,
                            TimeStr = o.RegisterDateTime.HasValue ? o.RegisterDateTime.Value.ToShortTimeString() : "",
                            TitleEnglish = o.TitleEnglish,
                            TitlePersian = o.TitlePersian,
                            DisplayStatusDissertation = ""
                        }).ToListAsync();
                    model = model.Select(o =>
                    {
                        o.DisplayStatusDissertation = DisplayStautsDissertation.Where(t => t.Code == o.StatusDissertation).FirstOrDefault()?.Title;
                        return o;
                    }).ToList();
                }
                else // پایان نامه های غیر فعال
                {
                    model = await _context.Dissertations
                        .Where(o => o.StatusDissertation == (int)DataLayer.Tools.Dissertation_Status.ExpirDissertation)
                        .Select(o => new Models.OUTPUT.Dissertation.DissertationModelOutPut
                        {
                            Abstract = o.Abstract,
                            DateStr = o.RegisterDateTime.HasValue ? o.RegisterDateTime.Value.ToPersianDateTime().ToString() : "",
                            DissertationId = o.DissertationId,
                            DissertationFileAddress = o.DissertationFileAddress,
                            ProceedingsFileAddress = o.ProceedingsFileAddress,
                            StudentId = o.StudentId,
                            StatusDissertation = o.StatusDissertation,
                            TermNumber = o.TermNumber,
                            TimeStr = o.RegisterDateTime.HasValue ? o.RegisterDateTime.Value.ToPersianDateTime().ToLongTimeString() : "",
                            TitleEnglish = o.TitleEnglish,
                            TitlePersian = o.TitlePersian,
                            DisplayStatusDissertation = ""
                        }).ToListAsync();
                    model = model.Select(o =>
                    {
                        o.DisplayStatusDissertation = DisplayStautsDissertation.Where(t => t.Code == o.StatusDissertation).FirstOrDefault()?.Title;
                        return o;
                    }).ToList();
                }
            }
            catch
            {

            }
            return model;
        }

        public async Task<DataSet> ExportTable(long TableID)
        {
            DataSet ds = new DataSet();

            if (TableID == 0)
                return ds;
            var TableName = await _context.Baslookups.Where(o => o.Id == TableID).Select(o => o.Title).FirstOrDefaultAsync();
            if (TableName.IsNullOrEmpty())
                return ds;

            var sqlconn = _configuration["ConnectionStrings:SqlConnection"]?.ToString();
            string getcustomer = $"select * from {TableName}";

            using (SqlConnection scon = new SqlConnection(sqlconn))
            {
                using (SqlCommand cmd = new SqlCommand(getcustomer))
                {
                    cmd.Connection = scon;
                    using (SqlDataAdapter sqlAdapter = new SqlDataAdapter(cmd))
                    {
                        sqlAdapter.Fill(ds);
                    }
                }
            }
            return ds;
        }

        public async Task<ErrorsVM> InsertNewCollege(BusinessLayer.Models.INPUT.Administrator.NewCollegeDTO NewCollege)
        {
            var model = new ErrorsVM();
            try
            {
                // اگر کد صفر یا یک عدد منفی ارسال شود، به صورت اتوماتیک یک کد ست میشود

                var AllColleges = await _generalService.GetAllCollegesUni();

                if (NewCollege.Title.IsNullOrEmpty())
                    model.ErrorList.Add("ارسال اطلاعات جهت ثبت دانشکده درست نیست");

                if (AllColleges.Where(o => o.Code == NewCollege.Code).ToList().Count > 0)
                    model.ErrorList.Add("کد ارسال شده برای ثبت دانشکده موجود است");

                if (model.ErrorList.Count == 0)
                {
                    Baslookup newBasLookup;
                    int NewCode;
                    if (NewCollege.Code <= 0)
                    {
                        NewCode = AllColleges.Max(o => o.Code);
                        newBasLookup = new Baslookup()
                        {
                            Code = NewCode + 1,
                            Title = NewCollege.Title,
                            Type = NewCollege.Type,
                            Description = NewCollege.Dsr
                        };
                    }
                    else
                    {
                        newBasLookup = new Baslookup()
                        {
                            Code = NewCollege.Code,
                            Title = NewCollege.Title,
                            Type = NewCollege.Type,
                            Description = NewCollege.Dsr
                        };
                    }
                    _context.Baslookups.Add(newBasLookup);
                    await _context.SaveChangesAsync();
                    model.IsValid = true;
                    model.Message = "دانشکده با موفقیت ثبت شد";
                }
            }
            catch (Exception ex)
            {
                model.Message = "خطا در اجرای برنامه";
                model.ErrorList.Add(ex.Message);
                if (ex.InnerException != null)
                    model.ErrorList.Add(ex.InnerException.Message);
            }
            return model;
        }

        public async Task<ErrorsVM> DeleteCollegeById(long CollegeRef)
        {
            var model = new ErrorsVM();
            try
            {
                var baslookups = await _context.Baslookups.Where(o => o.Id == CollegeRef).ToListAsync();
                _context.Baslookups.RemoveRange(baslookups);
                await _context.SaveChangesAsync();
                model.IsValid = true;
                model.Message = "دانشکده حذف شد";
            }
            catch (Exception ex)
            {
                model.Message = "خطا در اجرای برنامه";
                model.ErrorList.Add(ex.Message);
                if (ex.InnerException != null)
                    model.ErrorList.Add(ex.InnerException.Message);
            }
            return model;
        }

        public async Task<List<UserModelDTO>> GetUserInRole(long RoleRef)
        {
            var model = new List<UserModelDTO>();
            try
            {
                model = await _context.Users
                    .Include(o => o.Teachers)
                    .ThenInclude(o => o.TeacherNavigation)
                    .Include(o => o.Dissertations)
                    .Join(_context.UserRoles, x => x.Id, y => y.UserId, (x, y) => new { user = x, UserRole = y })
                    .Where(o => o.UserRole.RoleId == RoleRef)
                    .Select(o => new UserModelDTO
                    {
                        FirsName = o.user.FirstName,
                        LastName = o.user.LastName,
                        Active = o.user.Active,
                        CollegeName = o.user.College,
                        CollegeRef = o.user.CollegeRef.HasValue ? o.user.CollegeRef.Value : 0,
                        HasDissertation = o.user.Dissertations.Count > 0,
                        NationalCode = o.user.NationalCode,
                        TeachersName = o.user.Teachers.Where(t => t.TeacherNavigation != null)
                        .Select(t => t.TeacherNavigation.FirstName + " " + t.TeacherNavigation.LastName).ToList(),
                        UserId = o.user.Id,
                        UserName = o.user.UserName
                    })
                    .ToListAsync();
            }
            catch
            {

            }
            return model;
        }

    }
}
