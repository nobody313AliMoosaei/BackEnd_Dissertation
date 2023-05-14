using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dissertation_Project.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("API/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class Teachers_DataController : ControllerBase
    {
        #region IOC

        private DataLayer.DataBase.Context_Project _context;
        private UserManager<DataLayer.Entities.Users> _userManager;
        public Teachers_DataController(DataLayer.DataBase.Context_Project context
            , UserManager<DataLayer.Entities.Users> usermanager)
        {
            _context = context;
            _userManager = usermanager;
        }
        #endregion


        [HttpGet("GetAllTeacher")]
        public async Task<IActionResult> GetAllTeacher()
        {
            return Ok((await _userManager.GetUsersInRoleAsync(DataLayer.Tools.RoleName_enum.GuideMaster.ToString())).Select(t => new Model.DTO.OUTPUT.Teacher_Data.AllTeacher_Model_DTO()
            {
                Id = t.Id,
                Email = t.Email,
                FirstName = t.FirstName,
                LastName = t.LastName,
                NationalCode = t.NationalCode,
                College= t.College,
                UserName=t.UserName
            }).ToList());
        }

        [HttpGet("GetOneTeacherById/{Teacher_id}")]
        public async Task<IActionResult> GetOnTeacherById([FromRoute] ulong Teacher_id)
        {
            var Teacher = (await _userManager.GetUsersInRoleAsync(DataLayer.Tools.RoleName_enum.GuideMaster.ToString())).Select(t => new
            Model.DTO.OUTPUT.Teacher_Data.AllTeacher_Model_DTO()
            {
                Id = t.Id,
                Email = t.Email,
                FirstName = t.FirstName,
                LastName = t.LastName,
                NationalCode = t.NationalCode,
                UserName=t.UserName,
                College=t.College
            }).FirstOrDefault(t => t.Id == Teacher_id);
            if (Teacher == null)
            {
                return BadRequest("چنین استاد راهنمایی وجود ندارد");
            }
            return Ok(Teacher);
        }

        [HttpGet("GetOneTeacherByEmail/{Teacher_Email}")]
        public async Task<IActionResult> GetOneTeacherByEmail([FromRoute] string Teacher_Email)
        {
            var Teacher = (await _userManager
                .GetUsersInRoleAsync(DataLayer.Tools.RoleName_enum.GuideMaster.ToString())
                ).Select(t => new Model.DTO.OUTPUT.Teacher_Data.AllTeacher_Model_DTO()
                {
                    Id = t.Id,
                    Email = t.Email,
                    FirstName = t.FirstName,
                    LastName = t.LastName,
                    NationalCode = t.NationalCode,
                    UserName = t.UserName,
                    College=t.College
                }).FirstOrDefault(t => t.Email == Teacher_Email);

            if (Teacher == null)
            {
                return BadRequest("استاد راهنمایی با این مشخصات یافت نشد");
            }
            return Ok(Teacher);
        }

        // Add Teacher

        [HttpPost("add_Teacher")]
        public async Task<IActionResult> AddTaeacher([FromBody] Model.DTO.INPUT.Teacher_Data.Add_Teacher_DTO RegisterUser)
        {
            if (!ModelState.IsValid)
                return BadRequest("مشخصات ناقص وارد شده است");
            var user = new DataLayer.Entities.Users()
            {
                UserName = RegisterUser.UserName,
                Email = RegisterUser.Email,
                NationalCode = RegisterUser.NationalCode,
                FirstName=RegisterUser.FirstName,
                LastName=RegisterUser.LastName,
                College=RegisterUser.College
            };
            if (string.IsNullOrEmpty(RegisterUser.NationalCode))
            {
                return BadRequest("کد ملی وارد نشده است");
            }
            var resualt = await _userManager.CreateAsync(user, RegisterUser.NationalCode);
            if (resualt.Succeeded)
            {
                var t = await Add_Role_Teacher(user.UserName);
                if (t)
                    return Ok("استاد راهنما ثبت شده است");
            }

            return BadRequest("استاد راهنما ثبت نشده است");
        }


        // Add Role Teacher
        private async Task<bool> Add_Role_Teacher(string Username)
        {
            var user = await _userManager.FindByNameAsync(Username);
            if (user == null)
            {
                return false;
            }
            var resualt = await _userManager.AddToRoleAsync(user, DataLayer.Tools.RoleName_enum.GuideMaster.ToString());
            if (resualt == null)
                return false;
            if (resualt.Succeeded)
                return true;
            return false;
        }

        // Delete Teachers
        [HttpDelete("Delete_Teachers")]
        public async Task<IActionResult> DeleteTeacher(List<ulong> IDs)
        {
            if(IDs.Count>0)
            {
                foreach (var ID in IDs)
                {
                    await _userManager.DeleteAsync(await _userManager.FindByIdAsync(ID.ToString()));
                }
                return Ok("همه حذف شدند");
            }
            return BadRequest();
        }
    }
}
