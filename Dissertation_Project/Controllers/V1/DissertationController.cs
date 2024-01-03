﻿using BusinessLayer.Services.Dissertation;
using BusinessLayer.Utilities;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace Dissertation_Project.Controllers.V1
{
    [Authorize(Roles = "Student")]
    [ApiVersion("1.0")]
    [Route("API/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class DissertationController : ControllerBase
    {
        private DissertationBL _dissertationBL;
        private BusinessLayer.Services.GeneralService.IGeneralService _generalService;
        private BusinessLayer.Services.Administrator.AdministratorBL _adminBL;

        public DissertationController(DissertationBL dissertationBL,
            BusinessLayer.Services.GeneralService.IGeneralService generalService,
            BusinessLayer.Services.Administrator.AdministratorBL adminBL)
        {
            _dissertationBL = dissertationBL;
            _generalService = generalService;
            _adminBL = adminBL;
        }

        [HttpPost]
        public async Task<IActionResult> PreRegister(IFormFile DissertationFile, IFormFile ProFile,
            [FromForm] BusinessLayer.Models.INPUT.Dissertation.PreRegisterDataDTO PreRegisterData)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest("اطلاعات ارسال شده ناقص مي‌باشد");

                var userId = User.Claims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value;
                if (userId.IsNullOrEmpty())
                    return Unauthorized("کاربر لاگین نکرده است");

                var Result = await _dissertationBL.PreRegister(userId.Val64(), DissertationFile, ProFile, PreRegisterData);

                if (Result.IsValid)
                    return Ok(Result);
                return BadRequest(Result);
            }
            catch (Exception ex)
            {
                return BadRequest($"خطا در اجرای برنامه : {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentDissertation()
        {
            try
            {
                var UserID = User.Claims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value;
                if (UserID.IsNullOrEmpty())
                    return Unauthorized("کاربر لاگین نکرده است");

                var dissertation = await _dissertationBL.GetCurrentDissertation(UserID.Val64());
                if (dissertation == null)
                    return NotFound("پایان نامه در جريانی وجود ندارد");
                return Ok(dissertation);
            }
            catch
            {
                return BadRequest("خطا در اجراي برنامه");
            }
        }

        [HttpPost("UpdateDissertation")]
        public async Task<IActionResult> UpdateDissertation(long Dis_Id, IFormFile DissertationFile, IFormFile ProFile,
            [FromForm] BusinessLayer.Models.INPUT.Dissertation.PreRegisterDataDTO PreRegisterData)
        {
            var UserID = User.Claims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value;
            if (UserID.IsNullOrEmpty())
                return Unauthorized("کاربر لاگین نکرده است");
            var Result = await _dissertationBL.UpdateDissertation(DissertationFile, ProFile, new BusinessLayer.Models.INPUT.Dissertation.UpdateDissertationDTO
            {
                Abstract = PreRegisterData.Abstract,
                CollegeRef = PreRegisterData.CollegeRef,
                Dissertation_Id = Dis_Id,
                FirstName = PreRegisterData.FirstName,
                KeyWords = PreRegisterData.KeyWords,
                LastName = PreRegisterData.LastName,
                StudentId = UserID.Val64(),
                Teacher1 = PreRegisterData.Teacher_1,
                Teacher2 = PreRegisterData.Teacher_2,
                Teacher3 = PreRegisterData.Teacher_3,
                TermNumber = PreRegisterData.Term_Number,
                TitleEnglish = PreRegisterData.Title_English,
                TitlePersian = PreRegisterData.Title_Persian,
            });

            if (Result.IsValid)
                return Ok(Result);
            return BadRequest(Result);

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
        public async Task<IActionResult> DonloadFile([FromBody] string FileAddress)
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
                return NotFound();
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpGet("GetAllDissertationOfUesr")]
        public async Task<IActionResult> GetAllDissertationOfUesr()
        {
            var userId = this.User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            if (userId.IsNullOrEmpty())
                return Unauthorized("کاربر لاگین نکرده است");
            return Ok(await _generalService.GetAllDissertationOfUesr(userId.Val64()));
        }

        [HttpGet("IsValidUser")]
        public async Task<IActionResult> ValidUser()
        {
            try
            {
                var userid = this.User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
                if (userid.IsNullOrEmpty())
                    return NotFound(false);
                var result = await _generalService.UserIsStudent(userid.Val64());
                if (result)
                    return StatusCode((int)HttpStatusCode.Found, result);
                return NotFound(result);
            }
            catch
            {
                return NotFound(false);
            }
        }

        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] BusinessLayer.Models.INPUT.SignUp.UpdateUserDTO modelUser)
        {
            var UserId = this.User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            if (UserId.IsNullOrEmpty())
                return Unauthorized("کاربر لاگین نکرده است");

            var result = await _adminBL.UpdateUser(new BusinessLayer.Models.INPUT.Administrator.EditUserDTO
            {
                UserId = UserId.Val64(),
                CollegeRef = modelUser.CollegeRef,
                FirstName = modelUser.FirstName,
                LastName = modelUser.LastName,
                NationalCode = modelUser.NationalCode,
                Teacher1_Ref = modelUser.Teacher_1,
                Teacher2_Ref = modelUser.Teacher_2,
                Teacher3_Ref = modelUser.Teacher_3,
                UserName = modelUser.UserName
            });
            if (result.IsValid)
                return Ok(result);
            return BadRequest(result);
        }

        [Obsolete("deprecated By AliMoosaei",true)]
        [HttpGet("GetAllDissertationOfTeacher")]
        public async Task<IActionResult> GEtAllDissertationsOfTEacher()
        {
            var UserId = this.User.Claims.Where(o => o.Type == ClaimTypes.NameIdentifier).FirstOrDefault()?.Value;
            if (UserId.IsNullOrEmpty()) return Unauthorized("کاربر لاگین نکرده است");

            var result = await _dissertationBL.GetAllDissertationOfTeacher(UserId.Val64());
            if (result == null)
                return BadRequest();
            return Ok(result);
        }


    }
}
