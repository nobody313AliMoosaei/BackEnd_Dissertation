using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Dissertation_Project.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("API/v{version:apiVersion}/[controller]")]
    [Authorize(Roles = "Student")]
    public class Letter_PartController : ControllerBase
    {
        #region IOC
        private DataLayer.DataBase.Context_Project _Context;
        private UserManager<DataLayer.Entities.Users> _UserManager;

        public Letter_PartController(DataLayer.DataBase.Context_Project context
            , UserManager<DataLayer.Entities.Users> usermanager)
        {
            _Context = context;
            _UserManager = usermanager;
        }
        #endregion

        /// <summary>
        /// یاید یک دیتایی را دریافت کنیم
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateNewLetter(Model.DTO.INPUT.Letter_Part.New_LetterDTO New_Letter)
        {

            try
            {
                // Validations
                if (New_Letter == null)
                    return BadRequest("دیتایی ارسال نشده است");
                if (!ModelState.IsValid)
                    return BadRequest("ارسال دیتا اشتباه است");

                // لود کردن پایان نامه مربوط به کاربر جاری
                var User_Id = User.Claims.FirstOrDefault(t => t.Type == ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(User_Id))
                {
                    return Unauthorized("کاربر لاگین نکرده است");
                }

                var Dissertaion = await _Context.Dissertations
                    .Include(t => t.Student)
                    .Include(t => t.Comments)
                    .ThenInclude(t => t.Receivers)
                    .Where(t => t.Student.Id == ulong.Parse(User_Id))
                    .FirstOrDefaultAsync();
                if (Dissertaion == null)
                    return BadRequest("کاربر پایان نامه ای ندارد");

                // پیدا کردن کاربران با نقش ارسال شده
                var User_Have_Role = await _UserManager.GetUsersInRoleAsync(New_Letter.RoleReceiver);

                if (User_Have_Role == null
                    || User_Have_Role.Count <= 0)
                    return BadRequest("کاربری با نقش ارسال شده وجود ندارد");


                // ست کردن کامنت برای پایان نامه
                var Comment = new DataLayer.Entities.Comments()
                {
                    Title = New_Letter.Title,
                    Description = New_Letter.Description,
                    Insert_DateTime = DateTime.Now,
                    Sender = await _Context.Users.FirstOrDefaultAsync(t => t.Id == ulong.Parse(User_Id)),
                };
                foreach (var item in User_Have_Role)
                {
                    // برای هر کاربر می توانیم یک ایمیل نیز ارسال کنیم
                    // ....


                    Comment.Receivers.Add(new DataLayer.Entities.Comment_User()
                    {
                        User_Id = item.Id,
                        User = item
                    });
                }

                Dissertaion.Comments.Add(Comment);

                // Save Changes
                _Context.Dissertations.Update(Dissertaion);
                await _Context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                // ---- Log ----

                // ...


                Console.WriteLine(ex.Message);
                return BadRequest($"Fatal Error : {ex.Message}");
            }
        }


        [HttpGet("Received")]
        public async Task<IActionResult> GetReceivedLetters()
        {
            string User_Id = User.Claims.FirstOrDefault(t => t.Type == ClaimTypes.NameIdentifier)?.Value;

            // Validations
            if (string.IsNullOrEmpty(User_Id))
            {
                return Unauthorized();
            }
            if (string.IsNullOrWhiteSpace(User_Id))
            {
                return Unauthorized();
            }

            // لود کامنت که مربوط 
            var Comments = await _Context.Comments
                .Include(t => t.Receivers)
                .ThenInclude(t => t.User)
                .Where(t => t.Receivers.Any(t => t.User_Id == ulong.Parse(User_Id))) // Id = User_Id
                .Select(t => new Model.DTO.OUTPUT.Letter_Part.Receive_Letter_DTO
                {
                    Comment_Id = t.Comment_Id,
                    Title = t.Title,
                    Description = t.Description,
                    Receiver_Name = t.Receivers.Select(t1 => new Model.DTO.OUTPUT.Letter_Part.Receiver_Name_DTO
                    {
                        FirstName = t1.User.FirstName,
                        Lastname = t1.User.LastName
                    }).ToList(),
                })
                .ToListAsync();




            return Ok(Comments);
        }

        [HttpGet("Send")]
        public async Task<IActionResult> GetSendLetters()
        {
            string? User_Id = User.Claims.FirstOrDefault(t => t.Type == ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(User_Id))
            {
                return Unauthorized();
            }
            var Comments = await _Context.Comments
                .Include(t => t.Sender)
                .Where(t => t.Sender.Id == ulong.Parse(User_Id))
                .Select(t => new Model.DTO.OUTPUT.Letter_Part.Send_Letter_DTO
                {
                    Comment_Id = t.Comment_Id,
                    Description = t.Description,
                    Title = t.Title,
                    Sender_Name = t.Sender.FirstName + " " + t.Sender.LastName
                }).ToListAsync();


            return Ok(Comments);
        }

        [AllowAnonymous]
        [HttpGet("AllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            // Get All Role 
            // Fillter with DTO
            //Role_DTO

            var AllRole = await _Context.Roles.Where(t => t.Id > 2)
                .Select(t => new Model.DTO.OUTPUT.Letter_Part.Role_DTO
                {
                    Id = t.Id,
                    Persian_Name = t.Name_Persian,
                })
                .ToListAsync();

            return Ok(AllRole);
        }
    }
}
