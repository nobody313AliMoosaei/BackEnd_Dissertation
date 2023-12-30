﻿using BusinessLayer.Models.INPUT.Administrator;
using BusinessLayer.Models.INPUT.Teacher;
using BusinessLayer.Utilities;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Dissertation_Project.Controllers.V1
{
    [Authorize(Roles = "Administrator")]
    [ApiVersion("1.0")]
    [Route("API/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AdministratorController : ControllerBase
    {
        private BusinessLayer.Services.Administrator.AdministratorBL _adminBL;
        private BusinessLayer.Services.Teacher.ITeacherManager _teacherManager;
        private BusinessLayer.Services.GeneralService.IGeneralService _generalService;

        public AdministratorController(BusinessLayer.Services.Administrator.AdministratorBL adminbl,
            BusinessLayer.Services.Teacher.ITeacherManager teacherManager,
            BusinessLayer.Services.GeneralService.IGeneralService generalService)
        {
            _adminBL = adminbl;
            _teacherManager = teacherManager;
            _generalService = generalService;
        }

        [HttpPost("GetAllDissertation")]
        public async Task<IActionResult> GetAllDissertation(int pageNumber, int pageSize, [FromBody] BusinessLayer.Models.INPUT.Administrator.FilterDissertationDTO _filter)
        {
            return Ok(await _adminBL.GetAllDissertation(pageNumber, pageSize, _filter));
        }

        [HttpPost("ChangeDissertationStatus")]
        public async Task<IActionResult> ChangeDissertationStatus(long DissertationId, string StatusId)
        {
            return Ok(await _adminBL.ChangeDissertationStatus(DissertationId, StatusId));
        }

        [HttpPost("GetAllUser")]
        public async Task<IActionResult> GetAllUser([FromBody] BusinessLayer.Models.INPUT.Administrator.FilterDissertationDTO _filter, int PageNumber, int PageSize = 5)
        {
            return Ok(await _adminBL.GetAllUsers(_filter, PageNumber, PageSize));
        }

        [HttpGet("GetDissertationStatus")]
        public async Task<IActionResult> GetDissertationStatus()
        {
            return Ok(await _adminBL.GetStatusByType(DataLayer.Tools.BASLookupType.DissertationStatus.ToString()));
        }

        [HttpGet("GetCollegeUni")]
        public async Task<IActionResult> GetColleges()
        {
            return Ok(await _adminBL.GetStatusByType(DataLayer.Tools.BASLookupType.CollegesUni.ToString()));
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody]EditUserDTO userModel)
        {
            return Ok(await _adminBL.UpdateUser(userModel));
        }

        [HttpPut("DeActiveUser")]
        public async Task<IActionResult> DeActiveUser(long UserId)
        {
            return Ok(await _adminBL.DeActiveUser(UserId));
        }

        [HttpPost("AddNewRoleToUser")]
        public async Task<IActionResult> AddNewRoleToUser(long UserId, string NewRole)
        {
            return Ok(await _adminBL.AddNewRoleToUser(UserId, NewRole));
        }

        [HttpGet("GetAllTeachers")]
        public async Task<IActionResult> GetAllTeacher(string Value = "")
        {
            return Ok(await _teacherManager.GetAllTeachers(Value));
        }

        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            return Ok(await _generalService.GetAllRoles());

        }

        [HttpPut("UpdateTeacher")]
        public async Task<IActionResult> UpdateTeacher(long TeacherId, [FromBody]TeacherInModelDTO TeacherModel)
        {
            return Ok(await _teacherManager.UpdateTeacher(TeacherId, TeacherModel));
        }

        [HttpPost("AddNewUser")]
        public async Task<IActionResult> AddNewUser([FromBody] EditUserDTO NewUser, string Role)
        {
            return Ok(await _adminBL.AddNewUser(NewUser, Role));
        }

        [HttpGet("FindUser")]
        public async Task<IActionResult> FindUser(string UNE)
        {
            return Ok(await _adminBL.GetUserBySearch(UNE));
        }

        [HttpPost("UploadDissertation")]
        public async Task<IActionResult> UploadDissertationForUser(long UserId, IFormFile Dis_File, IFormFile Pre_File, [FromForm] NewDissertationDTO DissertationModel)
        {
            return Ok(await _adminBL.UploadDissertationForUser(UserId, DissertationModel, Dis_File, Pre_File));
        }

        [HttpPost("AddNewTeacher")]
        public async Task<IActionResult> AddNewTeacher([FromBody] TeacherInModelDTO newTeacher)
        {
            return Ok(await _teacherManager.AddNewTeacher(newTeacher));
        }

        [HttpGet("GetDissertationByStatus")]
        public async Task<IActionResult> GetDissertation(int XStatus = 1)
        {
            return Ok(await _adminBL.GetDissertationsByStatus(XStatus));
        }


        [HttpPost("SendComment")]
        public async Task<IActionResult> SendComment([FromBody] BusinessLayer.Models.INPUT.CommentInputDTO Comment)
        {
            var result = await _generalService.SendComment(Comment.UserId, Comment.DissertationId, Comment.Title, Comment.Dsr, Comment.CommentId);
            if (result.IsValid)
                return Ok(result);
            return BadRequest(result);
        }

        [HttpPost("Download")]
        public IActionResult DonloadFile([FromBody] string FileAddress)
        {
            try
            {
                var UserID = User.Claims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value;
                if (UserID.IsNullOrEmpty())
                    return Unauthorized("کاربر لاگین نکرده است");

                if (System.IO.File.Exists(FileAddress))
                {
                    var File_Info = new FileInfo(FileAddress);
                    var Filestream = System.IO.File.OpenRead(FileAddress);
                    string contentType = "application/octet-stream";

                    var fileDownloadName = UserID + "__" + File_Info.Name;

                    return File(Filestream, contentType, fileDownloadName);
                }
                return BadRequest();
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpGet("GetTeachersByCollegeRef")]
        public async Task<IActionResult> GetTeachersByCollegeRef(long CollegeRef)
        {
            return Ok(await _teacherManager.GetTeachersCollege(CollegeRef));
        }

        [HttpGet("ExportExcel")]
        public async Task<IActionResult> ExportExcel(long TableID)
        {
            try
            {
                if (TableID == 0)
                    return NotFound("سریال جدول به درستی ارسال نشده است");

                var TableName = (await _generalService.GetApp_Tables())
                    .Where(o => o.Id == TableID).Select(o => o.Title).FirstOrDefault();

                var arraylist = await _adminBL.ExportTable(TableID);

                XLWorkbook xl = new XLWorkbook();
                xl.Worksheets.Add(arraylist);

                MemoryStream mstream = new MemoryStream();
                xl.SaveAs(mstream);
                return File(mstream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{TableName}.xlsx");
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("AddNewCollege")]
        public async Task<IActionResult> AddNewCollege([FromBody] BusinessLayer.Models.INPUT.Administrator.NewCollegeDTO NewCollege)
        {
            return Ok(await _adminBL.InsertNewCollege(NewCollege));
        }

        [HttpPost("DeleteCollege")]
        public async Task<IActionResult> RemoveCollege(long CollegeRef)
        {
            return Ok(await _adminBL.DeleteCollegeById(CollegeRef));
        }

        [HttpGet("GetUserInRole")]
        public async Task<IActionResult> GetUserInRole(long RoleRef)
        {
            return Ok(await _adminBL.GetUserInRole(RoleRef));
        }

        [HttpGet("IsValidUser")]
        public async Task<IActionResult> ValidUser()
        {
            try
            {
                var userid = this.User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
                if (userid.IsNullOrEmpty())
                    return NotFound(false);
                var result = await _generalService.UserIsAdmin(userid.Val64());
                if (result)
                    return StatusCode((int)HttpStatusCode.Found, result);
                return NotFound(result);
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
