using BusinessLayer.Models;
using BusinessLayer.Models.INPUT.Teacher;
using BusinessLayer.Models.OUTPUT.Teacher;
using BusinessLayer.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Teacher
{
    public class TeacherManager : ITeacherManager
    {
        private DataLayer.DataBase.Context_Project _context;
        private UserManager<DataLayer.Entities.Users> _userManager;

        public TeacherManager(DataLayer.DataBase.Context_Project context, UserManager<DataLayer.Entities.Users> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public async Task<ErrorsVM> AddNewTeacher(TeacherInModelDTO newTeacher)
        {
            var Error = new ErrorsVM();
            try
            {
                if (newTeacher != null)
                {
                    if (newTeacher.FirstName.IsNullOrEmpty())
                        Error.ErrorList.Add("وارد کردن نام الزامي است");
                    if (newTeacher.LastName.IsNullOrEmpty())
                        Error.ErrorList.Add("وارد کردن نام خانوادگي الزامي است");
                    if (newTeacher.CollegRef == 0)
                        Error.ErrorList.Add("انتخاب دانشکده براي استاد راهنما الزامي است");
                    if (newTeacher.NationalCode.IsNullOrEmpty())
                        Error.ErrorList.Add("وارد کردن کدملي استاد راهنما الزامي است");

                    if (Error.ErrorList.Count > 0)
                        return Error;

                    var CollegeOfTeacher = await _context.Baslookups.Where(o => o.Type.ToLower() == DataLayer.Tools.BASLookupType.CollegesUni.ToString().ToLower()
                                                                                && o.Id == newTeacher.CollegRef).FirstOrDefaultAsync();
                    if (CollegeOfTeacher != null)
                    {
                        var _newUser = new DataLayer.Entities.Users
                        {
                            FirstName = newTeacher.FirstName,
                            LastName = newTeacher.LastName,
                            NationalCode = newTeacher.NationalCode,
                            UserName = newTeacher.UserName,
                            College = CollegeOfTeacher.Title,
                            CollegeRef = CollegeOfTeacher.Id,
                            CollegeRefNavigation = CollegeOfTeacher
                        };
                        if (Error.ErrorList.Count == 0)
                        {
                            var Result = await _userManager.CreateAsync(_newUser);
                            if (Result.Succeeded && await SetTeacherRole(_newUser))
                            {
                                Error.IsValid = true;
                                Error.Message = "استاد راهنما با موفقيت ثبت شد";
                            }
                            else
                                Error.ErrorList.Add("استاد راهنما ثبت نشد");
                        }
                    }
                    else
                        Error.ErrorList.Add("دانشکده انتخاب شده موجود نيست");
                }
                else
                    Error.Message = "اطلاعاتي براي ثبت کاربر وجود ندارد";
            }
            catch (Exception ex)
            {
                Error.Title = "خطا در اجراي برنامه";
                if (ex.InnerException == null)
                    Error.ErrorList.Add(ex.Message);
                else
                {
                    Error.ErrorList.Add(ex.Message);
                    Error.ErrorList.Add(ex.InnerException.Message);
                }
            }
            return Error;
        }

        public async Task<ErrorsVM> DeleteTeacher(long TeacherId)
        {
            var Error = new ErrorsVM();
            try
            {
                if (TeacherId > 0)
                {
                    var teacher = await _context.Users.Where(o => o.Id == TeacherId && o.Active == true).FirstOrDefaultAsync();
                    if (teacher != null)
                    {
                        if (!await IsTeacher(teacher))
                            Error.Message = "کاربر استاد راهنما نمی‌باشد";
                        else
                        {
                            teacher.Active = false;
                            _context.Users.Update(teacher);
                            await _context.SaveChangesAsync();
                            Error.IsValid = true;
                            Error.Message = "استاد راهنما حذف شد";
                        }
                    }
                    else
                        Error.ErrorList.Add("استاد راهنمايي يافت نشد");
                }
                else
                {
                    Error.ErrorList.Add("شناسه استاد راهنما معتبر نمي‌باشد");
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                    Error.ErrorList.Add($"{ex.Message}");
                else
                {
                    Error.ErrorList.Add($"{ex.Message}");
                    Error.ErrorList.Add($"{ex.InnerException.Message}");
                }
            }
            return Error;
        }

        public async Task<List<TeacherOutModelDTO>> GetAllTeachers(string Value = "")
        {
            var lstTeacher = new List<TeacherOutModelDTO>();
            try
            {
                if (Value.IsNullOrEmpty())
                    lstTeacher = await _context.Users.AsQueryable()
                        .Join(_context.UserRoles, x => x.Id, y => y.UserId, (x, y) => new { user = x, Role = y })
                        .Where(o => o.Role.RoleId == (int)DataLayer.Tools.RoleName_enum.GuideMaster)
                        .Select(o => o.user)
                        .Join(_context.Baslookups, x => x.CollegeRef, y => y.Id, (x, y) => new TeacherOutModelDTO
                        {
                            Id = x.Id,
                            College = y.Title,
                            CollegRef = x.CollegeRef.HasValue ? x.CollegeRef.Value : 0,
                            FirstName = x.FirstName,
                            LastName = x.LastName,
                            NationalCode = x.NationalCode,
                            UserName = x.UserName
                        }).ToListAsync();
                else
                {
                    Value = Value.Trim();
                    lstTeacher = await _context.Users.Where(o => o.FirstName == Value || o.LastName == Value
                                    || o.Email == Value || o.NationalCode == Value || o.UserName == Value
                                    || (o.FirstName + o.LastName).Replace(" ", "") == Value.Replace(" ", ""))
                    .Join(_context.UserRoles, x => x.Id, y => y.UserId, (x, y) => new { user = x, Role = y })
                    .Where(o => o.Role.RoleId == (int)DataLayer.Tools.RoleName_enum.GuideMaster)
                    .Select(o => o.user)
                    .Join(_context.Baslookups, x => x.CollegeRef, y => y.Id, (x, y) => new TeacherOutModelDTO
                    {
                        Id = x.Id,
                        College = y.Title,
                        CollegRef = x.CollegeRef.HasValue ? x.CollegeRef.Value : 0,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        NationalCode = x.NationalCode,
                        UserName = x.UserName
                    })
                    .ToListAsync();
                }
            }
            catch
            {

            }
            return lstTeacher;
        }

        public async Task<TeacherOutModelDTO> GetTeacher(long TeacherId)
        {
            try
            {
                if (TeacherId == 0)
                    return null;

                return (await this.GetAllTeachers()).Where(o => o.Id == TeacherId).FirstOrDefault();
            }
            catch
            {
                return null;
            }
        }

        public async Task<List<TeacherOutModelDTO>> GetTeachersCollege(long CollegeRef)
        {
            var lstTeacher = new List<TeacherOutModelDTO>();
            try
            {
                var Role = await _context.Roles.Where(o => o.Name.ToLower() == DataLayer.Tools.RoleName_enum.GuideMaster.ToString().ToLower()).FirstOrDefaultAsync();
                lstTeacher = await _context.Users.Include(o => o.CollegeRefNavigation)
                    .Join(_context.UserRoles, user => user.Id, UserRole => UserRole.UserId, (x, y) => new { user = x, UserRole = y })
                    .Where(o => o.UserRole.RoleId == Role.Id)
                    .Select(o => o.user)
                    .Where(o => o.Active == true && o.CollegeRef == CollegeRef)
                    .Select(o => new TeacherOutModelDTO
                    {
                         Id = o.Id,
                         FirstName = o.FirstName,
                         LastName = o.LastName,
                         UserName = o.UserName,
                         NationalCode = o.NationalCode,
                         CollegRef = o.CollegeRef.Value,
                         College = o.CollegeRefNavigation.Title
                    }).ToListAsync();

                //lstTeacher = (await _userManager.GetUsersInRoleAsync(DataLayer.Tools.RoleName_enum.GuideMaster.ToString()))
                //     .Where(o => o.Active == true && o.CollegeRef == CollegeRef)
                //     .Select(o => new TeacherOutModelDTO
                //     {
                //         Id = o.Id,
                //         FirstName = o.FirstName,
                //         LastName = o.LastName,
                //         UserName = o.UserName,
                //         NationalCode = o.NationalCode,
                //         CollegRef = o.CollegeRef.Value,
                //         College = o.CollegeRefNavigation.Title
                //     }).ToList();
            }
            catch
            {

            }
            return lstTeacher;
        }

        public async Task<ErrorsVM> UpdateTeacher(long TeacherID, TeacherInModelDTO ModelTeacher)
        {
            var Err = new ErrorsVM();
            try
            {
                if (TeacherID != 0)
                {
                    var teacher = (await _userManager.GetUsersInRoleAsync(DataLayer.Tools.RoleName_enum.GuideMaster.ToString()))
                        .Where(o => o.Id == TeacherID)
                        .FirstOrDefault();
                    if (teacher == null)
                        Err.ErrorList.Add("استاد راهنمايي يافت نشد");
                    else
                    {
                        var colleg = await _context.Baslookups.Where(o => o.Type.ToLower() == DataLayer.Tools.BASLookupType.CollegesUni.ToString().ToLower()
                        && o.Id == ModelTeacher.CollegRef).FirstOrDefaultAsync();
                        if (colleg != null)
                        {
                            teacher.FirstName = ModelTeacher.FirstName;
                            teacher.LastName = ModelTeacher.LastName;
                            teacher.NationalCode = ModelTeacher.NationalCode;
                            teacher.UserName = ModelTeacher.UserName;
                            teacher.CollegeRef = colleg.Id;
                            teacher.College = colleg.Title;
                            teacher.CollegeRefNavigation = colleg;
                            await _userManager.UpdateAsync(teacher);
                            await _context.SaveChangesAsync();
                            Err.IsValid = true;
                            Err.Message = "استاد راهنما به‌روز شد";
                        }
                        else
                            Err.ErrorList.Add("دانشکده معتبر نيست");
                    }
                }
                else
                    Err.ErrorList.Add("شناسه استاد راهنما معتبر نيست");
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                    Err.ErrorList.Add(ex.Message);
                else
                {
                    Err.ErrorList.Add(ex.Message);
                    Err.ErrorList.Add(ex.InnerException.Message);
                }
            }
            return Err;
        }

        private async Task<bool> SetTeacherRole(DataLayer.Entities.Users? _user)
        {
            try
            {
                if (_user != null)
                {
                    var Result = await _userManager.AddToRoleAsync(_user, DataLayer.Tools.RoleName_enum.GuideMaster.ToString());
                    if (Result.Succeeded)
                        return true;
                }
            }
            catch
            {
            }
            return false;
        }

        private async Task<bool> IsTeacher(DataLayer.Entities.Users? _user)
        {
            try
            {
                if (_user == null)
                    return false;

                return await _userManager.IsInRoleAsync(_user, DataLayer.Tools.RoleName_enum.GuideMaster.ToString());
            }
            catch
            {
                return false;
            }
        }
    }
}
