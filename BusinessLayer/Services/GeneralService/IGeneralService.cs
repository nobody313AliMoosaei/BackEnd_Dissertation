﻿using BusinessLayer.Models;
using BusinessLayer.Models.OUTPUT.Administrator;
using BusinessLayer.Models.OUTPUT.General;
using BusinessLayer.Models.OUTPUT.Teacher;
using BusinessLayer.Services.UploadFile;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.GeneralService
{
    public interface IGeneralService
    {
        // Display CollegeUni
        Task<string?> DisplayCollegeUni(int code);
        Task<List<DataLayer.Entities.Baslookup>?> GetAllCollegeUni();
        // Add Role To User
        Task<ErrorsVM> AddRoleToUser(DataLayer.Entities.Users? user, string RoleUser);

        // LogOut
        Task<ErrorsVM> LogOut();

        // Change Status Dissertation
        Task<ErrorsVM> ChangeDissertationStatus(long DissertationID, string DissertationStatus);

        // Upload File
        Task<ResultUploadFile> UploadFile(IFormFile MainFile);

        // Get Teachers
        Task<List<TeacherOutModelDTO>> GetAllTeacher(string Value = "");
        Task<List<TeacherOutModelDTO>> GetCollegeTeacher(long CollegeId);

        // send Comment
        Task<ErrorsVM> SendComment(long UserId, long DissertationId, string Title, string Dsr, long CommentId = 0);

        // Add Teacher To User 


        // GetAll Roles
        Task<List<Models.OUTPUT.Administrator.StatusModelDTO>> GetAllRoles();

        // GetAll DissertationStatus

        Task<string?> DisplayDissertationstatus(int code);

        Task<List<StatusModelDTO>?> GetAllDissertationStatus();

        Task<List<StatusModelDTO>> GetStatus(string StatusType);

        // GetAll CollegesUni
        Task<List<Models.OUTPUT.Administrator.StatusModelDTO>> GetAllCollegesUni();

        // Download File
        Models.OUTPUT.DownloadOutModelDTO? DownloadFileFormRoot(string FileAddress);

        // GetUserById
        Task<UserModelDTO> GetUserById(long UserId);

        Task<List<BusinessLayer.Models.OUTPUT.General.CommentOutPutModelDTO>> GetAllDissertationComments(long DissertationId, int PageNumber, int PageSize);

        Task<List<StatusModelDTO>> GetApp_Tables();

        //Task<List<CommentOutPutModelDTO>> GetAllReplayCommentsByCommentId(long DissertationId, long CommentId, int PageNumber, int PageSize);

        Task<UserModelDTO> GetDataFromAPI(string NationalCode, DateTime BirthDate);

        Task<Models.OUTPUT.General.ReportCountSystemDTO?> ReportCountSystem();

        Task<List<Models.OUTPUT.Dissertation.DissertationModelOutPut>> GetAllDissertationOfUesr(long UserId);

        Task<bool> UserIsEmployee(long UserId);

        Task<bool> UserIsAdmin(long UserId);

        Task<bool> UserIsStudent(long UserId);
    }
}
